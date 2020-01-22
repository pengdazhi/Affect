using Common.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Library.Model;
using Library.Service;

namespace Affect
{
    static class Program
    {
        public static Sunisoft.IrisSkin.SkinEngine s;
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            log4net.Config.XmlConfigurator.Configure();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            s = new Sunisoft.IrisSkin.SkinEngine();
            s.SkinFile = System.AppDomain.CurrentDomain.BaseDirectory + "\\Skins\\RealOne.ssk";
            //读取配置文件是否自动登录
            EncryptedNameValueSectionHandler hand = new EncryptedNameValueSectionHandler();
            string autoLogin = hand.ReadConfig("autoLogin");
            if (autoLogin == "T")
            {
                string passWord = hand.ReadConfig("userPwd");
                string userName = hand.ReadConfig("userName");
                LoginService login = new LoginService();
                string result = login.login(userName, passWord);
                if (result.Equals("1"))
                {
                    Application.Run(new Main());
                }
                else
                {
                    MessageBox.Show(result);
                    //首先载入登录窗体实例
                    Login frmLogin = new Login();
                    frmLogin.StartPosition = FormStartPosition.CenterScreen;
                    DialogResult loginResult = frmLogin.ShowDialog();
                    //若登录成功则加载主窗体
                    if (loginResult == DialogResult.OK)
                    {
                        Application.Run(new Main());
                    }
                    else
                    {
                        //登录失败则关闭当前程序进程
                        Application.Exit();
                    }

                }
            }
            else
            {
                //首先载入登录窗体实例
                Login frmLogin = new Login();
                frmLogin.StartPosition = FormStartPosition.CenterScreen;
                DialogResult loginResult = frmLogin.ShowDialog();
                //若登录成功则加载主窗体
                if (loginResult == DialogResult.OK)
                {
                    Application.Run(new Main());
                }
                else
                {
                    //登录失败则关闭当前程序进程
                    Application.Exit();
                }
            }

        }
    }
}
