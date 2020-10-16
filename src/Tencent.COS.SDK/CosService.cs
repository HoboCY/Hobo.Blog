using COSXML;
using COSXML.Auth;
using COSXML.CosException;
using COSXML.Model;
using COSXML.Model.Object;
using COSXML.Model.Tag;
using COSXML.Transfer;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tencent.COS.SDK
{
    public class CosService : ICosService
    {
        private readonly CosXml _cosXml;
        private readonly CosOptions _options;
        private readonly ILogger<CosService> _logger;

        public CosService(IOptions<CosOptions> options,
            ILogger<CosService> logger)
        {
            _options = options.Value;
            _logger = logger;

            //初始化 CosXmlConfig
            string region = _options.Region; //设置一个默认的存储桶地域
            CosXmlConfig config = new CosXmlConfig.Builder()
              .IsHttps(true)  //设置默认 HTTPS 请求
              .SetRegion(region)  //设置一个默认的存储桶地域
              .SetDebugLog(true)  //显示日志
              .Build();  //创建 CosXmlConfig 对象

            long durationSecond = 600;  //每次请求签名有效时长，单位为秒
            QCloudCredentialProvider cosCredentialProvider = new DefaultQCloudCredentialProvider(
              _options.SecretId, _options.SecretKey, durationSecond);

            _cosXml = new CosXmlServer(config, cosCredentialProvider);
        }

        public string Upload(byte[] data, string fileName)
        {
            // 上传对象
            try
            {
                string bucket = $"{_options.BucketName}-{_options.AppId}"; //存储桶，格式：BucketName-APPID
                string cosPath = fileName; // 对象键
                PutObjectRequest putObjectRequest = new PutObjectRequest(bucket, cosPath, data);

                _cosXml.PutObject(putObjectRequest);
                return $"https://{bucket}.cos.{_options.Region}.myqcloud.com/{cosPath}";
            }
            catch (CosClientException clientEx)
            {
                //请求失败
                _logger.LogError("CosClientException: " + clientEx);
                return "";
            }
            catch (CosServerException serverEx)
            {
                //请求失败
                _logger.LogError("CosServerException: " + serverEx.GetInfo());
                return "";
            }
        }

        public void Download(string name)
        {
            try
            {
                PreSignatureStruct preSignatureStruct = new PreSignatureStruct();
                preSignatureStruct.appid = _options.AppId;//腾讯云账号 APPID
                preSignatureStruct.region = _options.Region; //存储桶地域
                preSignatureStruct.bucket = $"{_options.BucketName}-{_options.AppId}"; //存储桶
                preSignatureStruct.key = name; //对象键
                preSignatureStruct.httpMethod = "GET"; //HTTP 请求方法
                preSignatureStruct.isHttps = true; //生成 HTTPS 请求 URL
                preSignatureStruct.signDurationSecond = 600; //请求签名时间为600s
                preSignatureStruct.headers = null;//签名中需要校验的 header
                preSignatureStruct.queryParameters = null; //签名中需要校验的 URL 中请求参数

                string requestSignURL = _cosXml.GenerateSignURL(preSignatureStruct);

                //下载请求预签名 URL (使用永久密钥方式计算的签名 URL)
                //string localDir = System.IO.Path.GetTempPath();//本地文件夹
                //string localFileName = "my-local-temp-file"; //指定本地保存的文件名
                //GetObjectRequest request = new GetObjectRequest(null, null, localDir, localFileName);
                ////设置下载请求预签名 URL
                //request.RequestURLWithSign = requestSignURL;
                ////设置进度回调
                //request.SetCosProgressCallback(delegate (long completed, long total)
                //{
                //    Console.WriteLine(String.Format("progress = {0:##.##}%", completed * 100.0 / total));
                //});
                ////执行请求
                //GetObjectResult result = cosXml.GetObject(request);
                ////请求成功
                //Console.WriteLine(result.GetResultInfo());
                Console.WriteLine(requestSignURL);
            }
            catch (CosClientException clientEx)
            {
                //请求失败
                Console.WriteLine("CosClientException: " + clientEx);
            }
            catch (CosServerException serverEx)
            {
                //请求失败
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
            }
        }
    }
}