using Microsoft.Extensions.DependencyInjection;
using NCoreUtils.Text.Internal;

namespace NCoreUtils;

public static class ServiceCollectionLibicuLoaderExtensions
{
    public static IServiceCollection AddLibicu(this IServiceCollection services)
        => services
            .AddSingleton<LibicuResolver>()
            .AddSingleton(serviceProvider => serviceProvider.GetRequiredService<LibicuResolver>().GetInstance())
            .AddSingleton<LibicuDecomposer>()
            .AddSingleton<IDecomposer>(serviceProvider => serviceProvider.GetRequiredService<LibicuDecomposer>());
}