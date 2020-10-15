using COSXML;
using COSXML.Auth;
using COSXML.CosException;
using COSXML.Model;
using COSXML.Model.Object;
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
            string region = "ap-shanghai"; //设置一个默认的存储桶地域
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

        public void Upload(byte[] data)
        {
            // 上传对象
            try
            {
                string bucket = $"{_options.BucketName}-{_options.AppId}"; //存储桶，格式：BucketName-APPID
                string cosPath = Guid.NewGuid().ToString(); // 对象键
                PutObjectRequest putObjectRequest = new PutObjectRequest(bucket, cosPath, data);

                _cosXml.PutObject(putObjectRequest);
            }
            catch (CosClientException clientEx)
            {
                //请求失败
                _logger.LogError("CosClientException: " + clientEx);
            }
            catch (CosServerException serverEx)
            {
                //请求失败
                _logger.LogError("CosServerException: " + serverEx.GetInfo());
            }
        }
    }
}