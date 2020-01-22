using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Library.Model;
using Common.Configuration;

namespace Library.Service
{
    public class LoginService
    {
        EncryptedNameValueSectionHandler hand = new EncryptedNameValueSectionHandler();
        public string login(string userName, string userPwd)
        {
            //cashTest();
            //return "1";

            string postData = string.Empty;
            postData = "username=" + userName + "&password=" + userPwd;
            string url = hand.ReadConfig("url");
            url = url + "/client/login";
            try
            {
                string result = HttpUtils.HttpPost(url, postData);
                JObject obj = (JObject)JsonConvert.DeserializeObject(result);
                if (obj["code"].ToString().Equals("0"))
                {
                    string token = obj["token"].ToString();
                    string billNoRegular = obj["data"].Value<string>("codebar_regular");
                    //将相关信息放入缓存中
                    //用户信息放入缓存
                    UserInfo user = new UserInfo();
                    user.userName = userName;
                    user.userPwd = userPwd;
                    user.token = token;
                    AffectCacheObject.Instance[Constants.USERKEY] = user;
                    //token信息放入缓存
                    AffectCacheObject.Instance[Constants.TOKEN] = token;
                    //单号识别正则表达式放入缓存
                    AffectCacheObject.Instance[Constants.BILL_REGULAR] = billNoRegular;
                    return "1";
                }
                else
                {
                    return "用户名或密码错误!";
                }
            }
            catch (Exception ex)
            {
                return "登陆异常!" + ex.Message;
            }


        }

        public void cashTest()
        {
            UserInfo user = new UserInfo();
            user.userName = "admin";
            user.userPwd = "123456";
            AffectCacheObject.Instance[Constants.USERKEY] = user;

        }
    }
}
