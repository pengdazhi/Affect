using Common.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Twain;
using Common;

namespace Affect
{
    public partial class FormConfig : Form
    {

        EncryptedNameValueSectionHandler hand = new EncryptedNameValueSectionHandler();
        private Twain32 mTwain = new Twain32();
        private int index = -1;
        public FormConfig()
        {
            InitializeComponent();
        }
        //取消event
        private void button2_Click(object sender, EventArgs e)
        {

            this.Close();
        }
        //保存event
        private void button1_Click(object sender, EventArgs e)
        {
            string url = this.urlTxt.Text.Trim();
            string dpi = string.Empty;
            if (this.dpiCmb.SelectedItem != null)
            {
                dpi = this.dpiCmb.SelectedItem.ToString();
            }
                        
            string dpiIndex = this.dpiCmb.SelectedIndex.ToString();
            string twIndex = this.scanCmb.SelectedIndex.ToString();
            string twName = this.scanCmb.SelectedValue.ToString();

            if (string.IsNullOrEmpty(url))
            {
                MessageBox.Show("管理网站URL为空,保存失败");
                return;
            }
            if (string.IsNullOrEmpty(twName))
            {
                MessageBox.Show("扫描仪设置为空,保存失败");
                return;
            }
            if (string.IsNullOrEmpty(dpi))
            {
                MessageBox.Show("扫描仪预设DPI值为空,保存失败");
                return;
            }
            hand.WriterConfig("url", url);
            //hand.WriterConfig("isShowConfig", this.Yradio.Checked ? "T" : "F");
            hand.WriterConfig("autoLogin", this.IsAutoCbx.Checked ? "T" : "F");
            hand.WriterConfig("dpi", dpi);
            hand.WriterConfig("dpiIndex", dpiIndex);
            hand.WriterConfig("twIndex", twIndex);
            hand.WriterConfig("twName", twName);

            if (!string.IsNullOrEmpty(saveDayTxt.Text.Trim()))
            {
                hand.WriterConfig("saveDay", saveDayTxt.Text.Trim());
            }
            MessageBox.Show("保存成功");

            this.Close();
        }

        private void FormConfig_Load(object sender, EventArgs e)
        {
            ShowUI();
        }


        //加载配置
        private void ShowUI()
        {

            try
            {
                mTwain.Language = TwLanguage.CHINESE_SINGAPORE;
                mTwain.IsTwain2Enable = false;
                mTwain.OpenDSM();
                List<string> srclst = new List<string>();

                for (int i = 0; i < mTwain.SourcesCount; i++)
                {
                    srclst.Add(mTwain.GetSourceProductName(i));
                }
                scanCmb.DataSource = srclst;
                mTwain.CloseDSM();
                //读取扫描仪配置
                if (!string.IsNullOrEmpty(hand.ReadConfig("twIndex"))) {
                    this.scanCmb.SelectedIndex = Convert.ToInt32(hand.ReadConfig("twIndex"));               
                }



                this.urlTxt.Text = hand.ReadConfig("url");
                this.IsAutoCbx.Checked = hand.ReadConfig("autoLogin") == "T" ? true : false;

                this.saveDayTxt.Text = hand.ReadConfig("saveDay");
                //this.saveUrlTxt.Text = hand.ReadConfig("saveUrl");
                //if (hand.ReadConfig("isShowConfig") == "T")
                //    this.Yradio.Checked = true;
                //else
                //    this.Nradio.Checked = true;

                //this.skinList.DataSource = new DirectoryInfo("Skins").GetFiles();
                //this.skinList.DisplayMember = "Name";

            }
            catch (Exception)
            {
                
                Logger.Error("未找到支持TWAIN扫描协议的设备,启动失败!");
                MessageBox.Show("未找到支持TWAIN扫描协议的设备,启动失败!");
                Application.Exit();
            }
        }

        private void scanCmb_SelectedIndexChanged(object sender, EventArgs e)
        {

            //读取扫描仪配置
            if (!string.IsNullOrEmpty(hand.ReadConfig("twName")))
            {
                string twname=hand.ReadConfig("twName");
                int j = -1;
                for (int i = 0; i < scanCmb.Items.Count; i++)
			    {
			        if (scanCmb.Items[i].ToString().Equals(twname))
	                {
                        j = i;
                        break;
	                }
			    }
                if (j<0)
                {
                    MessageBox.Show("在本机没有找到对应的扫描仪信息，请重新设置扫描仪!");
                }
                else
                {
                    try
                    {
                        mTwain.SourceIndex = j;
                        mTwain.OpenDataSource();
                        var _resolutions = mTwain.Capabilities.XResolution.Get();
                        List<string> dpilst = new List<string>();
                        for (var i = 0; i < _resolutions.Count; i++)
                        {
                            dpilst.Add(_resolutions[i].ToString());
                        }
                        dpiCmb.DataSource = dpilst;
                        mTwain.CloseDataSource();

                        //读取扫描仪DPI配置
                        if (!string.IsNullOrEmpty(hand.ReadConfig("dpi")))
                        {
                            string dpi = hand.ReadConfig("dpi");
                            int dj = -1;
                            for (int di = 0; di < dpiCmb.Items.Count; di++)
                            {
                                if (dpiCmb.Items[di].ToString().Equals(dpi))
                                {
                                    dj = di;
                                    break;
                                }
                            }
                            if (dj<0)
                            {
                                MessageBox.Show("在本机没有找到扫描仪预设的DPI值，请重新设置DPI值!");
                            }
                            else
                            {
                                this.dpiCmb.SelectedIndex = dj;
                            }

                        }
                    }
                    catch (Exception)
                    {

                        Logger.Error("读取扫描仪信息失败!");
                        MessageBox.Show("读取扫描仪信息失败!,请重新设置扫描仪!");
                    }
                }
            }
            
        }

        private void FormConfig_FormClosed(object sender, FormClosedEventArgs e)
        {
            //关闭DSM
            mTwain.CloseDSM();
        }
         
    }
}
