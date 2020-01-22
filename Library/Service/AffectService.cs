using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Library.Model;
using Newtonsoft.Json;
using Common;
using System.IO;
using Common.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Library.Service
{
    public class AffectService
    {
        public bool ZipFile(string dirPath, string zipFilePath)
        {
            return HttpUtils.ZipFile(dirPath, zipFilePath);
        }

        public bool uploadImg(string zipImgPath, UploadObj imgInfo)
        {
            EncryptedNameValueSectionHandler hand = new EncryptedNameValueSectionHandler();
            bool result = false;
            string infoJson = JsonConvert.SerializeObject(imgInfo);
            List<FormItemModel> formDatas = new List<Common.FormItemModel>();
            //添加JSON参数
            FormItemModel jsModel = new Common.FormItemModel();
            jsModel.Key = "data";
            jsModel.Value = infoJson;
            formDatas.Add(jsModel);
            //添加zip文件流
            FormItemModel fileModel = new Common.FormItemModel();
            fileModel.Key = "file";
            fileModel.Value = "";
            fileModel.FileName = zipImgPath;
            //fileModel.FileContent = File.OpenRead(zipImgPath);
            formDatas.Add(fileModel);
            //调用方法
            string url = hand.ReadConfig("url");
            url = url + "/client/upload";
            Logger.Info("传参文件名==" + zipImgPath + "传参JSON==" + infoJson);

            string resultJson = HttpUtils.PostForm2(url, formDatas);

            Logger.Info("返回结果==" + resultJson);
            if (string.IsNullOrEmpty(resultJson))
            {
                result = false;
            }
            else
            {
                JObject obj = (JObject)JsonConvert.DeserializeObject(resultJson);
                if (obj["code"].ToString().Equals("0"))
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }

            return result;
        }

        public bool uploadImg2(string zipImgPath, string infoJson)
        {
            EncryptedNameValueSectionHandler hand = new EncryptedNameValueSectionHandler();
            bool result = false;
            List<FormItemModel> formDatas = new List<Common.FormItemModel>();
            //添加JSON参数
            FormItemModel jsModel = new Common.FormItemModel();
            jsModel.Key = "data";
            jsModel.Value = infoJson;
            formDatas.Add(jsModel);
            //添加zip文件流
            FormItemModel fileModel = new Common.FormItemModel();
            fileModel.Key = "file";
            fileModel.Value = "";
            fileModel.FileName = zipImgPath;
            //fileModel.FileContent = File.OpenRead(zipImgPath);
            formDatas.Add(fileModel);
            //调用方法
            string url = hand.ReadConfig("url");
            url = url + "/client/upload";
            Logger.Info("传参文件名==" + zipImgPath + "传参JSON==" + infoJson);

            string resultJson = HttpUtils.PostForm2(url, formDatas);

            Logger.Info("返回结果==" + resultJson);
            if (string.IsNullOrEmpty(resultJson))
            {
                result = false;
            }
            else
            {
                JObject obj = (JObject)JsonConvert.DeserializeObject(resultJson);
                if (obj["code"].ToString().Equals("0"))
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }

            return result;
        }

        public bool ZipFile(List<string> lstFilePath, string zipName)
        {
            return HttpUtils.ZipFile(lstFilePath, zipName);
        }

        public string JosnToStr(Object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
