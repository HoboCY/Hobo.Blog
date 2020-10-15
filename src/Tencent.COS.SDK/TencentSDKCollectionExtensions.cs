using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tencent.COS.SDK
{
    public static class TencentSDKServiceCollectionExtensions
    {
        public static IServiceCollection AddCos(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ICosService, CosService>();
            return serviceCollection;
        }
    }
}