using Newtonsoft.Json;
using Qiniu.CDN;
using Qiniu.CDN.Model;
using Qiniu.Common;
using Qiniu.Http;
using Qiniu.IO;
using Qiniu.IO.Model;
using Qiniu.RS;
using Qiniu.RS.Model;
using Qiniu.RSF;
using Qiniu.RSF.Model;
using Qiniu.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace SevenNiu
{
    public class Api
    {
        /// <summary>
        /// 简单上传-上传小文件
        ///  fileName 可以指定文件夹
        /// </summary>
        public static void UploadFile(string area, string fileName, string localFilePath)
        {
            // 生成(上传)凭证时需要使用此Mac
            // 这个示例单独使用了一个Settings类，其中包含AccessKey和SecretKey
            // 实际应用中，请自行设置您的AccessKey和SecretKey
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);
            string bucket = area;// "test";
            string saveKey = fileName;// "1.png";
            string localFile = localFilePath;// "D:\\QFL\\1.png";
            // 上传策略，参见 
            // https://developer.qiniu.com/kodo/manual/put-policy
            PutPolicy putPolicy = new PutPolicy();
            // 如果需要设置为"覆盖"上传(如果云端已有同名文件则覆盖)，请使用 SCOPE = "BUCKET:KEY"
            // putPolicy.Scope = bucket + ":" + saveKey;
            putPolicy.Scope = bucket;
            // 上传策略有效期(对应于生成的凭证的有效期)          
            putPolicy.SetExpires(3600);
            // 上传到云端多少天后自动删除该文件，如果不设置（即保持默认默认）则不删除
            //putPolicy.DeleteAfterDays = 1;
            // 生成上传凭证，参见
            // https://developer.qiniu.com/kodo/manual/upload-token            
            string jstr = putPolicy.ToJsonString();
            string token = Auth.CreateUploadToken(mac, jstr);
            UploadManager um = new UploadManager();
            HttpResult result = um.UploadFile(localFile, saveKey, token);
            Console.WriteLine(result);
        }

        /// <summary>
        /// 简单上传-上传字节数据
        /// </summary>
        public void UploadData()
        {
            // 生成(上传)凭证时需要使用此Mac
            // 这个示例单独使用了一个Settings类，其中包含AccessKey和SecretKey
            // 实际应用中，请自行设置您的AccessKey和SecretKey
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);
            string bucket = "test";
            string saveKey = "myfile";
            byte[] data = System.IO.File.ReadAllBytes("D:/QFL/1.mp3");
            //byte[] data = System.Text.Encoding.UTF8.GetBytes("Hello World!");
            // 上传策略，参见 
            // https://developer.qiniu.com/kodo/manual/put-policy
            PutPolicy putPolicy = new PutPolicy();
            // 如果需要设置为"覆盖"上传(如果云端已有同名文件则覆盖)，请使用 SCOPE = "BUCKET:KEY"
            // putPolicy.Scope = bucket + ":" + saveKey;
            putPolicy.Scope = bucket;
            // 上传策略有效期(对应于生成的凭证的有效期)          
            putPolicy.SetExpires(3600);
            // 上传到云端多少天后自动删除该文件，如果不设置（即保持默认默认）则不删除
            putPolicy.DeleteAfterDays = 1;
            // 生成上传凭证，参见
            // https://developer.qiniu.com/kodo/manual/upload-token            
            string jstr = putPolicy.ToJsonString();
            string token = Auth.CreateUploadToken(mac, jstr);
            FormUploader fu = new FormUploader();
            HttpResult result = fu.UploadData(data, saveKey, token);
            Console.WriteLine(result);
        }

        /// <summary>
        /// 上传（来自网络回复的）数据流
        /// </summary>
        public void UploadStream()
        {
            // 生成(上传)凭证时需要使用此Mac
            // 这个示例单独使用了一个Settings类，其中包含AccessKey和SecretKey
            // 实际应用中，请自行设置您的AccessKey和SecretKey
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);
            string bucket = "test";
            // 上传策略，参见 
            // https://developer.qiniu.com/kodo/manual/put-policy
            PutPolicy putPolicy = new PutPolicy();
            // 如果需要设置为"覆盖"上传(如果云端已有同名文件则覆盖)，请使用 SCOPE = "BUCKET:KEY"
            // putPolicy.Scope = bucket + ":" + saveKey;
            putPolicy.Scope = bucket;
            // 上传策略有效期(对应于生成的凭证的有效期)          
            putPolicy.SetExpires(3600);
            // 上传到云端多少天后自动删除该文件，如果不设置（即保持默认默认）则不删除
            putPolicy.DeleteAfterDays = 1;
            // 生成上传凭证，参见
            // https://developer.qiniu.com/kodo/manual/upload-token            
            string jstr = putPolicy.ToJsonString();
            string token = Auth.CreateUploadToken(mac, jstr);
            try
            {
                string url = "http://img.ivsky.com/img/tupian/pre/201610/09/beifang_shanlin_xuejing-001.jpg";
                var wReq = System.Net.WebRequest.Create(url) as System.Net.HttpWebRequest;
                var resp = wReq.GetResponse() as System.Net.HttpWebResponse;
                using (var stream = resp.GetResponseStream())
                {
                    // 请不要使用UploadManager的UploadStream方法，因为此流不支持查找(无法获取Stream.Length)
                    // 请使用FormUploader或者ResumableUploader的UploadStream方法
                    FormUploader fu = new FormUploader();
                    var result = fu.UploadStream(stream, "xuejing-001.jpg", token);
                    Console.WriteLine(result);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// 上传大文件，可以从上次的断点位置继续上传
        /// </summary>
        public void UploadBigFile()
        {
            // 这个示例单独使用了一个Settings类，其中包含AccessKey和SecretKey
            // 实际应用中，请自行设置您的AccessKey和SecretKey
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);
            string bucket = "test";
            string saveKey = "1.mp4";
            string localFile = "D:\\QFL\\1.mp4";
            // 断点记录文件，可以不用设置，让SDK自动生成，如果出现续上传的情况，SDK会尝试从该文件载入断点记录
            // 对于不同的上传任务，请使用不同的recordFile
            string recordFile = "D:\\QFL\\resume.12345";
            PutPolicy putPolicy = new PutPolicy();
            putPolicy.Scope = bucket;
            putPolicy.SetExpires(3600);
            string token = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());
            // 包含两个参数，并且都有默认值
            // 参数1(bool): uploadFromCDN是否从CDN加速上传，默认否
            // 参数2(enum): chunkUnit上传分片大小，可选值128KB,256KB,512KB,1024KB,2048KB,4096KB
            ResumableUploader ru = new ResumableUploader(false, ChunkUnit.U1024K);
            // ResumableUploader.UploadFile有多种形式，您可以根据需要来选择
            //
            // 最简模式，使用默认recordFile和默认uploadProgressHandler
            // UploadFile(localFile,saveKey,token)
            // 
            // 基本模式，使用默认uploadProgressHandler
            // UploadFile(localFile,saveKey,token,recordFile)
            //
            // 一般模式，使用自定义进度处理(可以监视上传进度)
            // UploadFile(localFile,saveKey,token,recordFile,uploadProgressHandler)
            //
            // 高级模式，包含上传控制(可控制暂停/继续 或者强制终止)
            // UploadFile(localFile,saveKey,token,recordFile,uploadProgressHandler,uploadController)
            // 
            // 支持自定义参数
            //var extra = new System.Collections.Generic.Dictionary<string, string>();
            //extra.Add("FileType", "UploadFromLocal");
            //extra.Add("YourKey", "YourValue");
            //uploadFile(...,extra,...)
            //最大尝试次数(有效值1~20)，在上传过程中(如mkblk或者bput操作)如果发生错误，它将自动重试，如果没有错误则无需重试
            int maxTry = 10;
            // 使用默认进度处理，使用自定义上传控制            
            UploadProgressHandler upph = new UploadProgressHandler(ResumableUploader.DefaultUploadProgressHandler);
            UploadController upctl = new UploadController(uploadControl);
            HttpResult result = ru.UploadFile(localFile, saveKey, token, recordFile, maxTry, upph, upctl);
            Console.WriteLine(result);
        }
        // 内部变量，仅作演示
        private static bool paused = false;
        /// <summary>
        /// 上传控制
        /// </summary>
        /// <returns></returns>
        private static UPTS uploadControl()
        {
            // 这个函数只是作为一个演示，实际当中请根据需要来设置
            // 这个演示的实际效果是“走走停停”，也就是停一下又继续，如此重复直至上传结束
            paused = !paused;
            if (paused)
            {
                return UPTS.Suspended;
            }
            else
            {
                return UPTS.Activated;
            }
        }

        /// <summary>
        /// 下载可公开访问的文件
        /// </summary>
        public static void DownloadFile(string downLoadUrl,string path)
        {
            // 文件URL
            string rawUrl = downLoadUrl;// "http://img.ivsky.com/img/tupian/pre/201610/09/beifang_shanlin_xuejing-001.jpg";
            // 要保存的文件名，如果已存在则默认覆盖
            string saveFile = path;// "D:\\QFL\\saved-snow.jpg";
            // 可公开访问的url，直接下载
            HttpResult result = DownloadManager.Download(rawUrl, saveFile);
            Console.WriteLine(result);
        }

        /// <summary>
        /// 下载私有空间中的文件
        /// </summary>
        public void DownloadPrivateFile()
        {
            // 这个示例单独使用了一个Settings类，其中包含AccessKey和SecretKey
            // 实际应用中，请自行设置您的AccessKey和SecretKey
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);
            string rawUrl = "http://your-bucket.bkt.clouddn.com/1.jpg";
            string saveFile = "D:\\QFL\\saved-1.jpg";
            // 设置下载链接有效期3600秒
            int expireInSeconds = 3600;
            string accUrl = DownloadManager.CreateSignedUrl(mac, rawUrl, expireInSeconds);
            // 接下来可以使用accUrl来下载文件
            HttpResult result = DownloadManager.Download(accUrl, saveFile);
            Console.WriteLine(result);
        }

        /// <summary>
        /// 获取文件基本信息
        /// </summary>
        public static StatResult State(string bucket, string key)
        {
            // 这个示例单独使用了一个Settings类，其中包含AccessKey和SecretKey
            // 实际应用中，请自行设置您的AccessKey和SecretKey
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);
            //string bucket = "test";
            //string key = "1.avi";
            BucketManager bm = new BucketManager(mac);
            StatResult result = bm.Stat(bucket, key);
            Console.WriteLine(result);
            return result;
        }

        /// <summary>
        /// 批量操作
        /// </summary>
        public void Batch()
        {
            // 这个示例单独使用了一个Settings类，其中包含AccessKey和SecretKey
            // 实际应用中，请自行设置您的AccessKey和SecretKey
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);
            // 批量操作类似于
            // op=<op1>&op=<op2>&op=<op3>...
            string batchOps = "op=OP1&op=OP2";
            BucketManager bm = new BucketManager(mac);
            var result = bm.Batch(batchOps);
            // 或者
            //string[] batch_ops={"<op1>","<op2>","<op3>",...};
            //bm.Batch(batch_ops);
            Console.WriteLine(result);
        }

        /// <summary>
        /// 获取空间列表          
        /// </summary>
        public static BucketsResult ListZone()
        {
            // 这个示例单独使用了一个Settings类，其中包含AccessKey和SecretKey
            // 实际应用中，请自行设置您的AccessKey和SecretKey
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);
            BucketManager bm = new BucketManager(mac);

            BucketsResult result = bm.Buckets();
            //Console.WriteLine(result);
            Console.WriteLine(result.Text);
            return result;
        }

        /// <summary>
        /// 获取空间文件列表          
        /// </summary>
        public static List<ListResult> ListFiles(string bucket, string marker1, string prefix1)
        {
            // 这个示例单独使用了一个Settings类，其中包含AccessKey和SecretKey
            // 实际应用中，请自行设置您的AccessKey和SecretKey
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);
            //string bucket = "gatico";
            string marker = marker1; // 首次请求时marker必须为空
            string prefix = prefix1; // 按文件名前缀保留搜索结果
            string delimiter = "/"; // 目录分割字符(比如"/")
            int limit = 1000; // 单次列举数量限制(最大值为1000)
            BucketManager bm = new BucketManager(mac);
            //List<FileDesc> items = new List<FileDesc>();
            //List<string> commonPrefixes = new List<string>();
            List<ListResult> listResults = new List<ListResult>();
            do
            {
                ListResult result = bm.ListFiles(bucket, prefix, marker, limit, delimiter);
               // Console.WriteLine(result);
                listResults.Add(result);
                marker = result.Result.Marker;
                //if (result.Result.Items != null)
                //{
                //    items.AddRange(result.Result.Items);
                //}
                //if (result.Result.CommonPrefixes != null)
                //{
                //    commonPrefixes.AddRange(result.Result.CommonPrefixes);
                //}
            } while (!string.IsNullOrEmpty(marker));
            return listResults;
            //foreach (string cp in commonPrefixes)
            //{
            //    Console.WriteLine(cp);
            //}
            //foreach(var item in items)
            //{
            //    Console.WriteLine(item.Key);
            //}
        }

        /// <summary>
        /// 设置或更新文件生命周期
        /// </summary>
        public void UpdateLifecycle()
        {
            // 这个示例单独使用了一个Settings类，其中包含AccessKey和SecretKey
            // 实际应用中，请自行设置您的AccessKey和SecretKey
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);
            string bucket = "test";
            string key = "1.jpg";

            // 新的deleteAfterDays，设置为0表示取消lifecycle
            int deleteAfterDays = 1;
            BucketManager bm = new BucketManager(mac);
            var result = bm.UpdateLifecycle(bucket, key, deleteAfterDays);
            Console.WriteLine(result);
        }
        /// <summary>
        /// 持久化并保存处理结果
        /// </summary>
        public void PfopAndSave()
        {
            string bucket = "test";
            string key = "1.avi";
            // 队列名称，如果没有，请设置为null
            // 另请参阅https://qiniu.kf5.com/hc/kb/article/112978/
            string pipeline = "MEDIAPROC_PIPELINE";
            // 接收处理结果通知的URL，另请参阅
            // http://developer.qiniu.com/code/v6/api/dora-api/pfop/pfop.html#pfop-notification
            string notifyUrl = "NOTIFY_URL";
            bool force = false;
            // 要保存的目标空间
            string dstBucket = "dest-bucket";
            string dstKey = "2.mp4";
            //string saveAsUri = StringHelper.urlSafeBase64Encode(dstBucket + ":" + dstKey);
            string saveAsUri = Base64.UrlSafeBase64Encode(dstBucket + ":" + dstKey);
            //string saveAsUri = StringHelper.UrlEncode(dstBucket + ":" + dstKey);

            // 需要执行的数据处理,例如视频转码
            string fopM = "FILE_OPS"; //示例: "avthumb/mp4";
            // 使用管道'|'命令，将处理结果saveas另存
            string fops = fopM + "|saveas/" + saveAsUri;
            // 这个示例单独使用了一个Settings类，其中包含AccessKey和SecretKey
            // 实际应用中，请自行设置您的AccessKey和SecretKey
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);
            OperationManager ox = new OperationManager(mac);
            PfopResult result = ox.Pfop(bucket, key, fops, pipeline, notifyUrl, force);
            // 稍后可以根据PersistentId查询处理进度/结果
            //string persistentId = result.PersistentId;
            //HttpResult pr = ox.Prefop(persistentId);
            //Console.WriteLine(pr);
            Console.WriteLine(result);
        }
        /// <summary>
        /// 文件缓存刷新
        /// </summary>
        public void CdnFlleRefresh()
        {
            // 这个示例单独使用了一个Settings类，其中包含AccessKey和SecretKey
            // 实际应用中，请自行设置您的AccessKey和SecretKey
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);
            CdnManager cdnMgr = new CdnManager(mac);
            string[] urls = new string[] { "http://yourdomain.bkt.clouddn.com/somefile.php" };
            var result = cdnMgr.RefreshUrls(urls);
            // 或者使用下面的方法
            //RefreshRequest request = new RefreshRequest();
            //request.AddUrls(urls);
            //var result = cdnMgr.RefreshUrlsAndDirs(request);
            Console.WriteLine(result);
        }
        /// <summary>
        /// 目录缓存刷新
        /// </summary>
        public void CdnFolderRefresh()
        {
            // 这个示例单独使用了一个Settings类，其中包含AccessKey和SecretKey
            // 实际应用中，请自行设置您的AccessKey和SecretKey
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);
            CdnManager cdnMgr = new CdnManager(mac);
            string[] dirs = new string[] { "http://yourdomain.bkt.clouddn.com/" };
            var result = cdnMgr.RefreshDirs(dirs);
            // 或者使用下面的方法
            //RefreshRequest request = new RefreshRequest();
            //request.AddDirs(dirs);
            //var result = cdnMgr.RefreshUrlsAndDirs(request);
            Console.WriteLine(result);
        }
        /// <summary>
        /// 文件预取
        /// </summary>
        public void CdnPrefetch()
        {
            // 这个示例单独使用了一个Settings类，其中包含AccessKey和SecretKey
            // 实际应用中，请自行设置您的AccessKey和SecretKey
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);
            CdnManager cdnMgr = new CdnManager(mac);
            string[] urls = new string[] { "http://yourdomain.clouddn.com/somefile.php" };
            var result = cdnMgr.PrefetchUrls(urls);
            // 或者使用下面的方法
            //PrefetchRequest request = new PrefetchRequest(urls);
            //var result = cdnMgr.PrefetchUrls(request);
            Console.WriteLine(result);
        }
        /// <summary>
        /// 查询流量
        /// </summary>
        public void GetFluxData()
        {
            // 这个示例单独使用了一个Settings类，其中包含AccessKey和SecretKey
            // 实际应用中，请自行设置您的AccessKey和SecretKey
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);
            CdnManager cdnMgr = new CdnManager(mac);
            FluxRequest request = new FluxRequest();
            request.StartDate = "2016-09-01";
            request.EndDate = "2016-09-02";
            request.Granularity = "day";
            request.Domains = "DOMAIN1;DOMAIN2";
            var result = cdnMgr.GetFluxData(request);
            Console.WriteLine(result);
        }
        /// <summary>
        /// 查询带宽
        /// </summary>
        public void GetBandwidthData()
        {
            // 这个示例单独使用了一个Settings类，其中包含AccessKey和SecretKey
            // 实际应用中，请自行设置您的AccessKey和SecretKey
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);
            CdnManager cdnMgr = new CdnManager(mac);
            BandwidthRequest request = new BandwidthRequest();
            request.StartDate = "2016-09-01";
            request.EndDate = "2016-09-20";
            request.Granularity = "day";
            request.Domains = "yourdomain.bkt.clouddn.com;yourdomain2;yourdomain3";
            var result = cdnMgr.GetBandwidthData(request);
            Console.WriteLine(result);
        }
        /// <summary>
        /// 查询日志
        /// </summary>
        public void GetCdnLogList()
        {
            // 这个示例单独使用了一个Settings类，其中包含AccessKey和SecretKey
            // 实际应用中，请自行设置您的AccessKey和SecretKey
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);
            CdnManager cdnMgr = new CdnManager(mac);
            LogListRequest request = new LogListRequest();
            request.Day = "2016-09-01"; // date:which-day
            request.Domains = "DOMAIN1;DOMAIN2"; // domains
            var result = cdnMgr.GetCdnLogList(request);
            Console.WriteLine(result);
        }
        /// <summary>
        /// 生成时间戳防盗链接
        /// </summary>
        public void HotLink()
        {
            // 这个示例单独使用了一个Settings类，其中包含AccessKey和SecretKey
            // 实际应用中，请自行设置您的AccessKey和SecretKey
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);
            CdnManager cdnMgr = new CdnManager(mac);
            TimestampAntiLeechUrlRequest request = new TimestampAntiLeechUrlRequest();
            request.Host = "http://your-host";
            request.Path = "/path/";
            request.File = "file-name";
            request.Query = "?version=1.1";
            request.SetLinkExpire(600); // 设置有效期，单位:秒
            string prefLink = cdnMgr.CreateTimestampAntiLeechUrl(request);
            Console.WriteLine(prefLink);
        }

       
        public static string DeleteFile(string buick,string filePath)
        {
            Auth auth = new Auth(new Mac(Settings.AccessKey, Settings.SecretKey));
            HttpWebRequest wReq = null;
            
            string encodedEntryURI = Base64.UrlSafeBase64Encode(buick + ":" + filePath);
            string url = "http://rs.qbox.me" + "/delete/" + encodedEntryURI;
            wReq = WebRequest.Create(url) as HttpWebRequest;
            wReq.Method = "POST";
            //wReq.UserAgent = "Go-http-client/1.1";
            wReq.Headers.Add("Authorization", auth.CreateManageToken(url));
            wReq.ContentType = "application/x-www-form-urlencoded";
            //wReq.Headers.Add("Host", "uc.qbox.me");
            HttpWebResponse wResp = wReq.GetResponse() as HttpWebResponse;
            if (wResp.StatusCode!=HttpStatusCode.OK) {
                string res = "";
                using (StreamReader sr = new StreamReader(wResp.GetResponseStream()))
                {
                    res = sr.ReadToEnd();
                }
                return res;
            }
            return null; 
           
        }

    }
}
