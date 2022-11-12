using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using NCoreUtils.Text;
using NCoreUtils.Text.Internal;

namespace NCoreUtils;

public static class ServiceCollectionTextExtensions
{
    public static IReadOnlyList<IRuneSimplifier> DefaultRuneSimplifiers { get; }
        = new[] { RuneSimplifiers.Cyrillic, RuneSimplifiers.German };

    public static IServiceCollection AddStringSimplifier(
        this IServiceCollection services,
        char delimiter,
        IEnumerable<IRuneSimplifier> runeSimplifiers)
    {
        return services.AddSingleton<IStringSimplifier>(serviceProvider =>
        {
            var decomposer = serviceProvider.GetService<IDecomposer>();
            if (decomposer is null)
            {
                throw new InvalidOperationException($"Decomposer has not been registered, use one of NCoreUtils.Text.Libicu.Loader, NCoreUtils.Text.Libicu.Static or NCoreUtils.Text.Wasm package to add decomposition provider.");
            }
            return new StringSimplifier(decomposer, delimiter, runeSimplifiers);
        });
    }

    public static IServiceCollection AddStringSimplifier(
        this IServiceCollection services,
        char delimiter,
        params IRuneSimplifier[] runeSimplifiers)
        => services.AddStringSimplifier(delimiter, (IEnumerable<IRuneSimplifier>)runeSimplifiers);

    public static IServiceCollection AddDefaultStringSimplifier(
        this IServiceCollection services,
        IEnumerable<IRuneSimplifier>? additionalRuneSimplifiers)
        => services.AddStringSimplifier(
            '-',
            additionalRuneSimplifiers is null
                ? DefaultRuneSimplifiers
                : DefaultRuneSimplifiers.Concat(additionalRuneSimplifiers)
        );

    public static IServiceCollection AddDefaultStringSimplifier(this IServiceCollection services)
        => services.AddDefaultStringSimplifier(default);
}