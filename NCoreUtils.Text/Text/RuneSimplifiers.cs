using System;
using System.Collections.Generic;
#if !NETSTANDARD2_1
using System.Text;
#endif


namespace NCoreUtils.Text
{
    public static class RuneSimplifiers
    {
        public static IRuneSimplifier German { get; } = RuneSimplifier.FromMapping(new Dictionary<Rune, string>
        {
            { new Rune('ẞ'), "SS"},
            { new Rune('ß'), "ss" }
        });

        public static IRuneSimplifier Russian { get; } = RuneSimplifier.FromMapping(new Dictionary<Rune, string>
        {
            { new Rune('а'), "a" },
            { new Rune('б'), "b" },
            { new Rune('в'), "v" },
            { new Rune('г'), "g" },
            { new Rune('д'), "d" },
            { new Rune('е'), "e" },
            { new Rune('ё'), "jo" },
            { new Rune('ж'), "zh" },
            { new Rune('з'), "z" },
            { new Rune('и'), "i" },
            { new Rune('й'), "j" },
            { new Rune('к'), "k" },
            { new Rune('л'), "l" },
            { new Rune('м'), "m" },
            { new Rune('н'), "n" },
            { new Rune('о'), "o" },
            { new Rune('п'), "p" },
            { new Rune('р'), "r" },
            { new Rune('с'), "s" },
            { new Rune('т'), "t" },
            { new Rune('у'), "u" },
            { new Rune('ф'), "f" },
            { new Rune('х'), "h" },
            { new Rune('ц'), "c" },
            { new Rune('ч'), "ch" },
            { new Rune('ш'), "sh" },
            { new Rune('щ'), "sh" },
            { new Rune('ъ'), "" },
            { new Rune('ы'), "y" },
            { new Rune('ь'), "" },
            { new Rune('э'), "e" },
            { new Rune('ю'), "ju" },
            { new Rune('я'), "ja" },
            { new Rune('А'), "A" },
            { new Rune('Б'), "B" },
            { new Rune('В'), "V" },
            { new Rune('Г'), "G" },
            { new Rune('Д'), "D" },
            { new Rune('Е'), "E" },
            { new Rune('Ё'), "JO" },
            { new Rune('Ж'), "ZH" },
            { new Rune('З'), "Z" },
            { new Rune('И'), "I" },
            { new Rune('Й'), "J" },
            { new Rune('К'), "K" },
            { new Rune('Л'), "L" },
            { new Rune('М'), "M" },
            { new Rune('Н'), "N" },
            { new Rune('О'), "O" },
            { new Rune('П'), "P" },
            { new Rune('Р'), "R" },
            { new Rune('С'), "S" },
            { new Rune('Т'), "T" },
            { new Rune('У'), "U" },
            { new Rune('Ф'), "F" },
            { new Rune('Х'), "H" },
            { new Rune('Ц'), "C" },
            { new Rune('Ч'), "CH" },
            { new Rune('Ш'), "SH" },
            { new Rune('Щ'), "SH" },
            { new Rune('Ъ'), "" },
            { new Rune('Ы'), "Y" },
            { new Rune('Ь'), "" },
            { new Rune('Э'), "E" },
            { new Rune('Ю'), "JU" },
            { new Rune('Я'), "JA" }
        });
    }
}