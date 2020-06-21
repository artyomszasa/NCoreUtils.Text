using Microsoft.Extensions.DependencyInjection;
using NCoreUtils.Text.Internal;

namespace NCoreUtils
{
    public static class ServiceCollectionStaticLibicuExtensions
    {
        public static IServiceCollection AddLibicu(this IServiceCollection services)
            => services.AddSingleton<ILibicu>(new StaticLibicu());
    }
}