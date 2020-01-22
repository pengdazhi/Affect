using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Net;
using System.Configuration;
using System.Web;
using System.Security.Cryptography;


namespace Common
{
    /// <summary>
    /// 
    /// </summary>
    public class Utils
    {
        protected static readonly object obj = new object();

        #region 系统版本
        /// <summary>
        /// 版本信息类
        /// </summary>
        public class VersionInfo
        {
            public int FileMajorPart
            {
                get { return 4; }
            }
            public int FileMinorPart
            {
                get { return 0; }
            }
            public int FileBuildPart
            {
                get { return 0; }
            }
            public string ProductName
            {
                get { return "HH"; }
            }
            public int ProductType
            {
                get { return 2; }
            }
        }
        public static string GetVersion()
        {
            return "1.0.0";
        }
        #endregion

        #region MD5加密
        public static string MD5(string pwd)
        {
            //32位小写MD5加密
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(pwd);
            byte[] md5data = md5.ComputeHash(data);
            string str = "";
            for (int i = 0; i < md5data.Length; i++)
            {
                str += md5data[i].ToString("x").PadLeft(2, '0');

            }
            return str;
        }
        #endregion

        #region 对象转换处理
        /// <summary>
        /// 判断对象是否为Int32类型的数字
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public static bool IsNumeric(object expression)
        {
            if (expression != null)
                return IsNumeric(expression.ToString());

            return false;

        }

        /// <summary>
        /// 判断对象是否为Int32类型的数字
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static bool IsNumeric(string expression)
        {
            if (expression != null)
            {
                string str = expression;
                if (str.Length > 0 && str.Length <= 11 && Regex.IsMatch(str, @"^[-]?[0-9]*[.]?[0-9]*$"))
                {
                    if ((str.Length < 10) || (str.Length == 10 && str[0] == '1') || (str.Length == 11 && str[0] == '-' && str[1] == '1'))
                        return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 截取字符串长度，超出部分使用后缀suffix代替，比如abcdevfddd取前3位，后面使用...代替
        /// </summary>
        /// <param name="orginStr"></param>
        /// <param name="length"></param>
        /// <param name="suffix"></param>
        /// <returns></returns>
        public static string SubStrAddSuffix(string orginStr, int length, string suffix)
        {
            string ret = orginStr;
            if (orginStr.Length > length)
            {
                ret = orginStr.Substring(0, length) + suffix;
            }
            return ret;
        }
        /// <summary>
        /// 是否为Double类型
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static bool IsDouble(object expression)
        {
            if (expression != null)
                return Regex.IsMatch(expression.ToString(), @"^([0-9])[0-9]*(\.\w*)?$");

            return false;
        }

        /// <summary>
        /// 检测是否符合email格式
        /// </summary>
        /// <param name="strEmail">要判断的email字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsValidEmail(string strEmail)
        {
            return Regex.IsMatch(strEmail, @"^[\w\.]+([-]\w+)*@[A-Za-z0-9-_]+[\.][A-Za-z0-9-_]");
        }
        public static bool IsValidDoEmail(string strEmail)
        {
            return Regex.IsMatch(strEmail, @"^@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        /// <summary>
        /// 检测是否是正确的Url
        /// </summary>
        /// <param name="strUrl">要验证的Url</param>
        /// <returns>判断结果</returns>
        public static bool IsURL(string strUrl)
        {
            return Regex.IsMatch(strUrl, @"^(http|https)\://([a-zA-Z0-9\.\-]+(\:[a-zA-Z0-9\.&%\$\-]+)*@)*((25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])|localhost|([a-zA-Z0-9\-]+\.)*[a-zA-Z0-9\-]+\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{1,10}))(\:[0-9]+)*(/($|[a-zA-Z0-9\.\,\?\'\\\+&%\$#\=~_\-]+))*$");
        }

        /// <summary>
        /// 将字符串转换为数组
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>字符串数组</returns>
        public static string[] GetStrArray(string str)
        {
            return str.Split(new char[',']);
        }

        /// <summary>
        /// 将数组转换为字符串
        /// </summary>
        /// <param name="list">List</param>
        /// <param name="speater">分隔符</param>
        /// <returns>String</returns>
        public static string GetArrayStr(List<string> list, string speater)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                if (i == list.Count - 1)
                {
                    sb.Append(list[i]);
                }
                else
                {
                    sb.Append(list[i]);
                    sb.Append(speater);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// object型转换为bool型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的bool类型结果</returns>
        public static bool StrToBool(object expression, bool defValue)
        {
            if (expression != null)
                return StrToBool(expression, defValue);

            return defValue;
        }

        /// <summary>
        /// string型转换为bool型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的bool类型结果</returns>
        public static bool StrToBool(string expression, bool defValue)
        {
            if (expression != null)
            {
                if (string.Compare(expression, "true", true) == 0)
                    return true;
                else if (string.Compare(expression, "false", true) == 0)
                    return false;
            }
            return defValue;
        }

        /// <summary>
        /// 将对象转换为Int32类型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int ObjToInt(object expression, int defValue)
        {
            if (expression != null)
                return StrToInt(expression.ToString(), defValue);

            return defValue;
        }

        /// <summary>
        /// 将字符串转换为Int32类型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int StrToInt(string expression, int defValue)
        {
            if (string.IsNullOrEmpty(expression) || expression.Trim().Length >= 11 || !Regex.IsMatch(expression.Trim(), @"^([-]|[0-9])[0-9]*(\.\w*)?$"))
                return defValue;

            int rv;
            if (Int32.TryParse(expression, out rv))
                return rv;

            return Convert.ToInt32(StrToFloat(expression, defValue));
        }

        /// <summary>
        /// Object型转换为decimal型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的decimal类型结果</returns>
        public static decimal ObjToDecimal(object expression, decimal defValue)
        {
            if (expression != null)
                return StrToDecimal(expression.ToString(), defValue);

            return defValue;
        }

        /// <summary>
        /// string型转换为decimal型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的decimal类型结果</returns>
        public static decimal StrToDecimal(string expression, decimal defValue)
        {
            if ((expression == null) || (expression.Length > 10))
                return defValue;

            decimal intValue = defValue;
            if (expression != null)
            {
                bool IsDecimal = Regex.IsMatch(expression, @"^([-]|[0-9])[0-9]*(\.\w*)?$");
                if (IsDecimal)
                    decimal.TryParse(expression, out intValue);
            }
            return intValue;
        }

        /// <summary>
        /// Object型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float ObjToFloat(object expression, float defValue)
        {
            if (expression != null)
                return StrToFloat(expression.ToString(), defValue);

            return defValue;
        }

        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float StrToFloat(string expression, float defValue)
        {
            if ((expression == null) || (expression.Length > 10))
                return defValue;

            float intValue = defValue;
            if (expression != null)
            {
                bool IsFloat = Regex.IsMatch(expression, @"^([-]|[0-9])[0-9]*(\.\w*)?$");
                if (IsFloat)
                    float.TryParse(expression, out intValue);
            }
            return intValue;
        }

        /// <summary>
        /// Object型转换为double型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static double ObjToDouble(object expression, double defValue)
        {
            if (expression != null)
                return ObjToDouble(expression.ToString(), defValue);

            return defValue;
        }

        /// <summary>
        /// string型转换为double型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static double ObjToDouble(string expression, double defValue)
        {
            if ((expression == null) || (expression.Length > 10))
                return defValue;

            double intValue = defValue;
            if (expression != null)
            {
                if (IsDouble(expression))
                    double.TryParse(expression, out intValue);
            }
            return intValue;
        }

        /// <summary>
        /// 将对象转换为日期时间类型
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static DateTime StrToDateTime(string str, DateTime defValue)
        {
            if (!string.IsNullOrEmpty(str))
            {
                DateTime dateTime;
                if (DateTime.TryParse(str, out dateTime))
                    return dateTime;
            }
            return defValue;
        }

        /// <summary>
        /// 将对象转换为日期时间类型
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <returns>转换后的int类型结果</returns>
        public static DateTime StrToDateTime(string str)
        {
            return StrToDateTime(str, DateTime.Now);
        }

        /// <summary>
        /// 将对象转换为日期时间类型
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <returns>转换后的int类型结果</returns>
        public static DateTime ObjectToDateTime(object obj)
        {
            return StrToDateTime(obj.ToString());
        }

        /// <summary>
        /// 将对象转换为日期时间类型
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static DateTime ObjectToDateTime(object obj, DateTime defValue)
        {
            return StrToDateTime(obj.ToString(), defValue);
        }

        /// <summary>
        /// 将对象转换为字符串
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <returns>转换后的string类型结果</returns>
        public static string ObjectToStr(object obj)
        {
            if (obj == null)
                return "";
            return obj.ToString().Trim();
        }

        /// <summary>
        /// 将对象转换为Int类型
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static int ObjToInt(object obj)
        {
            if (isNumber(obj))
            {
                return int.Parse(obj.ToString());
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// 判断对象是否可以转成int型
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static bool isNumber(object o)
        {
            int tmpInt;
            if (o == null)
            {
                return false;
            }
            if (o.ToString().Trim().Length == 0)
            {
                return false;
            }
            if (!int.TryParse(o.ToString(), out tmpInt))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// 对象转换为Double类型
        /// </summary>
        /// <param name="obj">要转换的字符串</param>
        /// <returns></returns>
        public static double ToDouble(object obj)
        {
            if (obj == null)
                return 0;
            try
            {
                return double.Parse(obj.ToString());
            }
            catch
            {
                return 0;
            }
        }
        #endregion

        #region 分割字符串
        /// <summary>
        /// 分割字符串
        /// </summary>
        public static string[] SplitString(string strContent, string strSplit)
        {
            if (!string.IsNullOrEmpty(strContent))
            {
                if (strContent.IndexOf(strSplit) < 0)
                    return new string[] { strContent };

                return Regex.Split(strContent, Regex.Escape(strSplit), RegexOptions.IgnoreCase);
            }
            else
                return new string[0] { };
        }

        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <returns></returns>
        public static string[] SplitString(string strContent, string strSplit, int count)
        {
            string[] result = new string[count];
            string[] splited = SplitString(strContent, strSplit);

            for (int i = 0; i < count; i++)
            {
                if (i < splited.Length)
                    result[i] = splited[i];
                else
                    result[i] = string.Empty;
            }

            return result;
        }
        #endregion

        #region 删除最后结尾的一个逗号
        /// <summary>
        /// 删除最后结尾的一个逗号
        /// </summary>
        public static string DelLastComma(string str)
        {
            if (str.Length < 1)
            {
                return "";
            }
            return str.Substring(0, str.LastIndexOf(","));
        }
        #endregion

        #region 删除最后结尾的指定字符后的字符
        /// <summary>
        /// 删除最后结尾的指定字符后的字符
        /// </summary>
        public static string DelLastChar(string str, string strchar)
        {
            if (string.IsNullOrEmpty(str))
                return "";
            if (str.LastIndexOf(strchar) >= 0 && str.LastIndexOf(strchar) == str.Length - 1)
            {
                return str.Substring(0, str.LastIndexOf(strchar));
            }
            return str;
        }
        #endregion

        #region 生成指定长度的字符串
        /// <summary>
        /// 生成指定长度的字符串,即生成strLong个str字符串
        /// </summary>
        /// <param name="strLong">生成的长度</param>
        /// <param name="str">以str生成字符串</param>
        /// <returns></returns>
        public static string StringOfChar(int strLong, string str)
        {
            string ReturnStr = "";
            for (int i = 0; i < strLong; i++)
            {
                ReturnStr += str;
            }

            return ReturnStr;
        }
        #endregion

        #region 生成日期随机码
        /// <summary>
        /// 生成日期随机码
        /// </summary>
        /// <returns></returns>
        public static string GetRamCode()
        {
            #region
            return DateTime.Now.ToString("yyyyMMddHHmmssffff");
            #endregion
        }
        #endregion

        #region 生成随机字母或数字
        /// <summary>
        /// 生成随机数字
        /// </summary>
        /// <param name="length">生成长度</param>
        /// <returns></returns>
        public static string Number(int Length)
        {
            return Number(Length, false);
        }

        /// <summary>
        /// 生成随机数字
        /// </summary>
        /// <param name="Length">生成长度</param>
        /// <param name="Sleep">是否要在生成前将当前线程阻止以避免重复</param>
        /// <returns></returns>
        public static string Number(int Length, bool Sleep)
        {
            if (Sleep)
                System.Threading.Thread.Sleep(3);
            string result = "";
            System.Random random = new Random(unchecked((int)DateTime.Now.Ticks));
            for (int i = 0; i < Length; i++)
            {
                result += random.Next(10).ToString();
            }
            return result;
        }

        /// <summary>
        /// 生成随机字母数字的随机字符串（由指定字符中生成的）
        /// </summary>
        /// <param name="Length">生成长度</param>
        /// <returns></returns>
        public static string GetFixedRandomCode(int Length)
        {
            string chkCode = string.Empty;
            //验证码的字符集，去掉了一些容易混淆的字符  
            char[] character = { '0', '1', '2', '3', '4', 'a', 'b', 'c', 'd', 'e', 'f', '5', '6', '7', '8', '9' };
            Random rnd = new Random(unchecked((int)DateTime.Now.Ticks));
            //生成验证码字符串  
            for (int i = 0; i < Length; i++)
            {
                chkCode += character[rnd.Next(character.Length)];
            }
            return chkCode;
        }
        /// <summary>
        /// 生成随机字母数字的随机字符串
        /// </summary>
        /// <param name="Length">生成长度</param>
        /// <returns></returns>
        public static string GetRandomCode(int Length)
        {
            string chkCode = string.Empty;
            //验证码的字符集，去掉了一些容易混淆的字符  
            char[] character = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
            Random rnd = new Random(unchecked((int)DateTime.Now.Ticks));
            //生成验证码字符串  
            for (int i = 0; i < Length; i++)
            {
                chkCode += character[rnd.Next(character.Length)];
            }
            return chkCode;
        }
        /// <summary>
        /// 生成随机字母字符串(数字字母混和)
        /// </summary>
        /// <param name="codeCount">待生成的位数</param>
        public static string GetCheckCode(int codeCount)
        {
            string str = string.Empty;
            int rep = 0;
            long num2 = DateTime.Now.Ticks + rep;
            rep++;
            Random random = new Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> rep)));
            for (int i = 0; i < codeCount; i++)
            {
                char ch;
                int num = random.Next();
                if ((num % 2) == 0)
                {
                    ch = (char)(0x30 + ((ushort)(num % 10)));
                }
                else
                {
                    ch = (char)(0x41 + ((ushort)(num % 0x1a)));
                }
                str = str + ch.ToString();
            }
            return str;
        }
        /// <summary>
        /// 根据日期和随机码生成订单号
        /// </summary>
        /// <returns></returns>
        public static string GetOrderNumber()
        {
            string num = DateTime.Now.ToString("yyMMddHHmmss");//yyyyMMddHHmmssms
            return num + Number(2, true).ToString();
        }
        private static int Next(int numSeeds, int length)
        {
            byte[] buffer = new byte[length];
            System.Security.Cryptography.RNGCryptoServiceProvider Gen = new System.Security.Cryptography.RNGCryptoServiceProvider();
            Gen.GetBytes(buffer);
            uint randomResult = 0x0;//这里用uint作为生成的随机数  
            for (int i = 0; i < length; i++)
            {
                randomResult |= ((uint)buffer[i] << ((length - 1 - i) * 8));
            }
            return (int)(randomResult % numSeeds);
        }
        #endregion

        #region 检测是否有Sql危险字符
        /// <summary>
        /// 检测是否有Sql危险字符
        /// </summary>
        /// <param name="str">要判断字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsSafeSqlString(string str)
        {
            return !Regex.IsMatch(str, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
        }

        /// <summary>
        /// 检查危险字符
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public static string Filter(string sInput)
        {
            if (sInput == null || sInput == "")
                return null;
            string sInput1 = sInput.ToLower();
            string output = sInput;
            string pattern = @"*|and|exec|insert|select|delete|update|count|master|truncate|declare|char(|mid(|chr(|'";
            if (Regex.Match(sInput1, Regex.Escape(pattern), RegexOptions.Compiled | RegexOptions.IgnoreCase).Success)
            {
                throw new Exception("字符串中含有非法字符!");
            }
            else
            {
                output = output.Replace("'", "''");
            }
            return output;
        }

        /// <summary> 
        /// 检查过滤设定的危险字符
        /// </summary> 
        /// <param name="InText">要过滤的字符串 </param> 
        /// <returns>如果参数存在不安全字符，则返回true </returns> 
        public static bool SqlFilter(string word, string InText)
        {
            if (InText == null)
                return false;
            foreach (string i in word.Split('|'))
            {
                if ((InText.ToLower().IndexOf(i + " ") > -1) || (InText.ToLower().IndexOf(" " + i) > -1))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region 过滤特殊字符
        /// <summary>
        /// 过滤特殊字符
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public static string Htmls(string Input)
        {
            if (Input != string.Empty && Input != null)
            {
                string ihtml = Input.ToLower();
                ihtml = ihtml.Replace("<script", "&lt;script");
                ihtml = ihtml.Replace("script>", "script&gt;");
                ihtml = ihtml.Replace("<%", "&lt;%");
                ihtml = ihtml.Replace("%>", "%&gt;");
                ihtml = ihtml.Replace("<$", "&lt;$");
                ihtml = ihtml.Replace("$>", "$&gt;");
                return ihtml;
            }
            else
            {
                return string.Empty;
            }
        }
        #endregion

        #region 检查是否为IP地址
        /// <summary>
        /// 是否为ip
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIP(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }
        #endregion

        #region 获得当前绝对路径
        /// <summary>
        /// 获得当前绝对路径
        /// </summary>
        /// <param name="strPath">指定的路径</param>
        /// <returns>绝对路径</returns>
        public static string GetMapPath(string strPath)
        {

            strPath = strPath.Replace("/", "\\");
            if (strPath.StartsWith("\\"))
            {
                strPath = strPath.Substring(strPath.IndexOf('\\', 1)).TrimStart('\\');
            }
            return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);

        }
        #endregion

        #region 文件操作
        /// <summary>
        /// 判断文件夹目录是否存在，如果不存在则创建
        /// </summary>
        /// <param name="filepath">文件夹目录</param>
        public static void ExistDirectory(string filepath)
        {
            if (!Directory.Exists(filepath))
                Directory.CreateDirectory(filepath);
        }
        /// <summary>
        /// 判断文件是否存在，如果不存在则创建
        /// </summary>
        /// <param name="filepath">文件</param>
        public static void ExistFile(string filepath, string filename)
        {
            if (!Directory.Exists(filepath))
                Directory.CreateDirectory(filepath);
            string fileurl = filepath + filename;
            if (!System.IO.File.Exists(fileurl))
                System.IO.File.Create(fileurl);
        }


        /// <summary>
        /// 删除单个文件
        /// </summary>
        /// <param name="_filepath">文件相对路径</param>
        public static bool DeleteFile(string _filepath)
        {
            if (string.IsNullOrEmpty(_filepath))
            {
                return false;
            }
            string fullpath = GetMapPath(_filepath);
            if (File.Exists(fullpath))
            {
                File.Delete(fullpath);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 删除上传的文件(及缩略图)
        /// </summary>
        /// <param name="_filepath"></param>
        public static void DeleteUpFile(string _filepath)
        {
            if (string.IsNullOrEmpty(_filepath))
            {
                return;
            }
            string fullpath = GetMapPath(_filepath); //原图
            if (File.Exists(fullpath))
            {
                File.Delete(fullpath);
            }
            if (_filepath.LastIndexOf("/") >= 0)
            {
                string thumbnailpath = _filepath.Substring(0, _filepath.LastIndexOf("/")) + "mall_" + _filepath.Substring(_filepath.LastIndexOf("/") + 1);
                string fullTPATH = GetMapPath(thumbnailpath); //宿略图
                if (File.Exists(fullTPATH))
                {
                    File.Delete(fullTPATH);
                }
            }
        }

        /// <summary>
        /// 删除内容图片
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="startstr">匹配开头字符串</param>
        public static void DeleteContentPic(string content, string startstr)
        {
            if (string.IsNullOrEmpty(content))
            {
                return;
            }
            Regex reg = new Regex("IMG[^>]*?src\\s*=\\s*(?:\"(?<1>[^\"]*)\"|'(?<1>[^\']*)')", RegexOptions.IgnoreCase);
            MatchCollection m = reg.Matches(content);
            foreach (Match math in m)
            {
                string imgUrl = math.Groups[1].Value;
                string fullPath = GetMapPath(imgUrl);
                try
                {
                    if (imgUrl.ToLower().StartsWith(startstr.ToLower()) && File.Exists(fullPath))
                    {
                        File.Delete(fullPath);
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// 删除指定文件夹
        /// </summary>
        /// <param name="_dirpath">文件相对路径</param>
        public static bool DeleteDirectory(string _dirpath)
        {
            if (string.IsNullOrEmpty(_dirpath))
            {
                return false;
            }
            string fullpath = GetMapPath(_dirpath);
            if (Directory.Exists(fullpath))
            {
                Directory.Delete(fullpath, true);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 修改指定文件夹名称
        /// </summary>
        /// <param name="old_dirpath">旧相对路径</param>
        /// <param name="new_dirpath">新相对路径</param>
        /// <returns>bool</returns>
        public static bool MoveDirectory(string old_dirpath, string new_dirpath)
        {
            if (string.IsNullOrEmpty(old_dirpath))
            {
                return false;
            }
            string fulloldpath = GetMapPath(old_dirpath);
            string fullnewpath = GetMapPath(new_dirpath);
            if (Directory.Exists(fulloldpath))
            {
                Directory.Move(fulloldpath, fullnewpath);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 返回文件大小KB
        /// </summary>
        /// <param name="_filepath">文件相对路径</param>
        /// <returns>int</returns>
        public static int GetFileSize(string _filepath)
        {
            if (string.IsNullOrEmpty(_filepath))
            {
                return 0;
            }
            string fullpath = GetMapPath(_filepath);
            if (File.Exists(fullpath))
            {
                FileInfo fileInfo = new FileInfo(fullpath);
                return ((int)fileInfo.Length) / 1024;
            }
            return 0;
        }

        /// <summary>
        /// 计算文件大小函数(保留两位小数),Size为字节大小
        /// </summary>
        /// <param name="size">初始文件大小</param>
        /// <returns></returns>
        public static string CountSize(long size)
        {
            string m_strSize = "";
            long FactSize = 0;
            FactSize = size;
            if (FactSize < 1024.00)
                m_strSize = FactSize.ToString("F2") + " Byte";
            else if (FactSize >= 1024.00 && FactSize < 1048576)
                m_strSize = (FactSize / 1024.00).ToString("F2") + " K";
            else if (FactSize >= 1048576 && FactSize < 1073741824)
                m_strSize = (FactSize / 1024.00 / 1024.00).ToString("F2") + " M";
            else if (FactSize >= 1073741824)
                m_strSize = (FactSize / 1024.00 / 1024.00 / 1024.00).ToString("F2") + " G";
            return m_strSize;
        }
        /// <summary>
        /// 获取一批文件的大小
        /// </summary>
        /// <param name="sFilePath">文件所在的路径</param>
        /// <param name="sMask">文件名称含通配符</param>
        /// <returns></returns>
        public static long GetFilesSize(string sFilePath, string sMask)
        {
            long lSize = 0;
            if (sMask.Trim() == "")
                return lSize;
            DirectoryInfo pDirectoryInfo = new DirectoryInfo(sFilePath);
            if (pDirectoryInfo.Exists == false)
                return lSize;
            FileInfo[] pFileInfos = pDirectoryInfo.GetFiles(sMask, SearchOption.TopDirectoryOnly);
            foreach (FileInfo e in pFileInfos)
            {
                lSize += GetFileSize(e.FullName);
            }
            return lSize;
        }
        /// <summary>
        /// 返回文件扩展名，不含“.”
        /// </summary>
        /// <param name="_filepath">文件全名称</param>
        /// <returns>string</returns>
        public static string GetFileExt(string _filepath)
        {
            if (string.IsNullOrEmpty(_filepath))
            {
                return "";
            }
            if (_filepath.LastIndexOf(".") > 0)
            {
                return _filepath.Substring(_filepath.LastIndexOf(".") + 1); //文件扩展名，不含“.”
            }
            return "";
        }

        /// <summary>
        /// 返回文件名，不含路径
        /// </summary>
        /// <param name="_filepath">文件相对路径</param>
        /// <returns>string</returns>
        public static string GetFileName(string _filepath)
        {
            return _filepath.Substring(_filepath.LastIndexOf(@"/") + 1);
        }

        /// <summary>
        /// 文件是否存在
        /// </summary>
        /// <param name="_filepath">文件相对路径</param>
        /// <returns>bool</returns>
        public static bool FileExists(string _filepath)
        {
            string fullpath = GetMapPath(_filepath);
            if (File.Exists(fullpath))
            {
                return true;
            }
            return false;
        }

        ///// <summary>
        ///// 获得远程字符串
        ///// </summary>
        //public static string GetDomainStr(string key, string uriPath)
        //{
        //    string result = DataCache.Get(key) as string;
        //    if (result == null)
        //    {
        //        System.Net.WebClient client = new System.Net.WebClient();
        //        try
        //        {
        //            client.Encoding = System.Text.Encoding.UTF8;
        //            result = client.DownloadString(uriPath);
        //        }
        //        catch
        //        {
        //            result = "暂时无法连接!";
        //        }
        //        DataCache.Insert(key, result, 60);
        //    }

        //    return result;
        //}


        #endregion

        #region 替换指定的字符串
        /// <summary>
        /// 替换指定的字符串
        /// </summary>
        /// <param name="originalStr">原字符串</param>
        /// <param name="oldStr">旧字符串</param>
        /// <param name="newStr">新字符串</param>
        /// <returns></returns>
        public static string ReplaceStr(string originalStr, string oldStr, string newStr)
        {
            if (string.IsNullOrEmpty(oldStr))
            {
                return "";
            }
            return originalStr.Replace(oldStr, newStr);
        }
        #endregion

        #region 显示分页
        /// <summary>
        /// 返回分页页码
        /// </summary>
        /// <param name="pageSize">页面大小</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="totalCount">总记录数</param>
        /// <param name="linkUrl">链接地址，__id__代表页码</param>
        /// <param name="centSize">中间页码数量</param>
        /// <returns></returns>
        public static string OutPageList(int pageSize, int pageIndex, int totalCount, string linkUrl, int centSize)
        {
            //计算页数
            if (totalCount < 1 || pageSize < 1)
            {
                return "";
            }
            int pageCount = totalCount / pageSize;
            if (pageCount < 1)
            {
                return "";
            }
            if (totalCount % pageSize > 0)
            {
                pageCount += 1;
            }
            if (pageCount <= 1)
            {
                return "";
            }
            StringBuilder pageStr = new StringBuilder();
            string pageId = "__id__";
            string firstBtn = "<a href=\"" + ReplaceStr(linkUrl, pageId, (pageIndex - 1).ToString()) + "\">«上一页</a>";
            string lastBtn = "<a href=\"" + ReplaceStr(linkUrl, pageId, (pageIndex + 1).ToString()) + "\">下一页»</a>";
            string firstStr = "<a href=\"" + ReplaceStr(linkUrl, pageId, "1") + "\">1</a>";
            string lastStr = "<a href=\"" + ReplaceStr(linkUrl, pageId, pageCount.ToString()) + "\">" + pageCount.ToString() + "</a>";

            if (pageIndex <= 1)
            {
                firstBtn = "<span class=\"disabled\">«上一页</span>";
            }
            if (pageIndex >= pageCount)
            {
                lastBtn = "<span class=\"disabled\">下一页»</span>";
            }
            if (pageIndex == 1)
            {
                firstStr = "<span class=\"current\">1</span>";
            }
            if (pageIndex == pageCount)
            {
                lastStr = "<span class=\"current\">" + pageCount.ToString() + "</span>";
            }
            int firstNum = pageIndex - (centSize / 2); //中间开始的页码
            if (pageIndex < centSize)
                firstNum = 2;
            int lastNum = pageIndex + centSize - ((centSize / 2) + 1); //中间结束的页码
            if (lastNum >= pageCount)
                lastNum = pageCount - 1;
            pageStr.Append("<span>共" + totalCount + "记录</span>");
            pageStr.Append(firstBtn + firstStr);
            if (pageIndex >= centSize)
            {
                pageStr.Append("<span>...</span>\n");
            }
            for (int i = firstNum; i <= lastNum; i++)
            {
                if (i == pageIndex)
                {
                    pageStr.Append("<span class=\"current\">" + i + "</span>");
                }
                else
                {
                    pageStr.Append("<a href=\"" + ReplaceStr(linkUrl, pageId, i.ToString()) + "\">" + i + "</a>");
                }
            }
            if (pageCount - pageIndex > centSize - ((centSize / 2)))
            {
                pageStr.Append("<span>...</span>");
            }
            pageStr.Append(lastStr + lastBtn);
            return pageStr.ToString();
        }
        #endregion

        #region 操作权限菜单
        /// <summary>
        /// 获取操作权限
        /// </summary>
        /// <returns>Dictionary</returns>
        public static Dictionary<string, string> ActionType()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            dic.Add("Show", "显示");
            dic.Add("View", "查看");
            dic.Add("Add", "添加");
            dic.Add("Edit", "修改");
            dic.Add("Delete", "删除");
            dic.Add("Audit", "审核");
            dic.Add("Reply", "回复");
            dic.Add("Confirm", "确认");
            dic.Add("Cancel", "取消");
            dic.Add("Invalid", "作废");
            dic.Add("Build", "生成");
            dic.Add("Instal", "安装");
            dic.Add("Unload", "卸载");
            dic.Add("Back", "备份");
            dic.Add("Restore", "还原");
            dic.Add("Replace", "替换");
            dic.Add("Export", "导出");
            dic.Add("Import", "导入");
            dic.Add("Download", "下载");
            dic.Add("Upload", "上传");
            dic.Add("UploadCode", "导码");

            return dic;
        }
        #endregion

        #region 替换URL
        /// <summary>
        /// 替换扩展名
        /// </summary>
        public static string GetUrlExtension(string urlPage, string staticExtension)
        {
            int indexNum = urlPage.LastIndexOf('.');
            if (indexNum > 0)
            {
                return urlPage.Replace(urlPage.Substring(indexNum), "." + staticExtension);
            }
            return urlPage;
        }
        /// <summary>
        /// 替换扩展名，如没有扩展名替换默认首页
        /// </summary>
        public static string GetUrlExtension(string urlPage, string staticExtension, bool defaultVal)
        {
            int indexNum = urlPage.LastIndexOf('.');
            if (indexNum > 0)
            {
                return urlPage.Replace(urlPage.Substring(indexNum), "." + staticExtension);
            }
            if (defaultVal)
            {
                if (urlPage.EndsWith("/"))
                {
                    return urlPage + "index." + staticExtension;
                }
                else
                {
                    return urlPage + "/index." + staticExtension;
                }
            }
            return urlPage;
        }
        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <param name="strHtml"></param>
        /// <returns></returns>
        private static string StripHt(string strHtml)
        {
            Regex regex = new Regex("<.+?>", RegexOptions.IgnoreCase);
            string strOutput = regex.Replace(strHtml, "");
            return strOutput;
        }
        /// <summary>
        /// 除去字符串中的Html元素标签
        /// </summary>
        /// <param name="strhtml">要处理的字符串</param>
        /// <returns>返回除去Html元素标签的字符串</returns>
        private static string Striphtml(string strhtml)
        {
            string stroutput = strhtml;
            Regex regex = new Regex(@"<[^>]+>|</[^>]+>");
            stroutput = regex.Replace(stroutput, "");
            return stroutput;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strhtml"></param>
        /// <returns></returns>
        public static string RemoveHtml(string strhtml)
        {
            string html = Striphtml(StripHt(strhtml));
            html = Regex.Replace(html, "[\f\n\r\t\v]", "");
            return html;
        }

        /// <summary>
        /// 返回标准日期格式string
        /// </summary>
        public static string GetDate()
        {
            return DateTime.Now.ToString("yyyy-MM-dd");
        }

        #region 格式化日期型数据

        /// <summary>
        /// 格式化日期型数据
        /// </summary>
        public static string FormatDate(string date)
        {
            return FormatDate(date, null);
        }
        /// <summary>
        /// 格式化日期型数据
        /// </summary>
        /// <param name="date">日期字符串</param>
        /// <param name="format">返回日期格式</param>
        public static string FormatDate(string date, string format)
        {
            string dtt = date;
            if (dtt != "")
            {
                if (date.IndexOf("1900-1-1") != -1 || date.IndexOf("1900-01-01") != -1 || date.IndexOf("0001/1/1") != -1)
                {
                    dtt = "";
                }
                else
                {
                    DateTime odt;
                    DateTime.TryParse(dtt, out odt);
                    if (format != null)
                    {
                        dtt = odt.ToString(format);
                    }
                    else
                    {
                        dtt = odt.ToString("yyyy-MM-dd");
                        if (dtt.IndexOf("1900-01-01") != -1 || dtt.IndexOf("1900-1-1") != -1 || date.IndexOf("0001/1/1") != -1)
                        {
                            dtt = "";
                        }
                    }
                }
            }
            if (dtt.IndexOf("1900-1-1") != -1 || dtt.IndexOf("1900-01-01") != -1)
            {
                dtt = "";
            }
            return dtt;
        }

        /// <summary>
        /// 字符串格式化日期方法,默认格式：yyyy-MM-dd
        /// </summary>
        /// <param name="str">输入字符串</param>
        /// <returns>返回格式化的日期字符串</returns>
        public static string FormatDate(object str)
        {
            if (str != null && !string.IsNullOrEmpty(str.ToString()))
                return FormatDate(str.ToString().Trim(), "yyyy-MM-dd");
            return "";
        }
        /// <summary>
        /// 字符串格式化日期方法
        /// </summary>
        /// <param name="str">输入字符串</param>
        /// <param name="format">格式化格式</param>
        /// <returns>返回格式化的日期字符串</returns>
        public static string FormatDate(object str, string format)
        {
            if (str != null && !string.IsNullOrEmpty(str.ToString()))
                return FormatDate(str.ToString().Trim(), format);
            return "";
        }

        public static string ConvertDate(DateTime dt, string format)
        {
            string dtt = "";
            switch (format)
            {
                case "yyyy年mm月dd日":
                    {
                        dtt = dt.Year.ToString("0000") + "年" + dt.Month.ToString("00") + "月" + dt.Day.ToString("00") + "日";
                    } break;
                case "yy年mm月dd日":
                    {
                        dtt = dt.Year.ToString("00") + "年" + dt.Month.ToString("00") + "月" + dt.Day.ToString("00") + "日";
                    } break;
                case "yyMMdd":
                    {
                        dtt = dt.Year.ToString("00") + dt.Month.ToString("00") + dt.Day.ToString("00");
                    } break;
                case "yy.mm.dd":
                    {
                        dtt = dt.Year.ToString("00") + "." + dt.Month.ToString("00") + "." + dt.Day.ToString("00");
                    } break;
                case "yyyy.mm.dd":
                    {
                        dtt = dt.Year.ToString("0000") + "." + dt.Month.ToString("00") + "." + dt.Day.ToString("00");
                    } break;

                default:
                    {
                        dtt = dt.Year.ToString("0000") + "年" + dt.Month.ToString("00") + "月" + dt.Day.ToString("00") + "日";
                    } break;
            }
            return dtt;
        }
        /// <summary>
        /// 返回日期型数据
        /// </summary>
        /// <param name="date"></param>
        public static DateTime ToDateTime(string date)
        {
            DateTime odt = new DateTime(1900, 1, 1);
            if (date != "")
            {
                if (date.IndexOf("1900-1-1") != -1 || date.IndexOf("1900-01-01") != -1)
                {
                    date = "";
                }
                else
                {
                    DateTime.TryParse(date, out odt);
                }
            }
            return odt;
        }

        // 时间戳转为C#格式时间
        public static DateTime StampToDateTime(string timeStamp)
        {
            DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dateTimeStart.Add(toNow);
        }
        // DateTime时间格式转换为Unix时间戳格式
        public static int DateTimeToStamp(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }
        #endregion

        /// <summary>
        /// 返回获取的时间
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string GetDateTime()
        {
            return GetDateTime("");
        }
        /// <summary>
        /// 返回获取的时间
        /// </summary>
        /// <param name="format">设置时间格式</param>
        /// <returns></returns>
        public static string GetDateTime(string format)
        {
            if (string.IsNullOrEmpty(format))
                return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            return DateTime.Now.ToString(format);
        }
        /// <summary>
        /// 过滤输入,隔开的字符串，去掉重复值
        /// </summary>
        /// <param name="inputstr">输入,隔开的字符串</param>
        /// <returns></returns>
        public static string GetNoEqualsStr(string inputstr)
        {
            List<string> liststr = new List<string>();
            foreach (string str in SplitString(inputstr, ","))
            {
                if (!liststr.Contains(str))
                    liststr.Add(str);
            }
            return ListToStrs(liststr, ",");
        }

        ////----------------------
        /// <summary>
        /// List字符串值集合转换为字符串值，采用指定符号隔开，可以是,或其他
        /// </summary>
        /// <param name="list">List字符串值集合</param>
        /// <param name="nodeStr">指定符号隔开，可以是,或其他</param>
        /// <returns>返回转换后的字符串值</returns>
        public static string ListToStrs(List<string> list, string nodeStr)
        {
            return string.Join(nodeStr, list.ToArray());
        }
        /// <summary>
        /// 字符串值转换为List字符串值集合，采用指定符号隔开，可以是,或其他
        /// </summary>
        /// <param name="strs">字符串值</param>
        /// <param name="nodeStr">指定符号隔开，可以是,或其他</param>
        /// <returns>List字符串值集合</returns>
        public static List<string> StrsToList(string strs, string nodeStr)
        {
            if (string.IsNullOrEmpty(strs))
                return new List<string>();
            return new List<string>(strs.Split(new string[] { nodeStr }, StringSplitOptions.RemoveEmptyEntries));
        }


        /// <summary>
        /// 字段串是否为Null或为""(空)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool StrIsNullOrEmpty(string str)
        {
            if (str == null || str.Trim() == string.Empty)
                return true;
            return false;
        }


        /// <summary>
        /// 合并字符
        /// </summary>
        /// <param name="source">要合并的源字符串</param>
        /// <param name="target">要被合并到的目的字符串</param>
        /// <param name="mergechar">合并符</param>
        /// <returns>合并到的目的字符串</returns>
        public static string MergeString(string source, string target)
        {
            return MergeString(source, target, ",");
        }

        /// <summary>
        /// 合并字符
        /// </summary>
        /// <param name="source">要合并的源字符串</param>
        /// <param name="target">要被合并到的目的字符串</param>
        /// <param name="mergechar">合并符</param>
        /// <returns>并到字符串</returns>
        public static string MergeString(string source, string target, string mergechar)
        {
            if (StrIsNullOrEmpty(target))
                target = source;
            else
                target += mergechar + source;

            return target;
        }

        /// <summary>
        /// 计算字符串中子串出现的次数
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="substring">子串</param>
        /// <returns>出现的次数</returns>
        public static int SubstringCount(string str, string substring)
        {
            if (str.Contains(substring))
            {
                string strReplaced = str.Replace(substring, "");
                return (str.Length - strReplaced.Length) / substring.Length;
            }
            return 0;
        }

        /// <summary>
        /// 清除UBB标签
        /// </summary>
        /// <param name="sDetail">帖子内容</param>
        /// <returns>帖子内容</returns>
        public static string ClearUBB(string sDetail)
        {
            return Regex.Replace(sDetail, @"\[[^\]]*?\]", string.Empty, RegexOptions.IgnoreCase);
        }


        /// <summary>
        /// 快速判断字符串起始部分
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        public static bool StartsWithIgnoreCase(string target, string lookfor)
        {
            if (string.IsNullOrEmpty(target) || string.IsNullOrEmpty(lookfor))
            {
                return false;
            }

            if (lookfor.Length > target.Length)
            {
                return false;
            }
            return (0 == string.Compare(target, 0, lookfor, 0, lookfor.Length, StringComparison.OrdinalIgnoreCase));
        }
        /// <summary>
        /// 字符串处理方法
        /// </summary>
        /// <param name="IDs"></param>
        /// <param name="Spliter"></param>
        /// <param name="Quoter"></param>
        /// <returns></returns>
        public static string QuotIDs(string IDs, char Spliter, char Quoter)
        {
            if (IDs.Trim() == "")
                return "";
            string[] aID = IDs.Split(Spliter);
            string Result = "";
            foreach (string s in aID)
            {
                if (Result != "")
                    Result += Spliter;
                Result += Quoter + s + Quoter;
            }
            return Result;
        }


        /// <summary>
        /// 自定生成Guid数据值方法
        /// </summary>
        /// <returns>返回Guid类型值</returns>
        public static string GetNewGuid()
        {
            string result = string.Empty;
            lock (obj)
            {
                System.Guid guid = Guid.NewGuid();
                result = guid.ToString().ToLower();
            }
            return result;
        }
        /// <summary>
        /// 将字符串值转换为Guid类型值
        /// </summary>
        /// <param name="obj">字符串值</param>
        /// <returns>返回Guid类型值</returns>
        public static Guid StrToGuid(object obj)
        {
            string str = obj.ToString().Trim();
            if (string.IsNullOrEmpty(str))
                return new Guid();
            return new Guid(str);
        }
        /// <summary>
        /// 将Guid类型值转换为字符串值
        /// </summary>
        /// <param name="obj">Guid类型值</param>
        /// <returns>返回Guid类型值</returns>
        public static string GuidToStr(Guid obj)
        {
            return obj.ToString().Trim();
        }
        /// <summary>
        /// 获取随机时间格式
        /// </summary>
        /// <returns></returns>
        public static string GetRandomTime()
        {
            return GetRandomTime(2);
        }
        /// <summary>
        /// 获取随机时间格式
        /// </summary>
        /// <returns></returns>
        public static string GetRandomTime(int dateType)
        {
            if (dateType == 1)
                return DateTime.Now.ToString("yyyyMMddHHmmssfff");
            if (dateType == 2)
                return DateTime.Now.ToString("HHmmssfff");
            if (dateType == 3)
                return DateTime.Now.ToString("mmssfff");

            return DateTime.Now.ToString();
        }

        #region //进制字符串处理方法
        /// <summary>
        /// 将字符串转为16进制字符，允许中文
        /// </summary>
        /// <param name="s"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string StringToHexString(string s, Encoding encode)
        {
            byte[] b = encode.GetBytes(s);//按照指定编码将string编程字节数组
            string result = string.Empty;
            for (int i = 0; i < b.Length; i++)//逐字节变为16进制字符
            {
                result += Convert.ToString(b[i], 16);
            }
            return result;
        }

        /// <summary>
        /// 将16进制字符串转为字符串
        /// </summary>
        /// <param name="hs"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string HexStringToString(string hs, Encoding encode)
        {
            string strTemp = "";
            byte[] b = new byte[hs.Length / 2];
            for (int i = 0; i < hs.Length / 2; i++)
            {
                strTemp = hs.Substring(i * 2, 2);
                b[i] = Convert.ToByte(strTemp, 16);
            }
            //按照指定编码将字节数组变为字符串
            return encode.GetString(b);
        }

        /// <summary>
        /// 将byte[]转为16进制字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ByteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }
        /// <summary>
        /// 将16进制的字符串转为byte[]
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static byte[] StrToToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }

        /// <summary>
        /// 将byte[]转为64进制字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ByteToBase64Str(byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }
        /// <summary>
        /// 将64进制字符串转为byte[]
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static byte[] Base64StrToToByte(string hexString)
        {
            return Convert.FromBase64String(hexString);
        }

        #endregion

        #region //枚举转字典通用方法
        ///// <summary>
        ///// 根据对象类型生成字典枚举
        ///// </summary>
        ///// <param name="t">对象类型</param>
        ///// <returns>返回查询的集合</returns>
        //public static List<ListItem> GetDataDicList(Type t)
        //{
        //    List<ListItem> list = new List<ListItem>();
        //    foreach (KeyValuePair<int, string> kv in EnumDescriptionAttribute.GetDescriptions(t))
        //        list.Add(new ListItem(kv.Value.Trim(), kv.Key.ToString()));
        //    return list;
        //}

        ///// <summary>
        ///// 根据对象类型值查询名称方法
        ///// </summary>
        ///// <param name="v">value值</param>
        ///// <returns>返回查询的结果</returns>
        //public static string GetDataDicName(Type t, object v)
        //{
        //    return EnumDescriptionAttribute.GetDescription(t, Utils.ObjToInt(v, 0));
        //}
        ///// <summary>
        ///// 根据对象类型生成字典枚举
        ///// </summary>
        ///// <param name="t">对象类型</param>
        ///// <returns>返回查询的集合</returns>
        //public static List<ListItem> GetDataDicListStr(Type t)
        //{
        //    List<ListItem> list = new List<ListItem>();
        //    foreach (KeyValuePair<string, string> kv in EnumDescriptionAttribute.GetDescriptionsStr(t))
        //        list.Add(new ListItem(kv.Value.Trim(), kv.Key.ToString()));
        //    return list;
        //}

        ///// <summary>
        ///// 根据对象类型值查询名称方法
        ///// </summary>
        ///// <param name="v">value值</param>
        ///// <returns>返回查询的结果</returns>
        //public static string GetDataDicNameStr(Type t, object v)
        //{
        //    return EnumDescriptionAttribute.GetDescriptionStr(t, Utils.ObjectToStr(v));
        //}
        #endregion

        #region //在从1到30间随机生成指定个互不相同的整数
        /// <summary>
        /// 
        /// </summary>
        /// <param name="num"></param>
        /// <param name="sleep"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public static int[] GetRandomNum(int num, bool sleep, int minValue, int maxValue)
        {
            if (sleep)
                System.Threading.Thread.Sleep(3);
            string result = "";
            Random ra = new Random(unchecked((int)DateTime.Now.Ticks));
            int[] arrNum = new int[num];
            int tmp = 0;
            for (int i = 0; i <= num - 1; i++)
            {
                tmp = ra.Next(minValue, maxValue); //随机取数
                arrNum[i] = GetNum(arrNum, tmp, minValue, maxValue, ra); //取出值赋到数组中
            }
            return arrNum;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="arrNum"></param>
        /// <param name="tmp"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <param name="ra"></param>
        /// <returns></returns>
        public static int GetNum(int[] arrNum, int tmp, int minValue, int maxValue, Random ra)
        {
            int n = 0;
            while (n <= arrNum.Length - 1)
            {
                if (arrNum[n] == tmp) //利用循环判断是否有重复
                {
                    tmp = ra.Next(minValue, maxValue); //重新随机获取。
                    GetNum(arrNum, tmp, minValue, maxValue, ra);//递归:如果取出来的数字和已取得的数字有重复就重新随机获取。
                }
                n++;
            }
            return tmp;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="num"></param>
        /// <param name="nod"></param>
        /// <returns></returns>
        public static string GetRandomNumStr(int num, string nod)
        {
            int[] arr = GetRandomNum(num, true, 1, 30); //从1至30中取出10个互不相同的随机数
            int i = 0;
            string temp = "";
            while (i <= arr.Length - 1)
            {
                temp += arr[i] + "" + nod;
                i++;
            }
            if (!string.IsNullOrEmpty(temp))
                temp = temp.TrimEnd(nod.ToCharArray());
            return temp;
        }

        #endregion

        #region //字符编码通用方法
        /// <summary>
        /// 获取中英文混排字符串的实际长度(字节数)
        /// </summary>
        /// <param name="str">要获取长度的字符串</param>
        /// <returns>字符串的实际长度值（字节数）</returns>
        public static int GetStringLength(string str)
        {
            if (str.Equals(string.Empty))
            {
                return 0;
            }
            byte[] strBytes = Encoding.GetEncoding("gb2312").GetBytes(str);
            return strBytes.Length;
        }

        /// <summary>
        /// 截取指定字节位置的字符串(GB2312),推荐带有中文的字符串使用
        /// </summary>
        /// <param name="str">截取的字符串</param>
        /// <param name="startByteIndex">开始字节位置</param>
        /// <param name="byteLength">字节数</param>
        /// <returns>截取后的字符串</returns>
        public static String SubByteString_GB2312(String str, int startByteIndex, int byteLength)
        {
            byte[] substr = new byte[byteLength];
            System.Array.Copy(Encoding.GetEncoding("gb2312").GetBytes(str), startByteIndex, substr, 0, byteLength);
            return Encoding.GetEncoding("gb2312").GetString(substr);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_s"></param>
        /// <returns></returns>
        public static string Cleaner(string _s)
        {
            if (_s == null) return "";
            System.Text.StringBuilder sb = new System.Text.StringBuilder(_s);
            sb.Replace(@"\", @"\\");
            sb.Replace(@"'", @"\'");
            sb.Replace(@"""", @"\""");
            sb.Replace(Environment.NewLine, @"\n");	//替换连在一起的\r\n
            sb.Replace("\n", @"\n");				//单个替换
            sb.Replace("\r", @"\n");
            return sb.ToString();
        }
        /// <summary>
        /// Encodes a string to be represented as a string literal. The format
        /// is essentially a JSON string.
        /// 
        /// The string returned includes outer quotes 
        /// Example Output: "Hello \"Rick\"!\r\nRock on"
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string EncodeJsString(string s)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\"");
            foreach (char c in s)
            {
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\"");
                        break;
                    case '\\':
                        sb.Append("\\\\");
                        break;
                    case '\b':
                        sb.Append("\\b");
                        break;
                    case '\f':
                        sb.Append("\\f");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\r':
                        sb.Append("\\r");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    default:
                        int i = (int)c;
                        if (i < 32 || i > 127)
                        {
                            sb.AppendFormat("\\u{0:X04}", i);
                        }
                        else
                        {
                            sb.Append(c);
                        }
                        break;
                }
            }
            sb.Append("\"");

            return sb.ToString();
        }

        #endregion

        /// <summary>
        /// 返回上传目录相对路径
        /// </summary>
        /// <param name="rootPath">上传文件名</param>
        /// <param name="type">模式 0按年存入不同的文件夹 1按年存入不同的文件夹 2按年/月存入不同的文件夹 3按年/月/日存入不同的文件夹</param>
        public static string GetExpandPath(string rootPath, int type)
        {
            string path = rootPath;
            switch (type)
            {
                case 1: //按年存入不同的文件夹
                    path += DateTime.Now.ToString("yyyy");
                    break;
                case 2: //按年/月存入不同的文件夹
                    path += DateTime.Now.ToString("yyyy") + "/" + DateTime.Now.ToString("MM");
                    break;
                case 3: //按年/月/日存入不同的文件夹
                    path += DateTime.Now.ToString("yyyy") + "/" + DateTime.Now.ToString("MM") + "/" + DateTime.Now.ToString("dd");
                    break;
                default: //按年存入不同的文件夹
                    path += "";
                    break;
            }
            return path + "/";
        }


        /// <summary>
        /// 根据规则生成单据流水号
        /// </summary>
        /// <returns></returns>
        public static string CreateCode(string str = null)
        {
            var rs = "";
            if (!string.IsNullOrWhiteSpace(str))
            {
                rs += str;
            }
            rs += DateTime.Now.ToString("yyyyMMddHHmmssfff") + new Random().Next(10, 99);
            return rs;
        }
    }
}
