using System;
using System.Threading.Tasks;

namespace Tencent.COS.SDK
{
    public interface ICosService
    {
        string Upload(byte[] data, string fileName);
    }
}