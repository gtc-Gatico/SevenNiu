using Newtonsoft.Json;
using Qiniu.Http;
using Qiniu.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;

namespace SevenNiu
{
    public class Settings
    {
        public static Dictionary<string, string> config = new Dictionary<string, string>();
        public static string AccessKey = null;
        public static string SecretKey = null;
        
        public static bool isInit()
        {

            return !(AccessKey == null || SecretKey == null || "".Equals(AccessKey) || "".Equals(SecretKey));
        }

        public static void init()
        {
            Configuration configration = null;
            try
            {

                configration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                string[] allKeys = configration.AppSettings.Settings.AllKeys;
                for (int i = 0; i < allKeys.Length; i++)
                {
                    config.Add(allKeys[i], configration.AppSettings.Settings[allKeys[i]].Value);
                }
                AccessKey = config["AccessKey"];
                SecretKey = config["SecretKey"];
            }
            catch (Exception e)
            {
                Console.WriteLine("init读取配置文件失败");
            }
            finally
            {
                configration = null;
            }

        }



        public static void add(string key, string value)
        {
            Configuration configration = null;
            try
            {
                configration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                configration.AppSettings.Settings.Remove(key);
                configration.AppSettings.Settings.Add(key, value);
                config[key] = value;
            }
            catch (Exception e)
            {
                Console.WriteLine("添加配置文件失败");
            }
            finally
            {
                configration.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
                configration = null;
            }

        }

        public static void remove(string key)
        {
            Configuration configration = null;
            try
            {

                configration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                string[] allKeys = configration.AppSettings.Settings.AllKeys;
                for (int i = 0; i < allKeys.Length; i++)
                {
                    if (allKeys[i].Equals(key))
                    {
                        configration.AppSettings.Settings.Remove(key);
                        config.Remove(key);
                        break;
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("读取配置文件失败");
            }
            finally
            {
                configration.Save();
                configration = null;
            }
        }

        public static string getDomain(string buick)
        {
            //GET /v2/domains?tbl=test01 HTTP/1.1
            //Host: uc.qbox.me
            //User-Agent:Go-http-client/1.1
            //Authorization: Qiniu j853F3bLkWl59I5BOkWm6q1Z1mZClpr9Z9CLfDE0:jfblnkj6JsrlxgbE3l4SfhSyiL4=
            //Accept-Encoding: gzip
            if (config.ContainsKey(buick)) {
                return config[buick];
            }
            
            Auth auth = new Auth(new Mac(AccessKey, SecretKey));
            HttpWebRequest wReq = null;
            string url = "http://uc.qbox.me" + "/v2/domains?tbl=" + buick;
            wReq = WebRequest.Create(url) as HttpWebRequest;
            wReq.Method = "GET";
            wReq.UserAgent = "Go-http-client/1.1";
            wReq.Headers.Add("Authorization", auth.CreateManageToken(url));
            //wReq.Headers.Add("Host", "uc.qbox.me");
            HttpWebResponse wResp = wReq.GetResponse() as HttpWebResponse;
            string res = "";
            using (StreamReader sr = new StreamReader(wResp.GetResponseStream()))
            {
                res = sr.ReadToEnd();
            }
            string[] domian = JsonConvert.DeserializeObject<string[]>(res);
            if (domian.Length > 0) {
                add(buick, domian[0]);
               
                return config[buick];
            }

            return null;
        }
    }
       
}
