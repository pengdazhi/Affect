using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Configuration
{
    public class SysConfigInfo : IConfigInfo
    {
        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        private string _userPwd;
        public string UserPwd
        {
            get { return _userPwd; }
            set { _userPwd = value; }
        }
        private string _token;
        public string Token
        {
            get { return _userName; }
            set { _userName = value; }

        }

        private string _saveId { get; set; }
        public string SaveId
        {
            get { return _saveId; }
            set { _saveId = value; }
        }

        private string _autoLogin { get; set; }
        public string AutoLogin
        {
            get { return _autoLogin; }
            set { _autoLogin = value; }
        }
    }
}
