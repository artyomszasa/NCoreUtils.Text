using Microsoft.Extensions.DependencyInjection;
using NCoreUtils.Text.Internal;
using NCoreUtils.Text.Wasm;

namespace NCoreUtils
{
    public static class ServiceCollectionWasmTextExtensions
    {
        public static IServiceCollection AddJsInteropDecomposer(this IServiceCollection services)
            => services.AddSingleton<IDecomposer, JsInteropDecomposer>();
    }
}