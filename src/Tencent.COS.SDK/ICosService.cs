using System;

namespace Tencent.COS.SDK
{
    public interface ICosService
    {
        string Upload(byte[] data,string extensionName);
    }
}