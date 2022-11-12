using System.Runtime.Versioning;
using Microsoft.Extensions.DependencyInjection;
using NCoreUtils.Text.Internal;
using NCoreUtils.Text.Wasm;

namespace NCoreUtils
{
    public static class ServiceCollectionWasmTextExtensions
    {

#if !NETSTANDARD2_1
        [SupportedOSPlatform("browser")]
#endif
        public static IServiceCollection AddJsInteropDecomposer(this IServiceCollection services)
#if NET7_0_OR_GREATER
        {
            JsInteropDecomposer.InitializeAsync();
            return services.AddSingleton<IDecomposer, JsInteropDecomposer>();
        }
#else
            => services.AddSingleton<IDecomposer, JsInteropDecomposer>();
#endif
    }
}