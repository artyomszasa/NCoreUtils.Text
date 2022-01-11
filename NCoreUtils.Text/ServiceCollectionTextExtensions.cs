using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using NCoreUtils.Text;
using NCoreUtils.Text.Internal;

namespace NCoreUtils
{
    public static class ServiceCollectionTextExtensions
    {
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

        public static IServiceCollection AddDefaultStringSimplifier(this IServiceCollection services)
            => services.AddStringSimplifier('-', RuneSimplifiers.Russian, RuneSimplifiers.German);

        [Obsolete("AddSimplifier will be removed in future versions, use AddStringSiplifier instead.")]
        public static IServiceCollection AddSimplifier(
            this IServiceCollection services,
            char delimiter,
            IEnumerable<ICharacterSimplifier> characterSimplifiers)
        {
            return services.AddSingleton<ISimplifier>(serviceProvider =>
            {
                var icu = serviceProvider.GetService<ILibicu>();
                if (icu is null)
                {
                    throw new InvalidOperationException($"Libicu provider has not been registered, use NCoreUtils.Text.Libicu.Loader package to add libicu provider.");
                }
                return new Simplifier(icu, delimiter, characterSimplifiers);
            });
        }

        [Obsolete("AddSimplifier will be removed in future versions, use AddStringSiplifier instead.")]
        public static IServiceCollection AddSimplifier(
            this IServiceCollection services,
            char delimiter,
            params ICharacterSimplifier[] characterSimplifiers)
            => services.AddSimplifier(delimiter, (IEnumerable<ICharacterSimplifier>)characterSimplifiers);

        [Obsolete("AddDefaultSimplifier will be removed in future versions, use AddDefaultStringSiplifier instead.")]
        public static IServiceCollection AddDefaultSimplifier(this IServiceCollection services)
            => services.AddSimplifier('-', CharacterSimplifiers.Hungarian, CharacterSimplifiers.Russian);
    }
}