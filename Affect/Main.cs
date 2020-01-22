using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Library.Model;
using Twain;
using Common.Configuration;
using System.Text.RegularExpressions;
using Common;
using BarcodeLib;
using BarcodeLib.BarcodeReader;
using System.Drawing.Drawing2D;
using Library.Service;
using System.Collections;
using System.Security.AccessControl;

namespace Affect
{
    public partial class Main : Form
    {
        private List<string> lstImgPath = new List<string>();//图片地址集合
        public static List<string> lstBillNo = new List<string>();//单号
        private List<ScanImg> lstScanImg = new List<ScanImg>();//扫描图像对象集合
        private List<ScanImg> lstCashImg = new List<ScanImg>();//缓存的图像对象集合
        private MyOpaqueLayer m_OpaqueLayer = null;//半透明蒙板层
        private AffectService affSrv = new AffectService();
        private string liuNo;
        private string selectBillNo;
        private int scanCount = 0;
        private Twain32 mTwain = new Twain32();
        private int mImageIndex = 0;
        private int totalImageIndex = 0;
        private string mRunPath = "";
        private string mImagePath = "";
        private string liuImagePath = "";

        int row = 0; //排序图片
        private bool addScan = false;
        //private bool isWorking = false;
        //Task识别条码用到的变量
        public static readonly Queue<ScanImg> scanQueue = new Queue<ScanImg>();
        List<ScanImg> taskListScan = new List<ScanImg>(); //条码识别后的对象集合
        private List<ScanImg> lstInitScanImg = new List<ScanImg>();//初始扫描对象集合
        private List<ScanImg> validList = new List<ScanImg>();  //经过验证后进入条码队列的集合
        //后台提交图片用到的变量
        private int errorProcCount = 0;
        private int runingProcCount = 0;
        private string curRuningLiuNo = string.Empty;
        private string mWaitProcessPath = ""; //待处理文件夹
        private string mErrorProcessPath = ""; //处理错误文件夹
        private string zipPath = "";
        //条码识别前移需增加的变量

        string tmpBarcode = "", curBarcode = "";
        //文件排列顺序

        //系统状态
        //0:系统正常；9:系统出现异常需重新登录才能正常使用
        private int systemStatus = 0;

        #region Event

        private void subBtn_Click(object sender, EventArgs e)
        {
            if (systemStatus == 9)
            {
                MessageBox.Show("系统异常,请重新登录!");
                return;
            }
            if (lstCashImg.Count(t => t.billNo.IndexOf("无条码") > -1) > 0)
            {
                MessageBox.Show("存在无条码不允许提交!");
                return;
            }

            if (MessageBox.Show("确定要提交吗？", "询问", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }

            int imgCnt = lstCashImg.Count();
            if (imgCnt > 0)
            {
                MessageBox.Show("当前批次：" + liuNo + "已扫描" + imgCnt + "张图片，开始提交后台。");
                EncryptedNameValueSectionHandler hand = new EncryptedNameValueSectionHandler();
                bool isBackUpload = false;
                if (!string.IsNullOrEmpty(hand.ReadConfig("isBackUpload")))
                {
                    string backFlag = hand.ReadConfig("isBackUpload").ToString();
                    isBackUpload = backFlag.Equals("0");
                }
                if (isBackUpload)
                {
                    uploadImg2();
                }
                else
                {
                    uploadImg();
                }

                MessageBox.Show("提交结束！");
            }
            else
            {
                MessageBox.Show("没有需要提交的图片文件！");
            }

        }

        private void scanBtn_Click(object sender, EventArgs e)
        {
            if (systemStatus == 9)
            {
                MessageBox.Show("系统异常,请重新登录!");
                return;
            }

            string[] dirs = Directory.GetDirectories(mErrorProcessPath);
            if (dirs.Length > 0)
            {
                MessageBox.Show("已经有" + dirs.Length + "个未上传的流水,请点击重新上传再扫描!");
                return;
            }

            if (totalImageIndex > 400)
            {
                MessageBox.Show("扫描已经超过400张,请提交当前流水号!");
                return;
            }

            if (MessageBox.Show("确定要扫描吗？", "询问", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }

            scanCount++;
            curBarcode = "";
            UserInfo curUser = (UserInfo)AffectCacheObject.Instance[Constants.USERKEY];
            //生成流水号
            if (string.IsNullOrEmpty(liuNo))
            {
                //liuNo = curUser.userName + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                liuNo = DateTime.Now.ToString("MMddHHmmss") + curUser.userName;
                //liuNo = "temp";

            }
            //在Iamge文件夹下建立流水号文件夹
            liuImagePath = mImagePath + liuNo + "\\";

            if (Directory.Exists(liuImagePath) == false)
            {
                Directory.CreateDirectory(liuImagePath);
            }
            string twTestResult = twTestSelf();
            if (!string.IsNullOrEmpty(twTestResult))
            {
                MessageBox.Show(twTestResult);
                return;
            }
            #region 扫描开始
            EncryptedNameValueSectionHandler hand = new EncryptedNameValueSectionHandler();
            try
            {
                if (!string.IsNullOrEmpty(hand.ReadConfig("twIndex")))
                {
                    int twIndex = Convert.ToInt32(hand.ReadConfig("twIndex"));
                    int dpi = Convert.ToInt32(hand.ReadConfig("dpi"));

                    Logger.Info("扫描源状态:" + mTwain.IsTwain2Enable);
                    if (!mTwain.IsTwain2Enable)
                    {
                        mTwain.CloseDataSource();
                        mTwain.SourceIndex = twIndex;
                        //为避免扫描源已打开的错误，在打开前先关闭扫描源
                        mTwain.OpenDataSource();
                    }
                    mTwain.Capabilities.XferMech.Set(TwSX.File);
                    //mTwain.Capabilities.ImageFileFormat.Set(TwFF.Jfif);
                    //mTwain.Capabilities.JpegQuality.Set(TwJQ.Low);
                    mTwain.Capabilities.XResolution.Set(dpi);
                    mTwain.Capabilities.YResolution.Set(dpi);
                    mTwain.Capabilities.PixelType.Set(TwPixelType.RGB);
                    mTwain.ShowUI = hand.ReadConfig("isShowConfig") == "T" ? true : false;
                    mTwain.Acquire();

                }
                else
                {
                    MessageBox.Show("请先在设置中配置扫描仪再扫描!");
                }

            }
            catch (Exception)
            {

                throw;
            }
            #endregion

        }

        private string twTestSelf()
        {
            string result = string.Empty;
            int j = -1;

            EncryptedNameValueSectionHandler hand = new EncryptedNameValueSectionHandler();

            if (string.IsNullOrEmpty(hand.ReadConfig("twName")))
            {
                result = "请先在设置中配置扫描仪再扫描!";

            }
            else
            {
                string tmpTw = string.Empty;
                bool twFind = false;
                for (int i = 0; i < mTwain.SourcesCount; i++)
                {
                    tmpTw = mTwain.GetSourceProductName(i);
                    if (tmpTw.Equals(hand.ReadConfig("twName")))
                    {
                        twFind = true;
                        hand.WriterConfig("twIndex", Convert.ToString(i));
                        j = i;
                        break;
                    }

                }
                if (!twFind)
                {
                    result = "本机没有找到扫描仪" + hand.ReadConfig("twName");
                }
                else
                {
                    if (string.IsNullOrEmpty(hand.ReadConfig("dpi")))
                    {
                        result = "请先在设置中配置扫描仪的预设DPI值再扫描!";

                    }
                    else
                    {
                        string tmpDpi = string.Empty;
                        bool dpiFind = false;
                        mTwain.SourceIndex = j;
                        mTwain.OpenDataSource();
                        var _resolutions = mTwain.Capabilities.XResolution.Get();
                        for (var i = 0; i < _resolutions.Count; i++)
                        {
                            tmpDpi = _resolutions[i].ToString();
                            if (tmpDpi.Equals(hand.ReadConfig("dpi")))
                            {
                                dpiFind = true;
                                break;
                            }

                        }
                        mTwain.CloseDataSource();
                        if (!dpiFind)
                        {
                            result = "在当前扫描仪没有找到预设的DPI值,请重新设置扫描仪的DPI值!";
                        }
                    }
                }
            }
            return result;
        }

        private void setUpBtn_Click(object sender, EventArgs e)
        {
            FormConfig formConfig = new FormConfig();
            formConfig.StartPosition = FormStartPosition.CenterScreen;
            formConfig.ShowDialog();
        }

        private void Main_Load(object sender, EventArgs e)
        {

            mRunPath = System.IO.Directory.GetCurrentDirectory() + "\\";
            mImagePath = mRunPath + "Image\\";

            if (Directory.Exists(mImagePath) == false)
            {
                Directory.CreateDirectory(mImagePath);
            }

            mErrorProcessPath = mRunPath + "errorproc\\";
            if (Directory.Exists(mErrorProcessPath) == false)
            {
                Directory.CreateDirectory(mErrorProcessPath);
            }

            zipPath = mRunPath + "\\ZIP\\";
            if (Directory.Exists(zipPath) == false)
            {
                Directory.CreateDirectory(zipPath);
            }

            //增加系统自检，判断是否可以进行扫描
            systemSelfTest();
            try
            {
                mTwain.Language = TwLanguage.CHINESE_SINGAPORE;
                mTwain.IsTwain2Enable = false;
                mTwain.OpenDSM();

                mTwain.FileXferEvent += twFileXfer;
                mTwain.SetupFileXferEvent += twEndXfer;
                mTwain.AcquireError += AcquireError;

                mTwain.AcquireCompleted += twEndAcquire;
            }
            catch (Exception)
            {
                MessageBox.Show("请先设置扫描源");
            }
            //listView1.ListViewItemSorter = new ListViewItemComparer();
            listView1.ContextMenuStrip = contextMenuStrip1;
            listView1.LargeImageList = imageList;
            listView1.View = View.LargeIcon;
            imageList.ImageSize = new Size(96, 126);


            UserInfo curUser = (UserInfo)AffectCacheObject.Instance[Constants.USERKEY];

            this.label3.Text = "当前登录用户:" + curUser.userName;
            this.label4.Text = "版权所有 湖南步步高翔龙软件有限公司 FSSC出品  ";
            this.treeView1.ItemHeight = 20;

            var task = Task.Run(() =>
            {
                //用Task处理条码识别
                dailyMaintenanceTask();
            });


        }

        private void AcquireError(object sender, Twain32.AcquireErrorEventArgs e)
        {
            MessageBox.Show(e.Exception.Message);
            Logger.Error(liuNo + " 时间 " + System.DateTime.Now.ToString("YYYY-MM-DD hh:mm:ss") + "扫描异常：" + e.Exception.Message);

            //扫描结束后，初始化基础数据
            mTwain.CloseDataSource();
            totalImageIndex += mImageIndex;
            mImageIndex = 0;
            lstImgPath.Clear();
            lstScanImg.Clear();
            lstInitScanImg.Clear();
            taskListScan.Clear();
            addScan = false;
            //this.treeView1.Nodes[0].Text = this.treeView1.Nodes[0].Name + "(" + lstCashImg.Count + ")";
        }

        private void twFileXfer(object sender, Twain32.FileXferEventArgs e)
        {
            string curFileName = e.ImageFileXfer.FileName;
            bool fileStatus = File.Exists(curFileName);
            Logger.Info("当前图片:" + curFileName + ",是否存在:" + fileStatus);
            if (fileStatus)
            {
                mImageIndex++;
                if (addScan)
                {
                    string currentBillNo = this.treeView1.SelectedNode.Name;

                    using (Image img = Image.FromFile(curFileName))
                    {
                        try
                        {
                            ScanImg addImg = loadScanImgInfo(curFileName, currentBillNo, currentBillNo, false, lstCashImg.Count(t => t.billNo.Equals(currentBillNo)) + 1, img.Width, img.Height);
                            lstScanImg.Add(addImg);

                            lstCashImg.Insert(lstCashImg.FindLastIndex(t => t.billNo.Equals(currentBillNo)) + 1, addImg);

                            string fileName = addImg.storePath.Split('\\').Last();
                            if (!imageList.Images.Keys.Contains(fileName))
                                imageList.Images.Add(fileName, img);

                            var group = this.listView1.Groups[treeView1.SelectedNode.Name];

                            ListViewItem lvi = new ListViewItem();

                            lvi.ImageKey = fileName;

                            lvi.Text = "文档" + addImg.orderNum;
                            lvi.Name = addImg.billNo + "_" + addImg.orderNum;
                            lvi.Tag = addImg.storePath;

                            group.Items.Add(lvi);   //分组添加子项

                            //this.listView1.Items.Add(lvi);

                            listView1.Items.Insert(lstCashImg.FindLastIndex(t => t.billNo.Equals(currentBillNo)), lvi);

                            treeView1.SelectedNode.Text = this.treeView1.SelectedNode.Name + "(" + lstCashImg.Count(t => t.billNo.Equals(currentBillNo)) + ")";

                        }
                        catch (Exception)
                        {
                            throw;
                        }
                        finally
                        {
                            img.Dispose();
                        }
                    }
                }
                else
                {
                    //建立扫描图像对象
                    ScanImg img = new ScanImg();
                    img.scanNum = mImageIndex;
                    img.storePath = curFileName;

                    bool isCode = false;
                    int num = 1;
                    bool isBarcode = false;
                    string billReg = AffectCacheObject.Instance[Constants.BILL_REGULAR].ToString();

                    tmpBarcode = ScanBarCode2(img.storePath);
                    img.billNo = tmpBarcode;

                    if (tmpBarcode.Length > 0)
                    {
                        //调用正则表达式验证单号
                        isBarcode = Regex.IsMatch(tmpBarcode, billReg);
                        Logger.Info("条码:" + tmpBarcode + "=,正则验证规则" + billReg + "=,验证结果:" + isBarcode);
                        //isBarcode = true;

                        if (isBarcode)
                        {
                            img.isBarcode = "1";
                        }
                    }

                    if (!lstBillNo.Contains(img.billNo))
                    {
                        //如果有条码
                        if (!string.IsNullOrEmpty(img.billNo) && img.isBarcode == "1")
                        {
                            lstBillNo.Add(img.billNo);
                            tmpBarcode = img.billNo;
                            curBarcode = tmpBarcode;
                            isCode = true;
                            num = 1;
                        }
                        //如果开头无条码
                        else if (string.IsNullOrEmpty(curBarcode))
                        {
                            img.billNo = "无条码" + scanCount;
                            img.isBarcode = "1";
                            lstBillNo.Add(img.billNo);
                            tmpBarcode = img.billNo;
                            curBarcode = tmpBarcode;
                            isCode = true;
                            num = 1;
                        }
                        //否则普通附件
                        else
                        {
                            tmpBarcode = curBarcode;
                            img.billNo = tmpBarcode;
                            img.isBarcode = "0";
                            num = lstCashImg.Count(t => t.billNo.Equals(img.billNo)) + 1;
                        }
                    }
                    //如果识别出相同条码 累加到相同单号下
                    else
                    {
                        tmpBarcode = img.billNo;
                        num = lstCashImg.Count(t => t.billNo.Equals(img.billNo)) + 1;
                    }

                    //simg = Image.FromFile(img.storePath);
                    if (lstScanImg.Count(t => t.storePath.Equals(img.storePath)) == 0
                        && lstCashImg.Count(t => t.storePath.Equals(img.storePath)) == 0
                        )
                    {
                        img = loadTaskScanImgInfo(img.storePath, tmpBarcode, curBarcode, isCode, num, 0, 0);
                        //图片条码展示
                        ShowList(img);

                        //加入当前扫描集合
                        lstScanImg.Add(img);

                        lstCashImg.Add(img);

                        TreeNode liuNode = FindTreeNodByValue(liuNo);
                        if (liuNode == null)
                        {
                            TreeNode tn = new TreeNode();
                            tn.Name = liuNo;
                            tn.Text = liuNo;
                            //tn.NodeFont = new System.Drawing.Font("黑体", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(134)));
                            tn.ContextMenuStrip = contextMenuStrip4;
                            treeView1.Nodes.Add(tn);
                        }

                        TreeNode node = FindTreeChildNodByValue(img.billNo);
                        if (node == null)
                        {
                            node = new TreeNode();

                            node.Name = img.billNo;
                            node.Text = img.billNo + "(" + lstCashImg.Count(t => t.billNo.Equals(img.billNo)) + ")";
                            //billtn.NodeFont = new System.Drawing.Font("黑体", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(134)));

                            node.ContextMenuStrip = contextMenuStrip1;
                            liuNode = FindTreeNodByValue(liuNo);
                            liuNode.Nodes.Add(node);

                            treeView1.NodeMouseClick -= new TreeNodeMouseClickEventHandler(Node_Click);
                            treeView1.AfterLabelEdit -= new NodeLabelEditEventHandler(AfterLabel_Edit);
                            treeView1.MouseDown -= new MouseEventHandler(treeView1_MouseDown);

                            treeView1.NodeMouseClick += new TreeNodeMouseClickEventHandler(Node_Click);
                            treeView1.AfterLabelEdit += new NodeLabelEditEventHandler(AfterLabel_Edit);
                            treeView1.MouseDown += new MouseEventHandler(treeView1_MouseDown);
                            treeView1.ExpandAll();
                        }
                        else
                        {
                            node.Text = img.billNo + "(" + lstCashImg.Count(t => t.billNo.Equals(img.billNo)) + ")";
                        }
                    }
                }
            } 
            this.treeView1.Nodes[0].Text = this.treeView1.Nodes[0].Name + "(" + lstCashImg.Count + ")";
        }

        private void twEndXfer(object sender, Twain32.SetupFileXferEventArgs e)
        {
            string filename = string.Empty;
            filename = Guid.NewGuid().ToString();

            string FileNm = liuImagePath + filename + ".jpg";
            e.FileName = FileNm;

            ////加入到图片地址集合
            //lstImgPath.Add(FileNm);

            //建立扫描图像对象
            //ScanImg s = new ScanImg();
            //s.scanNum = mImageIndex;
            //s.storePath = FileNm;
            //lstInitScanImg.Add(s);
            //mImageIndex++;
        }

        //private void twEndXfer(object sender, Twain32.EndXferEventArgs e)
        //{
        //    string filename = string.Empty;
        //    filename = Guid.NewGuid().ToString();
        //    string FileNm = mImagePath + filename + ".jpg";
        //    e.Image.Save(FileNm, ImageFormat.Jpeg);
        //    ////加入到图片地址集合
        //    lstImgPath.Add(FileNm);
        //    e.Image.Dispose();
        //    //建立扫描图像对象
        //    ScanImg s = new ScanImg();
        //    s.scanNum = mImageIndex;
        //    s.storePath = FileNm;
        //    lstInitScanImg.Add(s);
        //    mImageIndex++; 
        //}

        private void twEndAcquire(object sender, EventArgs e)
        {

            MessageBox.Show("扫描结束,已扫描" + mImageIndex + "张!");

            #region 获取扫描后图片
            //String[] files = Directory.GetFiles(liuImagePath);
            //lstImgPath.Clear();
            //for (int i = 0; i < files.Length; i++)
            //{
            //    if (files[i].EndsWith(".png", true, null)
            //        || files[i].EndsWith(".bmp", true, null)
            //        || files[i].EndsWith(".jpg", true, null)
            //        || files[i].EndsWith(".gif", true, null))
            //    {
            //        lstImgPath.Add(files[i]);
            //    }
            //    //建立扫描图像对象
            //    ScanImg s = new ScanImg();
            //    s.scanNum = i;
            //    s.storePath = files[i];
            //    lstInitScanImg.Add(s);
            //}

            #endregion

            Logger.Info("条码识别结束。获取条码{0}个，需要展现图片有{1}张。", lstBillNo.Count, lstScanImg.Count);

            #region 建立扫描图像对象及条码识别

            if (!addScan)
            {
             

                //foreach (ScanImg sImg in lstScanImg)
                //{
                //    lstCashImg.Add(sImg);
                //}

                //treeView1.Nodes.Clear();

                //TreeNode liuNode = FindTreeNodByValue(liuNo);
                //if (liuNode == null)
                //{
                //    TreeNode tn = new TreeNode();
                //    tn.Name = liuNo;
                //    tn.Text = liuNo;
                //    //tn.NodeFont = new System.Drawing.Font("黑体", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(134)));
                //    tn.ContextMenuStrip = contextMenuStrip4;
                //    treeView1.Nodes.Add(tn);
                //}

                //foreach (string billNo in lstBillNo)
                //{
                //    TreeNode billtn = new TreeNode();

                //    billtn.Name = billNo;
                //    billtn.Text = billNo + "(" + lstCashImg.Count(t => t.billNo.Equals(billNo)) + ")";
                //    //billtn.NodeFont = new System.Drawing.Font("黑体", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(134)));

                //    billtn.ContextMenuStrip = contextMenuStrip1;
                //    liuNode = FindTreeNodByValue(liuNo);
                //    liuNode.Nodes.Add(billtn);
                //}

                //treeView1.NodeMouseClick -= new TreeNodeMouseClickEventHandler(Node_Click);
                //treeView1.AfterLabelEdit -= new NodeLabelEditEventHandler(AfterLabel_Edit);
                //treeView1.MouseDown -= new MouseEventHandler(treeView1_MouseDown);

                //treeView1.NodeMouseClick += new TreeNodeMouseClickEventHandler(Node_Click);
                //treeView1.AfterLabelEdit += new NodeLabelEditEventHandler(AfterLabel_Edit);
                //treeView1.MouseDown += new MouseEventHandler(treeView1_MouseDown);
                //treeView1.ExpandAll();
                ////}

            }
            //扫描结束后，初始化基础数据
            mTwain.CloseDataSource();
            totalImageIndex += mImageIndex;
            mImageIndex = 0;
            lstImgPath.Clear();
            lstScanImg.Clear();
            lstInitScanImg.Clear();
            taskListScan.Clear();
            addScan = false;
            #endregion

            //ShowUI();

        }

        private void treeView1_MouseDown(object sender, MouseEventArgs e)
        {
            treeView1.SelectedNode = treeView1.GetNodeAt(e.X, e.Y);
            if (treeView1.SelectedNode != null)
                selectBillNo = treeView1.SelectedNode.Name;
        }

        //private void treeView1_MouseClick(object sender, MouseEventArgs e)
        //{
        //    if (e.Button == MouseButtons.Right)
        //    {
        //        treeView1.SelectedNode = clickNode;
        //        treeView1.LabelEdit = true;
        //        clickNode.BeginEdit();
        //    }
        //}

        private void AfterLabel_Edit(object sender, NodeLabelEditEventArgs e)
        {

            if (e.Label != null)
            {
                if (e.Label.Length > 0)
                {
                    if (e.Label.IndexOfAny(new char[] { '@', '.', ',', '!' }) == -1)
                    {
                        foreach (var item in lstCashImg)
                        {
                            if (item.billNo.Equals(selectBillNo))
                                item.billNo = e.Label;
                        }

                        foreach (ListViewItem item in this.listView1.Items)
                        {
                            if (item.Name.Split('_')[0] == selectBillNo)
                            {
                                string pattern = @"[\u0000-\uffff]*[_]";
                                string replacement = e.Label + "_";
                                item.Name = Regex.Replace(item.Name, pattern, replacement);

                            }
                        }

                        var idx = lstBillNo.FindIndex(t => t.Equals(selectBillNo));
                        lstBillNo[idx] = e.Label;

                        e.CancelEdit = true;
                        e.Node.EndEdit(false);
                        e.Node.Text = e.Label + "(" + lstCashImg.Count(t => t.billNo == e.Label) + ")";
                        this.listView1.Groups[e.Node.Name].Header = e.Label;
                        this.listView1.Groups[e.Node.Name].Name = e.Label;
                        e.Node.Name = e.Label;

                    }
                    else
                    {
                        e.CancelEdit = true;
                        MessageBox.Show("包含特殊字符,编辑失败");
                        e.Node.BeginEdit();
                    }
                }
                else
                {
                    e.CancelEdit = true;
                    MessageBox.Show("编辑单号不允许为空");
                    e.Node.BeginEdit();
                }
                this.treeView1.LabelEdit = false;
            }
        }

        private void Node_Click(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode treeNode = e.Node;
            List<Control> list = new List<Control>();
            if (treeNode.Parent != null)
            {
                this.label2.Text = "当前选中单号" + treeNode.Name;
                var prevNode = e.Node.PrevNode;
                while (prevNode != null)
                {
                    prevNode = prevNode.PrevNode;
                }


                this.listView1.Focus();

                this.listView1.EnsureVisible(listView1.Items.IndexOf(listView1.Groups[treeView1.SelectedNode.Name].Items[0]));

                GroupsFocus(e.Node.Name);

            }
            else
            {
                this.label2.Text = "";
            }
        }

        private void Img_Click(object sender, EventArgs e)
        {
            ShowOpaqueLayer(new List<Control>() { (PictureBox)sender }, 125, true);
        }

        private void UpdateBarTool_Click(object sender, EventArgs e)
        {
            if (this.treeView1.SelectedNode == null)
            {
                MessageBox.Show("请点击要修改的条码");
                return;
            }

            InptBNForm inpt = new InptBNForm();
            inpt.StartPosition = FormStartPosition.CenterScreen;
            DialogResult result = inpt.ShowDialog();
            if (result == DialogResult.OK)
            {
                foreach (var item in lstCashImg)
                {
                    if (item.billNo.Equals(selectBillNo))
                        item.billNo = inpt.resultTxt;
                }

                foreach (ListViewItem item in this.listView1.Items)
                {
                    if (item.Name.Split('_')[0] == selectBillNo)
                    {
                        string pattern = @"[\u0000-\uffff]*[_]";
                        string replacement = inpt.resultTxt + "_";
                        item.Name = Regex.Replace(item.Name, pattern, replacement);
                    }
                }

                var idx = lstBillNo.FindIndex(t => t.Equals(selectBillNo));
                lstBillNo[idx] = inpt.resultTxt;

                TreeNode tn = this.treeView1.Nodes[0].Nodes[selectBillNo];

                //e.CancelEdit = true;
                //e.Node.EndEdit(false);
                tn.Text = inpt.resultTxt + "(" + lstCashImg.Count(t => t.billNo == inpt.resultTxt) + ")";
                this.listView1.Groups[selectBillNo].Header = inpt.resultTxt;
                this.listView1.Groups[selectBillNo].Name = inpt.resultTxt;
                tn.Name = inpt.resultTxt;

                this.listView1.EnsureVisible(listView1.Items.IndexOf(listView1.Groups[treeView1.SelectedNode.Name].Items[0]));

                GroupsFocus(treeView1.SelectedNode.Name);
            }

        }

        private void DelLiuNoTool_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("您确定要删除当前" + treeView1.SelectedNode.Name + "流水号吗？", "询问", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }

            //删除单号 
            TreeNode selectedNode = this.treeView1.SelectedNode;
            TreeNodeCollection childNodes = selectedNode.Nodes;

            for (int i = this.listView1.Items.Count - 1; i > -1; i--)
            {
                this.listView1.Items.RemoveAt(i);
            }

            for (int i = this.listView1.Groups.Count - 1; i > -1; i--)
            {
                this.listView1.Groups.RemoveAt(i);
            }

            ////删除图片对象集合里面的 内容
            lstCashImg.RemoveAll(t => t.serialNum == treeView1.SelectedNode.Name);

            lstBillNo.Clear();

            this.treeView1.Nodes.Clear();

            this.imageList.Images.Clear();
            this.listView1.LargeImageList.Images.Clear();

            scanCount = 0;
        }

        private void DelBarTool_Click(object sender, EventArgs e)
        {
            if (this.treeView1.SelectedNode == null)
            {
                MessageBox.Show("请点击要删除的条码");
                return;
            }

            if (this.listView1.Groups.Count == 1)
            {
                MessageBox.Show("当前流水号下至少需要一组条码图片");
                return;
            }

            if (listView1.Groups[treeView1.SelectedNode.Name] == listView1.Groups[0])
            {
                MessageBox.Show("头单无法移除条码");
                return;
            }

            if (MessageBox.Show("您确定要删除当前" + treeView1.SelectedNode.Name + "单号吗？", "询问", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }

            TreeNode prevNode = this.treeView1.SelectedNode.PrevNode;
            string name = this.treeView1.SelectedNode.Name;
            string preNo = string.Empty;
            int preNoCount = 0;

            if (prevNode != null)
            {
                preNo = prevNode.Name;
                preNoCount = lstCashImg.Where(t => t.billNo == preNo).Count();
                prevNode.Text = prevNode.Name + "(" + (preNoCount + lstCashImg.Where(t => t.billNo == this.treeView1.SelectedNode.Name).Count()) + ")";
            }

            if (listView1.Groups[""] == null && prevNode == null)
            {
                listView1.Groups[treeView1.SelectedNode.Name].Header = "无条码";
                listView1.Groups[treeView1.SelectedNode.Name].Name = "";

            }
            else
            {
                for (int i = 0; i < listView1.Groups[name].Items.Count; i++)
                {
                    var item = listView1.Groups[name].Items[i];

                    ListViewItem newItem = new ListViewItem();
                    newItem.Text = item.Text;
                    newItem.Name = item.Name;
                    newItem.ImageKey = item.ImageKey;
                    newItem.Tag = item.Tag;
                    listView1.Groups[preNo].Items.Add(newItem);
                    //listView1.Items.Add(newItem);

                    this.listView1.Items.Insert(lstCashImg.FindLastIndex(t => t.billNo.Equals(treeView1.SelectedNode.Name)) + i, newItem);
                }
                for (int i = listView1.Groups[name].Items.Count - 1; i > -1; i--)
                {
                    var item = listView1.Groups[treeView1.SelectedNode.Name].Items[i];
                    listView1.Items.Remove(item);
                }
            }

            listView1.Groups.Remove(listView1.Groups[name]);

            ////再重新绘制上一个group
            //for (int i = 0; i < listView1.Groups[preNo].Items.Count; i++)
            //{
            //    var lvi = listView1.Groups[preNo].Items[i];
            //    //lvi.ImageKey = preNo + "_" + (i + 1);
            //    lvi.Text = "文档" + (i + 1);
            //    lvi.Name = preNo + "_" + (i + 1);
            //}

            //修改图片对象属性
            foreach (var item in lstCashImg)
            {
                if (item.billNo == treeView1.SelectedNode.Name)
                {
                    item.isBarcode = "0";
                    item.billNo = preNo;
                    item.orderNum = lstCashImg.Count(t => t.billNo == preNo);

                }
            }

            refreshGroup(preNo);

            //移除流水号对象
            lstBillNo.Remove(this.treeView1.SelectedNode.Name);

            this.treeView1.SelectedNode.Remove();

            this.listView1.EnsureVisible(listView1.Items.IndexOf(listView1.Groups[preNo].Items[0]));

            GroupsFocus(listView1.Groups[preNo].Name);

            List<string> names = new List<string>();
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                if (!names.Contains(listView1.Items[i].ImageKey))
                {
                    names.Add(listView1.Items[i].ImageKey);
                }
                else
                {
                    MessageBox.Show(listView1.Items[i].ImageKey + " " + listView1.Items[i].Name + " " + listView1.Items[i].Text);
                }
            }
        }

        private void AppendBarTool_Click(object sender, EventArgs e)
        {
            if (this.treeView1.SelectedNode == null)
            {
                MessageBox.Show("请点击要追加目标条码");
                return;
            }

            if (MessageBox.Show("确定要追加扫描吗？", "询问", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }

            addScan = true;

            #region 扫描开始
            EncryptedNameValueSectionHandler hand = new EncryptedNameValueSectionHandler();
            try
            {
                if (!string.IsNullOrEmpty(hand.ReadConfig("twIndex")))
                {
                    int twIndex = Convert.ToInt32(hand.ReadConfig("twIndex"));
                    int dpi = Convert.ToInt32(hand.ReadConfig("dpi"));


                    mTwain.SourceIndex = twIndex;
                    mTwain.OpenDataSource();
                    mTwain.Capabilities.XferMech.Set(TwSX.File);
                    mTwain.Capabilities.XResolution.Set(dpi);
                    mTwain.Capabilities.YResolution.Set(dpi);
                    mTwain.Capabilities.PixelType.Set(TwPixelType.RGB);
                    mTwain.ShowUI = hand.ReadConfig("isShowConfig") == "T" ? true : false;
                    mTwain.Acquire();
                }
                else
                {
                    MessageBox.Show("请先在设置中配置扫描仪再扫描!");
                }

            }
            catch (Exception)
            {

                throw;
            }
            #endregion

        }

        private void DelAllBarTool_Click(object sender, EventArgs e)
        {
            if (this.treeView1.SelectedNode != null)
            {
                if (MessageBox.Show("您确定要删除当前" + treeView1.SelectedNode.Name + "单号下所有图片吗？", "询问", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    return;
                }

                lstCashImg.RemoveAll(item => item.billNo.Equals(treeView1.SelectedNode.Name) && item.serialNum.Equals(treeView1.SelectedNode.Parent.Name));

                for (int i = this.listView1.Groups[treeView1.SelectedNode.Name].Items.Count - 1; i > -1; i--)
                {
                    var item = this.listView1.Groups[treeView1.SelectedNode.Name].Items[i];
                    this.listView1.Items.RemoveByKey(item.Name);
                }

                this.listView1.Groups.Remove(this.listView1.Groups[treeView1.SelectedNode.Name]);

                lstBillNo.Remove(treeView1.SelectedNode.Name);
                this.treeView1.SelectedNode.Remove();
                this.treeView1.Nodes[0].Text = this.treeView1.Nodes[0].Name + "(" + lstCashImg.Count + ")";

                if (listView1.Groups.Count > 0)
                {
                    this.listView1.EnsureVisible(listView1.Items.IndexOf(listView1.Groups[treeView1.SelectedNode.Name].Items[0]));
                    GroupsFocus(treeView1.SelectedNode.Name);
                }
            }
        }

        private void RefreshTool_Click(object sender, EventArgs e)
        {

        }

        private void DelImgTool_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("您确定要删除该张图片吗？", "询问", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }
            if (this.listView1.SelectedItems.Count > 0)
            {

                var selectItem = this.listView1.SelectedItems[0];

                string name = selectItem.Name.Split('_')[0];
                int orderNum = selectItem.Group.Items.IndexOfKey(selectItem.Name) + 1;

                lstCashImg.RemoveAll(t => (t.billNo == name && t.orderNum == orderNum));

                foreach (TreeNode item in this.treeView1.Nodes[selectItem.Group.Tag.ToString()].Nodes)
                {
                    if (item.Name.Equals(name))
                    {
                        item.Text = name + "(" + lstCashImg.Count(t => t.billNo == name) + ")";
                    }
                }

                this.listView1.Items.RemoveByKey(selectItem.Name);

                refreshGroup(name);

                //MessageBox.Show("移除成功");
            }
            this.treeView1.Nodes[0].Text = this.treeView1.Nodes[0].Name + "(" + lstCashImg.Count + ")";
        }

        private void listView1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right || e.Button == MouseButtons.Left)
            {
                foreach (ListViewItem item in this.listView1.Items)
                {
                    item.BackColor = Color.White;
                }

                if (this.listView1.SelectedItems.Count > 0)
                {
                    this.label2.Text = "当前选中单号" + listView1.SelectedItems[0].Group.Name;
                    string name = this.listView1.SelectedItems[0].Group.Name;
                    int indx = this.listView1.SelectedItems[0].Group.Items.IndexOfKey(listView1.SelectedItems[0].Name);
                    if (e.Button == MouseButtons.Right)
                    {
                        if (indx == 0 && !string.IsNullOrEmpty(name))
                        {
                            ((ListView)sender).ContextMenuStrip = contextMenuStrip2;
                        }
                        else
                        {
                            ((ListView)sender).ContextMenuStrip = contextMenuStrip3;
                        }
                    }

                    ItemsFocus(this.listView1.SelectedItems[0].Group.Name, this.listView1.SelectedItems[0].Name);

                }
                else
                {
                    ((ListView)sender).ContextMenuStrip = null;
                }
            }
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (this.listView1.SelectedItems.Count > 0)
                {
                    BigImgForm bigImg = new BigImgForm(this);
                    bigImg.StartPosition = FormStartPosition.CenterScreen;
                    bigImg.ShowImg(listView1.Groups[listView1.SelectedItems[0].Group.Name].Items.IndexOf(listView1.SelectedItems[0]), this.listView1.SelectedItems[0].Group.Name, this.listView1.SelectedItems[0].Text, this.listView1.SelectedItems[0].Tag.ToString());
                    bigImg.ShowDialog();

                }
            }
        }

        private void DelMainBarTool_Click(object sender, EventArgs e)
        {

            if (this.listView1.SelectedItems.Count > 0)
            {
                if (this.listView1.Groups.IndexOf(listView1.SelectedItems[0].Group) == 0)
                {
                    MessageBox.Show("头单无法移除条码");
                    return;
                }
                if (this.listView1.Groups.Count == 1)
                {
                    MessageBox.Show("当前流水号下至少需要一组条码图片");
                    return;
                }

                if (MessageBox.Show("您确定要移除条码吗？", "询问", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    return;
                }
                var selectItem = this.listView1.SelectedItems[0];

                string name = selectItem.Name.Split('_')[0];

                int preNoIndx = lstBillNo.FindIndex(t => t.Equals(name)) - 1;

                //string preNo = string.IsNullOrEmpty(lstCashImg[preNoIndx].billNo) ? ("无条码" + scanCount) : lstCashImg[preNoIndx].billNo;
                string preNo = string.Empty;
                if (preNoIndx < 0)
                    preNo = "无条码" + scanCount;
                else
                    preNo = lstBillNo[preNoIndx];

                int preCount = lstCashImg.Where(t => t.billNo == preNo).Count();
                var prevNode = this.treeView1.Nodes[selectItem.Group.Tag.ToString()].Nodes[name].PrevNode;


                //移除流水号对象
                lstBillNo.Remove(name);

                //修改缓存图片对象属性
                foreach (var item in lstCashImg)
                {
                    if (item.billNo == name)
                    {
                        item.billNo = preNo;
                        item.orderNum = lstCashImg.Count(t => t.billNo == preNo);
                        item.isBarcode = "0";
                    }
                }

                //修改树
                if (prevNode != null)
                {
                    //preNo = prevNode.Name;
                    //preNoCount = lstCashImg.Where(t => t.billNo == preNo).Count();
                    prevNode.Text = prevNode.Name + "(" + lstCashImg.Where(t => t.billNo == preNo).Count() + ")";
                }

                this.treeView1.Nodes[selectItem.Group.Tag.ToString()].Nodes.RemoveByKey(name);
                //this.listView1.Items.RemoveByKey(selectItem.Name);

                if (listView1.Groups[""] == null && preNoIndx < 0)
                {
                    listView1.Groups[name].Header = "无条码" + scanCount;
                    listView1.Groups[name].Name = "无条码" + scanCount;
                }
                else
                {
                    for (int i = 0; i < selectItem.Group.Items.Count; i++)
                    {
                        //var item = selectItem.Group.Items[i];    //由于下面对选中项进行了位置上移动 ，grop将会变为更新后的group，所以不能这么写
                        var item = listView1.Groups[name].Items[i];

                        ListViewItem newItem = new ListViewItem();
                        newItem.Text = item.Text;

                        newItem.Name = item.Name;

                        newItem.ImageKey = item.ImageKey;
                        newItem.Tag = item.Tag;

                        listView1.Groups[preNo].Items.Add(newItem);
                        //listView1.Items.Add(newItem);
                        listView1.Items.Insert(lstCashImg.FindLastIndex(t => t.billNo.Equals(preNo)) + i, newItem);


                    }

                    for (int i = selectItem.Group.Items.Count - 1; i > -1; i--)
                    {
                        var item = listView1.Groups[name].Items[i];
                        listView1.Items.Remove(item);
                    }
                }

                listView1.Groups.Remove(listView1.Groups[name]);

                refreshGroup(preNo);

                this.listView1.EnsureVisible(listView1.Items.IndexOf(listView1.Groups[preNo].Items[0]));

                GroupsFocus(listView1.Groups[preNo].Name);

                List<string> names = new List<string>();
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    if (!names.Contains(listView1.Items[i].ImageKey))
                    {
                        names.Add(listView1.Items[i].ImageKey);
                    }
                    else
                    {
                        MessageBox.Show(listView1.Items[i].ImageKey + " " + listView1.Items[i].Name + " " + listView1.Items[i].Text);
                    }
                }
            }
            else
            {
                MessageBox.Show("请点击移除的条码图片");
                return;
            }
        }

        private void UpdateImgBarTool_Click(object sender, EventArgs e)
        {

            if (this.listView1.SelectedItems.Count > 0)
            {
                var selectItem = this.listView1.SelectedItems[0];
                string name = selectItem.Group.Name;

                InptBNForm inpt = new InptBNForm();
                inpt.StartPosition = FormStartPosition.CenterScreen;
                DialogResult result = inpt.ShowDialog();
                if (result == DialogResult.OK)
                {
                    foreach (var item in lstCashImg)
                    {
                        if (item.billNo.Equals(name))
                            item.billNo = inpt.resultTxt;
                    }

                    foreach (ListViewItem item in this.listView1.Items)
                    {
                        if (item.Name.Split('_')[0] == name)
                        {
                            string pattern = @"[\u0000-\uffff]*[_]";
                            string replacement = inpt.resultTxt + "_";
                            item.Name = Regex.Replace(item.Name, pattern, replacement);
                        }
                    }

                    var idx = lstBillNo.FindIndex(t => t.Equals(name));
                    lstBillNo[idx] = inpt.resultTxt;

                    TreeNode tn = this.treeView1.Nodes[0].Nodes[name];

                    //e.CancelEdit = true;
                    //e.Node.EndEdit(false);
                    tn.Text = inpt.resultTxt + "(" + lstCashImg.Count(t => t.billNo == inpt.resultTxt) + ")";
                    this.listView1.Groups[name].Header = inpt.resultTxt;
                    this.listView1.Groups[name].Name = inpt.resultTxt;
                    tn.Name = inpt.resultTxt;

                    this.listView1.EnsureVisible(listView1.Items.IndexOf(listView1.SelectedItems[0]));

                    GroupsFocus(listView1.SelectedItems[0].Group.Name);
                }
            }
            else
            {
                MessageBox.Show("请点击要修改的条码图片");
                return;
            }
        }

        private void AddBarTool_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count > 0)
            {
                InptBNForm inpt = new InptBNForm();
                inpt.StartPosition = FormStartPosition.CenterScreen;
                DialogResult result = inpt.ShowDialog();
                if (result == DialogResult.OK)
                {

                    var selectItem = this.listView1.SelectedItems[0];
                    string groupName = selectItem.Group.Name;
                    string name = selectItem.Name.Split('_')[0];
                    int orderNum = selectItem.Group.Items.IndexOfKey(selectItem.Name) + 1;
                    int indx = listView1.Groups.IndexOf(selectItem.Group);
                    //lstCashImg.GroupBy(t => t.billNo).FindIndex(t => t.billNo.Equals(name));
                    //string preNo = string.IsNullOrEmpty(lstCashImg[preNoIndx].billNo) ? "无条码" : lstCashImg[preNoIndx].billNo;
                    var currentNode = this.treeView1.Nodes[selectItem.Group.Tag.ToString()].Nodes[name];

                    var isBar = false;
                    var isAdd = false;
                    var isStopDel = false;

                    //修改缓存图片对象属性
                    foreach (var item in lstCashImg)
                    {
                        if (item.billNo == name)
                        {
                            //if (item.orderNum == orderNum)
                            if (item.orderNum == orderNum)
                            {
                                item.isBarcode = "1";
                                isBar = true;
                            }
                            if (isBar && item.orderNum >= orderNum)
                                item.billNo = inpt.resultTxt;
                        }
                    }

                    //修改树节点
                    if (currentNode != null)
                    {
                        //preNo = currentNode.Name;
                        //currentCount = lstCashImg.Where(t => t.billNo == name).Count();
                        currentNode.Text = currentNode.Name + "(" + lstCashImg.Where(t => t.billNo == selectItem.Group.Name).Count() + ")";
                    }

                    //新增数节点
                    TreeNode newNode = new TreeNode();
                    newNode.Name = inpt.resultTxt;
                    newNode.Text = inpt.resultTxt + "(" + lstCashImg.Count(t => t.billNo.Equals(inpt.resultTxt)) + ")";
                    newNode.Tag = liuNo;
                    newNode.ContextMenuStrip = contextMenuStrip1;
                    treeView1.Nodes[selectItem.Group.Tag.ToString()].Nodes.Add(newNode);

                    lstBillNo.Add(inpt.resultTxt);

                    //this.listView1.Items.RemoveByKey(selectItem.Name);

                    ListViewGroup group = new ListViewGroup();  //创建BilNO分组
                    group.Header = inpt.resultTxt;
                    group.Name = inpt.resultTxt;
                    group.Tag = liuNo;

                    listView1.Groups.Add(group);

                    for (int i = 0; i < selectItem.Group.Items.Count; i++)
                    {
                        //var item = selectItem.Group.Items[i];    //由于下面对选中项进行了位置上移动 ，grop将会变为更新后的group，所以不能这么写
                        var item = listView1.Groups[groupName].Items[i];

                        ListViewItem newItem = new ListViewItem();
                        //lvi.ImageKey = preNo + "_" + (i + 1);
                        newItem.Text = item.Text;
                        newItem.Name = item.Name;
                        newItem.ImageKey = item.ImageKey;
                        newItem.Tag = item.Tag;

                        if (item == selectItem)
                            isAdd = true;
                        if (isAdd)
                        {
                            group.Items.Add(newItem);
                            listView1.Items.Add(newItem);
                        }
                    }


                    for (int i = selectItem.Group.Items.Count - 1; i > -1; i--)
                    {
                        var item = listView1.Groups[groupName].Items[i];

                        if (!isStopDel)
                            listView1.Items.Remove(item);
                        if (item == selectItem)
                            isStopDel = true;
                    }

                    refreshGroup(group.Name);

                    List<string> names = new List<string>();
                    for (int i = 0; i < listView1.Items.Count; i++)
                    {
                        if (!names.Contains(listView1.Items[i].ImageKey))
                        {
                            names.Add(listView1.Items[i].ImageKey);
                        }
                        else
                        {
                            MessageBox.Show(listView1.Items[i].ImageKey + " " + listView1.Items[i].Name + " " + listView1.Items[i].Text);
                        }
                    }


                    this.treeView1.Nodes[0].Text = this.treeView1.Nodes[0].Name + "(" + lstCashImg.Count + ")";
                }
            }
            else
            {
                MessageBox.Show("请点击要添加为主条码的图片");
                return;
            }
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (runingProcCount > 0)
            {
                MessageBox.Show("有图片上传流程还未完成，不能关闭退出程序!");
                e.Cancel = true;
            }
        }

        private void freshUpBtn_Click(object sender, EventArgs e)
        {
            if (systemStatus == 9)
            {
                MessageBox.Show("系统异常,请重新登录!");
                return;
            }

            string[] dirs = Directory.GetDirectories(mErrorProcessPath);

            if (dirs.Length > 0)
            {
                //先重新登录下保证token最新
                UserInfo user = (UserInfo)AffectCacheObject.Instance[Constants.USERKEY];
                LoginService loginSrv = new LoginService();
                loginSrv.login(user.userName, user.userPwd);
                var task = Task.Run(() =>
                {
                    string tmpLiu = string.Empty;
                    int result = 0;
                    foreach (string item in dirs)
                    {
                        tmpLiu = item.Substring(mErrorProcessPath.LastIndexOf("\\") + 1);
                        if (File.Exists(zipPath + tmpLiu + ".zip"))
                        {
                            File.Delete(zipPath + tmpLiu + ".zip");
                        }
                        Logger.Info("流水号:" + tmpLiu + "重新提交开始上传===" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        Logger.Info("流水号:" + tmpLiu + ",路径:" + item);
                        result = uploadImgBackWork(item);
                        Logger.Info("流水号:" + tmpLiu + "重新提交结束上传===" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        Logger.Info("流水号:" + tmpLiu + "重新提交结束上传===返回结果是" + result);


                    }
                    if (result == 0)
                    {
                        MessageBox.Show("重新提交成功，可继续扫描！");
                    }
                    else
                    {
                        MessageBox.Show("重新提交失败，请联系管理维护人员！");
                    }
                });
                var form = new frmWaitingBox(task); //等待界面
                form.ShowDialog();

            }
            else
            {
                MessageBox.Show("没有提交失败的流水号,不需要重新提交!");
            }
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }
        #endregion

        #region Method

        public Main()
        {
            InitializeComponent();
        }

        private TreeNode FindTreeNodByValue(string value)
        {
            foreach (TreeNode tn in treeView1.Nodes)
            {
                if (tn.Name.Equals(value))
                {
                    return tn;
                }
            }
            return null;
        }

        private TreeNode FindTreeChildNodByValue(string value)
        {
            foreach (TreeNode tn in treeView1.Nodes[0].Nodes)
            {
                if (tn.Name.Equals(value))
                {
                    return tn;
                }
            }
            return null;
        }

        private string ScanBarCode(string fileName)
        {
            string result = string.Empty;
            DateTime now = DateTime.Now;
            Image primaryImage = Image.FromFile(fileName);

            Bitmap pImg = MakeGrayscale3((Bitmap)primaryImage);
            String[] datas = BarcodeReader.read(fileName, BarcodeReader.CODE39);
            return datas.ToString();
        }

        ///  条码识别
        /// </summary>
        /// <param name="fileName"></param>
        private string ScanBarCode2(string fileName)
        {
            string result = string.Empty;
            DateTime now = DateTime.Now;
            //FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            Image primaryImage = Image.FromFile(fileName);
            Bitmap pImg = MakeGrayscale3((Bitmap)primaryImage);
            try
            {

                primaryImage.Dispose();
                primaryImage = null;

                if (pImg != null)
                {
                    using (ZBar.ImageScanner scanner = new ZBar.ImageScanner())
                    {
                        scanner.SetConfiguration(ZBar.SymbolType.None, ZBar.Config.Enable, 0);
                        scanner.SetConfiguration(ZBar.SymbolType.CODE39, ZBar.Config.Enable, 1);
                        scanner.SetConfiguration(ZBar.SymbolType.CODE128, ZBar.Config.Enable, 1);

                        List<ZBar.Symbol> symbols = new List<ZBar.Symbol>();
                        symbols = scanner.Scan((Image)pImg);

                        //symbols = scanner.Scan(primaryImage);

                        if (symbols != null && symbols.Count > 0)
                        {
                            string tmpCode = string.Empty;

                            // symbols.ForEach(s => result += "条码内容:" + s.Data + " 条码质量:" + s.Quality + Environment.NewLine);
                            foreach (ZBar.Symbol sm in symbols)
                            {
                                tmpCode = sm.Data;

                                if (tmpCode.Length > 3)
                                {
                                    result = tmpCode;
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                Logger.Error("条码识别异常:" + ex.Message);
            }
            finally
            {
                //fs.Close();
                //fs.Dispose();
                //fs = null;
                if (pImg != null)
                    pImg.Dispose();
                pImg = null;

            }

            return result;
        }

        /// <summary>
        /// 处理图片灰度
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        public static Bitmap MakeGrayscale3(Bitmap original)
        {
            try
            {
                //create a blank bitmap the same size as original
                Bitmap newBitmap = new Bitmap(original.Width, original.Height);

                //get a graphics object from the new image
                Graphics g = Graphics.FromImage(newBitmap);

                //create the grayscale ColorMatrix
                System.Drawing.Imaging.ColorMatrix colorMatrix = new System.Drawing.Imaging.ColorMatrix(
                   new float[][] 
                  {
                     new float[] {.3f, .3f, .3f, 0, 0},
                     new float[] {.59f, .59f, .59f, 0, 0},
                     new float[] {.11f, .11f, .11f, 0, 0},
                     new float[] {0, 0, 0, 1, 0},
                     new float[] {0, 0, 0, 0, 1}
                  });

                //create some image attributes
                ImageAttributes attributes = new ImageAttributes();

                //set the color matrix attribute
                attributes.SetColorMatrix(colorMatrix);

                //draw the original image on the new image
                //using the grayscale color matrix
                g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
                   0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

                //dispose the Graphics object
                g.Dispose();
                return newBitmap;

            }
            catch (Exception)
            {
                return null;
            }
        }

        private void barcodeRead(string imgPath)
        {
            //OpenFileDialog fileDialog = new OpenFileDialog();
            //DialogResult dr = fileDialog.ShowDialog();
            //mImagePath = System.IO.Directory.GetCurrentDirectory() + "\\barcodeExmple\\";
            String[] files = Directory.GetFiles(imgPath);
            lstImgPath.Clear();
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].EndsWith(".png", true, null)
                    || files[i].EndsWith(".bmp", true, null)
                    || files[i].EndsWith(".jpg", true, null)
                    || files[i].EndsWith(".gif", true, null))
                {
                    lstImgPath.Add(files[i]);
                }
            }
            lstBillNo.Clear();
            string tmpBarcode = string.Empty;
            foreach (string imgFile in lstImgPath)
            {
                ScanImg scImg = new ScanImg();

                tmpBarcode = ScanBarCode(imgFile);

                if (tmpBarcode.Length > 0)
                {
                    lstBillNo.Add(tmpBarcode);
                }
                else
                {

                }
            }
            //treeView1.Nodes.Clear();
            string liuNo = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            TreeNode tn = new TreeNode();
            tn.Name = liuNo;
            tn.Text = liuNo;
            treeView1.Nodes.Add(tn);
            foreach (string billNo in lstBillNo)
            {
                TreeNode billtn = new TreeNode();
                billtn.Name = billNo;
                billtn.Text = billNo;
                TreeNode liuTn = FindTreeNodByValue(liuNo);
                liuTn.Nodes.Add(billtn);
            }
            treeView1.ExpandAll();

        }

        private static System.Drawing.Image resizeImage(System.Drawing.Image imgToResize, Size size)
        {
            //获取图片宽度
            int sourceWidth = imgToResize.Width;
            //获取图片高度
            int sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;
            //计算宽度的缩放比例
            nPercentW = ((float)size.Width / (float)sourceWidth);
            //计算高度的缩放比例
            nPercentH = ((float)size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;
            //期望的宽度
            int destWidth = (int)(sourceWidth * nPercent);
            //期望的高度
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((System.Drawing.Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //绘制图像
            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();
            return (System.Drawing.Image)b;
        }

        ////展示图片
        //private void ShowList()
        //{
        //    this.listView1.BeginUpdate();
        //    ListViewGroup group = null;

        //    for (int i = 0; i < lstScanImg.Count; i++)
        //    {
        //        var img = lstScanImg[i];
        //        using (Image pic = Image.FromFile(img.storePath))
        //        {
        //            try
        //            {
        //                if (listView1.Groups[img.billNo] != null) //证明已经有存在相同编码附件
        //                {
        //                    group = this.listView1.Groups[img.billNo];
        //                }
        //                else
        //                {
        //                    //if (string.IsNullOrEmpty(img.billNo))
        //                    //{
        //                    //    //如果无单号 创建无条码 并放置当前扫描最前方
        //                    //    if (i == 0)
        //                    //    {
        //                    //        group = new ListViewGroup();  //证明不存在相同编码附件，创建BilNO分组
        //                    //        group.Header = "无条码";
        //                    //        group.Name = img.billNo;
        //                    //        group.Tag = liuNo;
        //                    //        //把分组添加到listview中
        //                    //        listView1.Groups.Add(group);
        //                    //    }
        //                    //}
        //                    //else
        //                    //{
        //                    if (img.isBarcode.Equals("1"))
        //                    {
        //                        group = new ListViewGroup();  //证明不存在相同编码附件，创建BilNO分组
        //                        group.Header = img.billNo;
        //                        group.Name = img.billNo;
        //                        group.Tag = liuNo;
        //                        //把分组添加到listview中
        //                        listView1.Groups.Add(group);
        //                    }
        //                    //}
        //                }

        //                ListViewItem lvi = new ListViewItem();
        //                string fileName = img.storePath.Split('\\').Last();
        //                lvi.ImageKey = fileName;
        //                lvi.Text = "文档" + img.orderNum;
        //                lvi.Name = img.billNo + "_" + img.orderNum;
        //                lvi.Tag = img.storePath;

        //                group.Items.Add(lvi);   //分组添加子项
        //                listView1.Items.Add(lvi);

        //                if (!imageList.Images.Keys.Contains(fileName))
        //                    imageList.Images.Add(fileName, pic);
        //                //if (!imageList.Images.Keys.Contains(fileName))
        //                //{
        //                //    FileStream fileStream = new FileStream(img.storePath, FileMode.Open, FileAccess.Read);
        //                //    int byteLength = (int)fileStream.Length;
        //                //    byte[] fileBytes = new byte[byteLength];
        //                //    fileStream.Read(fileBytes, 0, byteLength);
        //                //    //文件流关闭,文件解除锁定
        //                //    fileStream.Close();
        //                //    fileStream.Dispose();
        //                //    imageList.Images.Add(fileName, Image.FromStream(new MemoryStream(fileBytes)));
        //                //}

        //            }
        //            catch (Exception)
        //            {
        //                throw;


        //            }
        //            finally
        //            {
        //                pic.Dispose();
        //            }
        //        }
        //    }

        //    this.listView1.EndUpdate();
        //}

        //展示图片

        /// <summary>
        /// 展示图片
        /// </summary>
        /// <param name="img"></param>
        private void ShowList(ScanImg img)
        {

            this.listView1.BeginUpdate();
            ListViewGroup group = null;

            //for (int i = 0; i < lstScanImg.Count; i++)
            //{
            //    var img = lstScanImg[i];
            using (Image pic = Image.FromFile(img.storePath))
            {
                try
                {
                    if (listView1.Groups[img.billNo] != null) //证明已经有存在相同编码附件
                    {
                        group = this.listView1.Groups[img.billNo];
                    }
                    else
                    {
                        //if (string.IsNullOrEmpty(img.billNo))
                        //{
                        //    //如果无单号 创建无条码 并放置当前扫描最前方
                        //    if (i == 0)
                        //    {
                        //        group = new ListViewGroup();  //证明不存在相同编码附件，创建BilNO分组
                        //        group.Header = "无条码";
                        //        group.Name = img.billNo;
                        //        group.Tag = liuNo;
                        //        //把分组添加到listview中
                        //        listView1.Groups.Add(group);
                        //    }
                        //}
                        //else
                        //{
                        if (img.isBarcode.Equals("1"))
                        {
                            group = new ListViewGroup();  //证明不存在相同编码附件，创建BilNO分组
                            group.Header = img.billNo;
                            group.Name = img.billNo;
                            group.Tag = liuNo;
                            //把分组添加到listview中
                            listView1.Groups.Add(group);
                        }
                        //}
                    }

                    ListViewItem lvi = new ListViewItem();
                    string fileName = img.storePath.Split('\\').Last();
                    lvi.ImageKey = fileName;
                    lvi.Text = "文档" + img.orderNum;
                    lvi.Name = img.billNo + "_" + img.orderNum;
                    lvi.Tag = img.storePath;

                    group.Items.Add(lvi);   //分组添加子项
                    listView1.Items.Add(lvi);

                    if (!imageList.Images.Keys.Contains(fileName))
                        imageList.Images.Add(fileName, pic);
                    //if (!imageList.Images.Keys.Contains(fileName))
                    //{
                    //    FileStream fileStream = new FileStream(img.storePath, FileMode.Open, FileAccess.Read);
                    //    int byteLength = (int)fileStream.Length;
                    //    byte[] fileBytes = new byte[byteLength];
                    //    fileStream.Read(fileBytes, 0, byteLength);
                    //    //文件流关闭,文件解除锁定
                    //    fileStream.Close();
                    //    fileStream.Dispose();
                    //    imageList.Images.Add(fileName, Image.FromStream(new MemoryStream(fileBytes)));
                    //}

                }
                catch (Exception)
                {
                    throw;


                }
                finally
                {
                    pic.Dispose();
                }
                //}
            }

            this.listView1.EndUpdate();
        }

        public ScanImg loadScanImgInfo(string imgFile, string barcode, string oldbarcode, bool isBarcode, int orderNum, int width, int height)
        {
            ScanImg sImg = new ScanImg();
            if (isBarcode)
            {
                sImg.billNo = barcode;
            }
            else
            {
                sImg.billNo = oldbarcode;
            }
            sImg.isBarcode = isBarcode ? "1" : "0";

            sImg.storePath = imgFile;
            sImg.serialNum = liuNo;
            sImg.orderNum = orderNum;
            sImg.width = width;
            sImg.height = height;

            //sImg.billNo = string.IsNullOrEmpty(sImg.billNo) ? "无条码" + scanCount : sImg.billNo;
            return sImg;
        }

        /// <summary>
        /// 显示遮罩层
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="alpha">透明度</param>
        /// <param name="isShowOne">是否只显示一个层</param>
        public void ShowOpaqueLayer(List<Control> controls, int alpha, bool isShowOne)
        {
            try
            {

                foreach (Control item in panel3.Controls)
                {
                    if (item.GetType() == typeof(PictureBox))
                        item.Controls.Clear();
                    if (item.GetType() == typeof(Label))
                        item.BackColor = Color.Transparent;
                }

                foreach (Control item in controls)
                {
                    this.m_OpaqueLayer = new MyOpaqueLayer(alpha, false);
                    this.m_OpaqueLayer.Dock = DockStyle.Fill;
                    this.m_OpaqueLayer.BringToFront();

                    this.m_OpaqueLayer.Enabled = true;
                    this.m_OpaqueLayer.Visible = true;
                    item.Controls.Add(this.m_OpaqueLayer);

                    this.panel3.Controls.Find("lab_" + item.Name, false).First().BackColor = Color.FromArgb(30, 0, 122, 204);
                }



                //}
            }
            catch { }
        }

        /// <summary>
        /// 隐藏遮罩层
        /// </summary>
        public void HideOpaqueLayer()
        {
            try
            {
                if (this.m_OpaqueLayer != null)
                {
                    this.m_OpaqueLayer.Visible = false;
                    this.m_OpaqueLayer.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// 设置TreeView选中节点
        /// </summary>
        /// <param name="treeView"></param>
        /// <param name="selectStr">选中节点文本</param>
        private void SelectTreeView(TreeView treeView, string liuNo, string billNo)
        {
            treeView.Focus();
            for (int i = 0; i < treeView.Nodes.Count; i++)
            {
                if (treeView.Nodes[i].Name == liuNo)
                {
                    for (int j = 0; j < treeView.Nodes[i].Nodes.Count; j++)
                    {
                        if (treeView.Nodes[i].Nodes[j].Name == billNo)
                        {
                            treeView1.SelectedNode = treeView.Nodes[i].Nodes[j];//选中
                            //treeView.Nodes[i].Nodes[j].Checked = true;
                            treeView.Nodes[i].Expand();//展开父级
                            return;
                        }
                    }
                }
            }
        }

        private void uploadImg()
        {
            //将文件压缩
            string compressPath = System.IO.Directory.GetCurrentDirectory() + "\\COMPRESS\\";
            if (Directory.Exists(compressPath) == false)
            {
                Directory.CreateDirectory(compressPath);
            }

            UserInfo curUser = (UserInfo)AffectCacheObject.Instance[Constants.USERKEY];
            //liuNo = curUser.userName + DateTime.Now.ToString("yyyyMMddHHmmssfff");
            if (string.IsNullOrEmpty(liuNo))
            {
                MessageBox.Show("没有流水号不能提交！");
            }
            else
            {
                string zipName = zipPath + liuNo + ".zip";
                List<string> lstFilePath = new List<string>();
                string cpmFile = string.Empty;
                string tmpScanFile = string.Empty;
                foreach (ScanImg cashScanImg in lstCashImg)
                {
                    //lstFilePath.Add(cashScanImg.storePath);
                    tmpScanFile = cashScanImg.storePath;
                    cpmFile = compressPath + tmpScanFile.Substring(tmpScanFile.LastIndexOf("\\"));
                    if (!compressImage(cashScanImg.storePath, cpmFile, 100, 1000, true))
                    {
                        MessageBox.Show("压缩文件失败!");
                        return;
                    }
                    else
                    {
                        lstFilePath.Add(cpmFile);
                    }
                }
                bool isZip = affSrv.ZipFile(lstFilePath, zipName);
                if (isZip)
                {
                    //MessageBox.Show("压缩成功！");
                    //提交后台
                    UploadObj upObj = new UploadObj();
                    upObj.batchId = liuNo;
                    upObj.items = lstCashImg;
                    bool isUpload = false;
                    var task = Task.Run(() =>
                    {
                        isUpload = affSrv.uploadImg(zipName, upObj);
                    });
                    var form = new frmWaitingBox(task); //等待界面
                    DialogResult rs = form.ShowDialog();
                    if (rs == DialogResult.OK)
                    {
                        if (isUpload)
                        {

                            //MessageBox.Show("上传成功！");
                            //清空流水号
                            liuNo = string.Empty;
                            //清空缓存图片对象
                            lstCashImg.Clear();

                            //清空流水号
                            liuNo = string.Empty;

                            this.imageList.Images.Clear();
                            listView1.LargeImageList.Images.Clear();

                            //清空界面元素
                            this.treeView1.Nodes.Clear();

                            for (int i = this.listView1.Items.Count - 1; i > -1; i--)
                            {
                                this.listView1.Items.RemoveAt(i);
                            }

                            for (int i = this.listView1.Groups.Count - 1; i > -1; i--)
                            {
                                this.listView1.Groups.RemoveAt(i);
                            }

                            //删除压缩文件夹下所有图片
                            DirectoryInfo cpdir = new DirectoryInfo(compressPath);
                            FileInfo[] cpinf = cpdir.GetFiles();
                            foreach (FileInfo cpfinf in cpinf)
                            {
                                cpfinf.Delete();
                            }

                            //删除Image文件夹下所有图片
                            DirectoryInfo dir = new DirectoryInfo(liuImagePath);
                            FileInfo[] inf = dir.GetFiles();
                            foreach (FileInfo finf in inf)
                            {
                                finf.Delete();
                            }

                            scanCount = 0;
                            totalImageIndex = 0;
                            lstBillNo.Clear();

                        }
                        else
                        {
                            //删除压缩文件夹下所有图片
                            DirectoryInfo cpdir = new DirectoryInfo(compressPath);
                            FileInfo[] cpinf = cpdir.GetFiles();
                            foreach (FileInfo cpfinf in cpinf)
                            {
                                cpfinf.Delete();
                            }
                            MessageBox.Show("上传失败！");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("压缩失败！");
                }
            }
        }

        /// <summary>
        /// 后台线程提交图片方法
        /// </summary>
        private void uploadImg2()
        {
            List<string> names = new List<string>();
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                if (!names.Contains(listView1.Items[i].ImageKey))
                {
                    names.Add(listView1.Items[i].ImageKey);
                }
                else
                {
                    MessageBox.Show(listView1.Items[i].ImageKey + " " + listView1.Items[i].Name + " " + listView1.Items[i].Text);
                }
            }

            for (int i = 0; i < listView1.Items.Count; i++)
            {
                listView1.Items[i].ImageKey = Guid.NewGuid().ToString(); //避免重复
            }

            List<string> names2 = new List<string>();
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                if (!names2.Contains(listView1.Items[i].ImageKey))
                {
                    names2.Add(listView1.Items[i].ImageKey);
                }
                else
                {
                    MessageBox.Show(listView1.Items[i].ImageKey + " " + listView1.Items[i].Name + " " + listView1.Items[i].Text);
                }
            }

            imageList.Images.Clear();
            listView1.LargeImageList.Images.Clear();

            //清空界面元素
            this.treeView1.Nodes.Clear();

            for (int i = this.listView1.Items.Count - 1; i > -1; i--)
            {
                this.listView1.Items.RemoveAt(i);
            }

            for (int i = this.listView1.Groups.Count - 1; i > -1; i--)
            {
                this.listView1.Groups.RemoveAt(i);
            }

            UploadObj upObj = new UploadObj();
            upObj.batchId = liuNo;
            upObj.items = lstCashImg;
            string infoJson = affSrv.JosnToStr(upObj);
            File.WriteAllText(liuImagePath + "\\" + liuNo + ".txt", infoJson, Encoding.UTF8);

            //清空缓存图片对象
            lstCashImg.Clear();
            scanCount = 0;
            totalImageIndex = 0;
            lstBillNo.Clear();
            try
            {
                //将文件移到待处理区域
                mWaitProcessPath = mRunPath + "WAITPROCESS\\";

                if (Directory.Exists(mWaitProcessPath) == false)
                {
                    Directory.CreateDirectory(mWaitProcessPath);
                }
                string liuNoProcess = mWaitProcessPath + liuNo;

                DirectorySecurity liuImgSec = Directory.GetAccessControl(liuImagePath);
                liuImgSec.AddAccessRule(new FileSystemAccessRule(@"EveryOne", FileSystemRights.FullControl, AccessControlType.Allow));
                Directory.Move(liuImagePath, liuNoProcess);

                //清空流水号
                liuNo = string.Empty;

                //获取正在提交的流水号。
                curRuningLiuNo = liuNoProcess.Substring(liuNoProcess.LastIndexOf("\\") + 1);

                runingProcCount += 1;

                this.label4.Text = "版权所有 湖南步步高翔龙软件有限公司 FSSC出品  " + "正在进行图片提交的流水号是" + curRuningLiuNo + ".";
                Task t = Task.Run(() =>
                {

                    Logger.Info("流水号:" + curRuningLiuNo + "开始上传===" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    int result = uploadImgBackWork(liuNoProcess);
                    runingProcCount = runingProcCount - 1;
                    Logger.Info("流水号:" + curRuningLiuNo + "结束上传===" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    Logger.Info("流水号:" + curRuningLiuNo + "结束上传===返回结果是" + result);
                    if (result == 0)
                    {
                        updateLableText("流水号" + curRuningLiuNo + "提交成功.");
                        MessageBox.Show("流水号" + curRuningLiuNo + "提交成功.");
                    }
                    else
                    {
                        errorProcCount += 1;
                        updateLableText("流水号" + curRuningLiuNo + "提交失败,失败的提交任务有" + errorProcCount + "个.");
                        MessageBox.Show("流水号" + curRuningLiuNo + "提交失败,失败的提交任务有" + errorProcCount + "个.");
                    }


                });

                t.ContinueWith(r =>
                {
                    string Exception = Convert.ToString(t.Exception);
                    Logger.Error(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "==后台异常信息：" + Exception);
                    errorProcCount += 1;
                    runingProcCount = runingProcCount - 1;
                    updateLableText("流水号" + curRuningLiuNo + "提交失败,失败的提交任务有" + errorProcCount + "个.");
                }, TaskContinuationOptions.OnlyOnFaulted);

            }
            catch (Exception ex)
            {
                systemStatus = 9;
                Logger.Error("流水号:" + liuNo + "处理失败出现系统异常，路径:" + liuImagePath + "异常信息:" + ex.Message);
                MessageBox.Show("流水号:" + liuNo + "提交异常！请重新登录，点击重新提交按钮重试！");
                //清空流水号
                liuNo = string.Empty;
            }

        }

        private int uploadImgBackWork(string procDir)
        {
            int result = 0;
            //将文件压缩
            string compressPath = System.IO.Directory.GetCurrentDirectory() + "\\COMPRESS\\";
            if (Directory.Exists(compressPath) == false)
            {
                Directory.CreateDirectory(compressPath);
            }

            string curNo = procDir.Substring(procDir.LastIndexOf("\\") + 1);
            string zipName = zipPath + curNo + ".zip";
            string curLiuNo = procDir.Substring(procDir.LastIndexOf("\\") + 1);
            string liucompPath = compressPath + curNo + "\\";
            try
            {

                //图片压缩文件夹增加以单号为名的文件夹
                if (Directory.Exists(liucompPath) == false)
                {
                    Directory.CreateDirectory(liucompPath);
                }
                string cpmFile = string.Empty;
                String[] files = Directory.GetFiles(procDir);
                for (int i = 0; i < files.Length; i++)
                {
                    cpmFile = liucompPath + files[i].Substring(files[i].LastIndexOf("\\"));
                    if (!compressImage(files[i], cpmFile, 100, 1000, true))
                    {
                        result = 1;
                    }

                }

                if (result == 0)
                {
                    bool isZip = affSrv.ZipFile(liucompPath, zipName);
                    if (isZip)
                    {

                        string infoJson = File.ReadAllText(procDir + "\\" + curLiuNo + ".txt", Encoding.UTF8);
                        Logger.Info("curLiuNo==" + curLiuNo + ",infoJson==" + infoJson);
                        bool isUpload = affSrv.uploadImg2(zipName, infoJson);
                        //bool isUpload = uploadsuccess;
                        if (!isUpload)
                        {
                            result = 3;
                            if (!Directory.Exists(mErrorProcessPath + curLiuNo))
                            {
                                Directory.Move(procDir, mErrorProcessPath + curLiuNo);
                            }

                        }
                        else
                        {
                            Directory.Delete(procDir, true);
                        }

                    }
                    else
                    {
                        result = 2;
                        if (!Directory.Exists(mErrorProcessPath + curLiuNo))
                        {
                            Directory.Move(procDir, mErrorProcessPath + curLiuNo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = 4;
                Logger.Error(procDir + "提交异常,异常信息：" + ex.Message);
                if (!Directory.Exists(mErrorProcessPath + curLiuNo))
                {
                    Directory.Move(procDir, mErrorProcessPath + curLiuNo);
                }
            }
            finally
            {
                //删除压缩文件夹下所有图片
                Directory.Delete(liucompPath, true);
            }



            return result;
        }

        private void refreshGroup(string name)
        {
            IList<ScanImg> list = lstCashImg.Where(t => t.billNo.Equals(name)).ToList();
            ListViewGroup group = listView1.Groups[name];
            //再重新绘制group
            for (int i = 0; i < group.Items.Count; i++)
            {
                var lvi = group.Items[i];
                //lvi.ImageKey = preNo + "_" + (i + 1);
                lvi.Text = "文档" + (i + 1);
                lvi.Name = group.Name + "_" + (i + 1);
                list[i].orderNum = i + 1;
            }
        }

        private void updateLableText(string msg)
        {
            Action act = delegate() { this.label4.Text = "版权所有 湖南步步高翔龙软件有限公司 FSSC出品  " + msg; };
            this.Invoke(act);
        }

        private void ItemsFocus(string groupName, string name)
        {
            foreach (ListViewItem item in this.listView1.Items)
            {
                if (item.Name.Equals(name))
                {
                    item.BackColor = Color.FromArgb(140, 189, 247);
                }
                else
                    item.BackColor = Color.White;
            }
            if (treeView1.Nodes.Count > 0)
            {
                this.treeView1.SelectedNode = this.treeView1.Nodes[0].Nodes[groupName];
                this.treeView1.Focus();
            }
        }

        private void GroupsFocus(string name)
        {
            foreach (ListViewItem item in this.listView1.Items)
            {
                if (item.Group.Name.Equals(name))
                {
                    item.BackColor = Color.FromArgb(140, 189, 247);
                }
                else
                    item.BackColor = Color.White;
            }
            if (treeView1.Nodes.Count > 0)
            {
                this.treeView1.SelectedNode = this.treeView1.Nodes[0].Nodes[name];
                this.treeView1.Focus();
            }
        }

        //public delegate void Entrust(string str);
        /// <summary>
        /// 无损压缩图片
        /// </summary>

        /// <param name="sFile">原图片地址</param>
        /// <param name="dFile">压缩后保存图片地址</param>
        /// <param name="flag">压缩质量（数字越小压缩率越高）1-100</param>
        /// <param name="size">压缩后图片的最大大小</param>
        /// <param name="sfsc">是否是第一次调用</param>
        /// <returns></returns>
        public bool compressImage(string sFile, string dFile, int flag = 90, int size = 300, bool sfsc = true)
        {
            //如果是第一次调用，原始图像的大小小于要压缩的大小，则直接复制文件，并且返回true
            FileInfo firstFileInfo = new FileInfo(sFile);
            if (sfsc == true && firstFileInfo.Length < size * 1024)
            {
                firstFileInfo.CopyTo(dFile);
                return true;
            }
            using (Image iSource = Image.FromFile(sFile))
            {
                ImageFormat tFormat = iSource.RawFormat;

                int dHeight = iSource.Height / 2;
                int dWidth = iSource.Width / 2;
                int sW = 0, sH = 0;
                //按比例缩放
                Size tem_size = new Size(iSource.Width, iSource.Height);
                if (tem_size.Width > dHeight || tem_size.Width > dWidth)
                {
                    if ((tem_size.Width * dHeight) > (tem_size.Width * dWidth))
                    {
                        sW = dWidth;
                        sH = (dWidth * tem_size.Height) / tem_size.Width;
                    }
                    else
                    {
                        sH = dHeight;
                        sW = (tem_size.Width * dHeight) / tem_size.Height;
                    }
                }
                else
                {
                    sW = tem_size.Width;
                    sH = tem_size.Height;
                }

                Bitmap ob = new Bitmap(dWidth, dHeight);
                Graphics g = Graphics.FromImage(ob);

                g.Clear(Color.WhiteSmoke);
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                g.DrawImage(iSource, new Rectangle((dWidth - sW) / 2, (dHeight - sH) / 2, sW, sH), 0, 0, iSource.Width, iSource.Height, GraphicsUnit.Pixel);

                g.Dispose();

                //以下代码为保存图片时，设置压缩质量
                EncoderParameters ep = new EncoderParameters();
                long[] qy = new long[1];
                qy[0] = flag;//设置压缩的比例1-100
                EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
                ep.Param[0] = eParam;

                try
                {
                    ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                    ImageCodecInfo jpegICIinfo = null;
                    for (int x = 0; x < arrayICI.Length; x++)
                    {
                        if (arrayICI[x].FormatDescription.Equals("JPEG"))
                        {
                            jpegICIinfo = arrayICI[x];
                            break;
                        }
                    }
                    if (jpegICIinfo != null)
                    {
                        ob.Save(dFile, jpegICIinfo, ep);//dFile是压缩后的新路径
                        FileInfo fi = new FileInfo(dFile);
                        if (fi.Length > 1024 * size)
                        {
                            flag = flag - 10;
                            compressImage(sFile, dFile, flag, size, false);
                        }
                    }
                    else
                    {
                        ob.Save(dFile, tFormat);
                    }
                    return true;
                }
                catch
                {
                    return false;
                }
                finally
                {
                    iSource.Dispose();
                    ob.Dispose();
                }
            }
        }

        private void dailyMaintenanceTask()
        {
            EncryptedNameValueSectionHandler hand = new EncryptedNameValueSectionHandler();
            string daytimestr = string.Empty;
            string tmpdaystr = string.Empty;
            if (string.IsNullOrEmpty(hand.ReadConfig("saveDay")))
            {
                //默认保留7天数据
                daytimestr = DateUtil.AddDate(-7).ToString("yyyy-MM-dd");
            }
            else
            {
                int sayDays = Convert.ToInt32(hand.ReadConfig("saveDay"));
                daytimestr = DateUtil.AddDate(-sayDays).ToString("yyyy-MM-dd");

            }
            Logger.Info("维护日期是：" + daytimestr);
            DateTime mDate = DateUtil.ConvertStringToDate(daytimestr, null);

            //删除zip文件

            string zipfn = string.Empty;
            DateTime tmpzipDate;
            DirectoryInfo zipdir = new DirectoryInfo(zipPath);
            FileInfo[] zipinf = zipdir.GetFiles();
            foreach (FileInfo zfinf in zipinf)
            {
                zipfn = zfinf.Name;
                tmpzipDate = DateUtil.ConvertStringToDate(zipfn.Substring(0, 8), "yyyyMMdd");
                if (DateUtil.DiffDays(tmpzipDate, mDate) > 0)
                {
                    zfinf.Delete();
                }


            }
            //删除日志文件
            string LogPath = mRunPath + "Log\\";
            string fn = string.Empty;
            DateTime tmpDate;
            DirectoryInfo dir = new DirectoryInfo(LogPath);
            FileInfo[] inf = dir.GetFiles();
            foreach (FileInfo finf in inf)
            {
                fn = finf.Name;
                tmpDate = DateUtil.ConvertStringToDate(fn.Substring(0, fn.IndexOf(".")), null);
                if (DateUtil.DiffDays(tmpDate, mDate) > 0)
                {
                    finf.Delete();
                }


            }
        }

        /// <summary>
        /// 系统自检
        /// </summary>
        private void systemSelfTest()
        {
            //判断Image文件夹下是否为空
            string[] mDirs = Directory.GetDirectories(mImagePath);
            if (mDirs.Length > 0)
            {
                string tmpLiuNo = string.Empty;
                List<string> exceptionItem = new List<string>();
                foreach (var imgItem in mDirs)
                {
                    tmpLiuNo = imgItem.Substring(imgItem.LastIndexOf("\\"));
                    //判断该文件夹下是否存在保存JSON的txt文件。如没有，则应删除;如果有则转至错误文件夹
                    if (!File.Exists(imgItem + tmpLiuNo + ".txt"))
                    {
                        exceptionItem.Add(imgItem);
                    }
                    else
                    {
                        Directory.Move(imgItem, mErrorProcessPath + tmpLiuNo);
                        Logger.Info("异常流水号:" + tmpLiuNo + "已经移至错误文件夹!");
                    }
                }
                foreach (var expItme in exceptionItem)
                {
                    Directory.Delete(expItme, true);
                    Logger.Info("异常文件夹:" + expItme + "已经删除!");
                }

            }
        }

        internal string GetText(string GroupName, int idx)
        {
            if (idx > -1 && idx < listView1.Groups[GroupName].Items.Count)
                return listView1.Groups[GroupName].Items[idx].Text;
            else
                return "error";
        }

        internal string GetUrl(string GroupName, int idx)
        {
            if (idx > -1 && idx < listView1.Groups[GroupName].Items.Count)
                return mImagePath + liuNo + "\\" + listView1.Groups[GroupName].Items[idx].ImageKey;
            else
                return "error";
        }
        #endregion

        #region 多线程条码识别
        private void barcodeReadTask(int cpuNum)
        {

            DateTime beforeDT = System.DateTime.Now;
            //由于存在扫描未生成图片 需校验是否真实存在图片 不存在及删除
            Logger.Info("已经扫描了" + lstInitScanImg.Count + "张，进入条码识别前的验证集合");
            foreach (ScanImg img in lstInitScanImg)
            {
                if (File.Exists(img.storePath))
                {
                    validList.Add(img);
                }
            }

            Logger.Info("经过验证共有" + validList.Count + "张，进入条码识别");
            object synObj = new object();
            //将图像对象压入队列
            foreach (ScanImg img in validList)
            {
                lock (synObj)
                {
                    scanQueue.Enqueue(img);
                }
            }
            Logger.Info(validList.Count + "张图片成功压入条码识别队列,条码识别队列共有" + scanQueue.Count + "张图片");

            validList.Clear();

            //开启多线程处理条码识别
            TaskManager barcodeReadManager = new TaskManager();  //任务管理器
            barcodeReadManager.Setup(cpuNum);
            barcodeReadManager.Start(taskBarcodeRead);
            barcodeReadManager.Wait();

            string tmpBarcode = "", curBarcode = "";
            bool isCode = false;
            int num = 1;
            //Image simg;
            //MessageBox.Show("taskListScan=" + taskListScan.Count);
            List<ScanImg> listOrder = taskListScan.OrderBy(m => m.scanNum).ToList();
            bool isSeeNoBarCode = false;
            foreach (ScanImg img in listOrder)
            {
                if (!lstBillNo.Contains(img.billNo))
                {
                    if (!string.IsNullOrEmpty(img.billNo))
                    {
                        lstBillNo.Add(img.billNo);
                        tmpBarcode = img.billNo;
                        isCode = true;
                        num = 1;
                    }
                    else
                    {
                        if (!isSeeNoBarCode)
                        {
                            img.billNo = "无条码" + scanCount;
                            lstBillNo.Add(img.billNo);
                            tmpBarcode = img.billNo;
                            isCode = true;
                            num = 1;
                        }
                    }
                }
                else
                {
                    tmpBarcode = img.billNo;
                    num = lstScanImg.Count(t => t.billNo.Equals(img.billNo)) + lstCashImg.Count(t => t.billNo.Equals(img.billNo)) + 1;
                }

                if (!tmpBarcode.Equals(curBarcode))
                {
                    curBarcode = tmpBarcode;
                }

                isSeeNoBarCode = true;
                //simg = Image.FromFile(img.storePath);
                if (lstScanImg.Count(t => t.storePath.Equals(img.storePath)) == 0
                    //&& lstCashImg.Count(t => t.storePath.Equals(img.storePath)) == 0
                    )
                {
                    lstScanImg.Add(loadTaskScanImgInfo(img.storePath, tmpBarcode, curBarcode, isCode, num, 0, 0));
                    num++;
                }

                isCode = false;
            }
            DateTime afterDT = System.DateTime.Now;
            TimeSpan ts = afterDT.Subtract(beforeDT);
            Logger.Info("{0}张图片条码识别共计耗时{1}ms", listOrder.Count, ts.Milliseconds);

        }

        private void taskBarcodeRead()
        {

            while (true)
            {
                if (scanQueue != null && scanQueue.Count > 0)
                {
                    ScanImg scanImg = scanQueue.Dequeue();
                    if (scanImg != null)
                    {
                        String tmpBarcode = ScanBarCode2(scanImg.storePath);
                        // String tmpBarcode ="";
                        bool isBarcode = false;
                        string billReg = AffectCacheObject.Instance[Constants.BILL_REGULAR].ToString();
                        if (tmpBarcode.Length > 0)
                        {
                            //调用正则表达式验证单号
                            isBarcode = Regex.IsMatch(tmpBarcode, billReg);
                            Logger.Info("条码:" + tmpBarcode + "=,正则验证规则" + billReg + "=,验证结果:" + isBarcode);
                            //isBarcode = true;

                            if (isBarcode)
                            {
                                scanImg.billNo = tmpBarcode;
                            }
                        }

                        taskListScan.Add(scanImg);
                    }
                }
                else
                {
                    if (scanQueue.Count <= 0)
                    {
                        break;
                    }
                }
            }

        }

        private ScanImg loadTaskScanImgInfo(string imgFile, string barcode, string oldbarcode, bool isBarcode, int orderNum, int width, int height)
        {
            ScanImg sImg = new ScanImg();
            if (isBarcode)
            {
                sImg.billNo = barcode;
            }
            else
            {
                sImg.billNo = oldbarcode;
            }
            sImg.isBarcode = isBarcode ? "1" : "0";

            sImg.storePath = imgFile;
            sImg.serialNum = liuNo;
            sImg.orderNum = orderNum;
            sImg.width = width;
            sImg.height = height;
            return sImg;
        }


        #endregion

    }

    class orderNumComparer : IComparer
    {
        private int col;
        public orderNumComparer()
        {
            col = 0;
        }
        public orderNumComparer(int column)
        {
            col = column;
        }
        public int Compare(object x, object y)
        {
            if (((ScanImg)x).billNo == ((ScanImg)y).billNo)
                return (((ScanImg)x).orderNum - ((ScanImg)y).orderNum);
            else
                return 0;
        }
    }
}
