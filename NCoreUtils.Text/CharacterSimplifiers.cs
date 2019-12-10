using System.Collections.Generic;

namespace NCoreUtils.Text
{
    public static class CharacterSimplifiers
    {
        public static ICharacterSimplifier Hungarian { get; } = CharacterSimplifier.FromMapping(new Dictionary<char, string>
        {
            { 'ö', "o" },
            { 'ü', "u" },
            { 'ó', "o" },
            { 'ő', "o" },
            { 'ú', "u" },
            { 'é', "e" },
            { 'á', "a" },
            { 'ű', "u" },
            { 'í', "i" }
        });

        public static ICharacterSimplifier Russian { get; } = CharacterSimplifier.FromMapping(new Dictionary<char, string>
        {
            { 'а', "a" },
            { 'б', "b" },
            { 'в', "v" },
            { 'г', "g" },
            { 'д', "d" },
            { 'е', "e" },
            { 'ё', "jo" },
            { 'ж', "zh" },
            { 'з', "z" },
            { 'и', "i" },
            { 'й', "j" },
            { 'к', "k" },
            { 'л', "l" },
            { 'м', "m" },
            { 'н', "n" },
            { 'о', "o" },
            { 'п', "p" },
            { 'р', "r" },
            { 'с', "s" },
            { 'т', "t" },
            { 'у', "u" },
            { 'ф', "f" },
            { 'х', "h" },
            { 'ц', "c" },
            { 'ч', "ch" },
            { 'ш', "sh" },
            { 'щ', "sh" },
            { 'ъ', "" },
            { 'ы', "y" },
            { 'ь', "" },
            { 'э', "e" },
            { 'ю', "ju" },
            { 'я', "ja" }
        });
    }
}