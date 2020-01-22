using System;
using System.Configuration;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Xml;

namespace Common.Configuration
{
    public class EncryptedNameValueSectionHandler : IConfigurationSectionHandler
    {
        public object Create(object parent, object context, XmlNode section)
        {
            Encrypt encrypt = new Encrypt();
            NameValueCollection nv = new NameValueCollection();
            XmlNodeList list = section.SelectNodes("add");
            foreach (XmlNode node in list)
            {
                string key = node.Attributes["key"].Value;
                string value = node.Attributes["value"].Value;
                if (node.Attributes["isEncrypted"] != null)
                {
                    if ("true".Equals(node.Attributes["isEncrypted"].Value))
                    {
                        nv[key] = encrypt.DecryptString(value);
                    }
                    else
                    {
                        nv[key] = value;
                        WriterConfig(key, encrypt.EncryptString(value));
                    }
                }
                else
                {
                    nv[key] = value;
                }
            }
            return nv;
        }

        public void WriterConfig(string name, string value)
        {

            try
            {
                System.Configuration.Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                //写入<add>元素的Value
                config.AppSettings.Settings[name].Value = value;
                config.Save(ConfigurationSaveMode.Modified);
                //刷新，否则程序读取的还是之前的值（可能已装入内存）
                // System.Configuration.ConfigurationManager.RefreshSection("appSettings");
            }
            catch
            {
            }
        }

        public string ReadConfig(string name)
        {
            //XmlDocument doc = new XmlDocument();
            //System.Configuration.Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //return config.AppSettings.Settings[name].Value;

            //XmlNode root = doc.SelectSingleNode("configuration/userInfo");
            //if (root.HasChildNodes)
            //{
            //    foreach (XmlNode node in root.ChildNodes)
            //    {
            //        if (node.Attributes["key"].Value == name && !string.IsNullOrEmpty(node.Attributes["value"].Value))
            //        {
            //            return new Encrypt().DecryptString(node.Attributes["value"].Value);
            //        }
            //    }
            //}
            try
            {

                System.Configuration.Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                return config.AppSettings.Settings[name].Value;
            }
            catch
            {
                throw new Exception("读取配置信息XML文件失败");
            }

        }
    }
}
