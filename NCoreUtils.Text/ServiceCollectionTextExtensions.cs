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
                var icu = serviceProvider.GetService<ILibicu>();
                if (icu is null)
                {
                    throw new InvalidOperationException($"Libicu provider has not been registered, use either NCoreUtils.Text.Libicu.Loader or NCoreUtils.Text.Libicu.Static package to add libicu provider.");
                }
                return new StringSimplifier(icu, delimiter, runeSimplifiers);
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
                    throw new InvalidOperationException($"Libicu provider has not been registered, use either NCoreUtils.Text.Libicu.Loader or NCoreUtils.Text.Libicu.Static packages to add libicu provider.");
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