using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Library.Service;
using Common;
using Library.Model;
using Common.Configuration;
using System.Threading;

namespace Affect
{
    public partial class Login : Form
    {
        EncryptedNameValueSectionHandler hand = new EncryptedNameValueSectionHandler();

        public Login()
        {
            InitializeComponent();

            ShowUI();
        }

        //加载首页配置
        private void ShowUI()
        {
            string autoLogin = hand.ReadConfig("autoLogin");
            string saveId = hand.ReadConfig("saveId");
            string userName = hand.ReadConfig("userName");

            this.autoLogin.Checked = autoLogin == "T";
            this.saveId.Checked = saveId == "T";

            if (saveId == "T")
            {
                this.username.Text = userName;
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {

            string username = this.username.Text;
            string password = this.password.Text;
            bool saveId = this.saveId.Checked;
            bool autoLogin = this.autoLogin.Checked;
            string loginResult = "";
            var task = Task.Run(() =>
            {
                LoginService login = new LoginService();
                loginResult = login.login(username, password);
            });
            var form = new frmWaitingBox(task); //等待界面
            form.ShowDialog();

            //login.cashTest();
            if (loginResult.Equals("1"))
            {
                string rs = string.Empty;
                UserInfo user = (UserInfo)AffectCacheObject.Instance[Constants.USERKEY];
                rs = user.userName + "----" + user.userPwd;

                if (autoLogin || saveId)
                {
                    hand.WriterConfig("userName", user.userName);
                    hand.WriterConfig("userPwd", user.userPwd);
                    hand.WriterConfig("autoLogin", autoLogin ? "T" : "F");
                    hand.WriterConfig("saveId", saveId ? "T" : "F");

                    SysConfigInfo config = SysConfigs.Instance().GetConfig();
                    config.UserName = user.userName;
                    config.UserPwd = user.userPwd;
                    config.Token = autoLogin ? "T" : "F";
                    config.SaveId = saveId ? "T" : "F";
                }

                // MessageBox.Show(rs);
                this.DialogResult = DialogResult.OK;

            }
            else
            {
                MessageBox.Show(loginResult);
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormConfig formConfig = new FormConfig();
            formConfig.StartPosition = FormStartPosition.CenterScreen;
            formConfig.ShowDialog();
        }
    }
}
