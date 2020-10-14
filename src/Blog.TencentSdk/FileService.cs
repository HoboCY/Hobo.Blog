using COSXML;
using COSXML.Auth;
using COSXML.CosException;
using COSXML.Model;
using COSXML.Model.Bucket;
using COSXML.Transfer;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.TencentSdk
{
    public class FileService : IFileService
    {
        public void ConfigureFileService()
        {
            string appId = "";
            string region = "";
            CosXmlConfig config = new CosXmlConfig.Builder()
                .IsHttps(true)
                .SetRegion(region)
                .SetDebugLog(true)
                .Build();

            string secretId = "COS_SECRETID"; //"云 API 密钥 SecretId";
            string secretKey = "COS_SECRETKEY"; //"云 API 密钥 SecretKey";
            long durationSecond = 600;  //每次请求签名有效时长，单位为秒
            QCloudCredentialProvider cosCredentialProvider = new DefaultQCloudCredentialProvider(
              secretId, secretKey, durationSecond);

            CosXml cosXml = new CosXmlServer(config, cosCredentialProvider);

            try
            {
                string bucketName = "examplebucket-1250000000"; //格式：BucketName-APPID
                PutBucketRequest request = new PutBucketRequest(bucketName);
                //执行请求
                PutBucketResult result = cosXml.PutBucket(request);
                //请求成功
                Console.WriteLine(result.GetResultInfo());
            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                //请求失败
                Console.WriteLine("CosClientException: " + clientEx);
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                //请求失败
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
            }

            TransferConfig transferConfig = new TransferConfig();

            // 初始化 TransferManager
            TransferManager transferManager = new TransferManager(cosXml, transferConfig);

            String bucket = "examplebucket-1250000000"; //存储桶，格式：BucketName-APPID
            String cosPath = "exampleobject"; //对象在存储桶中的位置标识符，即称对象键
            String srcPath = @"temp-source-file";//本地文件绝对路径

            // 上传对象
            COSXMLUploadTask uploadTask = new COSXMLUploadTask(bucket, cosPath);
            uploadTask.SetSrcPath(srcPath);

            uploadTask.progressCallback = delegate (long completed, long total)
            {
                Console.WriteLine(String.Format("progress = {0:##.##}%", completed * 100.0 / total));
            };
            uploadTask.successCallback = delegate (CosResult cosResult)
            {
                COSXMLUploadTask.UploadTaskResult result = cosResult
                  as COSXMLUploadTask.UploadTaskResult;
                Console.WriteLine(result.GetResultInfo());
                string eTag = result.eTag;
            };
            uploadTask.failCallback = delegate (CosClientException clientEx, CosServerException serverEx)
            {
                if (clientEx != null)
                {
                    Console.WriteLine("CosClientException: " + clientEx);
                }
                if (serverEx != null)
                {
                    Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                }
            };
            transferManager.Upload(uploadTask);
        }
    }
}