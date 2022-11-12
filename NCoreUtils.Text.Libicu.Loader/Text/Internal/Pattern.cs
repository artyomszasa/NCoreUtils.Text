using System;
using System.Globalization;

namespace NCoreUtils.Text.Internal;

internal readonly struct Pattern
{
    private static bool Eq(ReadOnlySpan<char> a, ReadOnlySpan<char> b)
        => MemoryExtensions.Equals(a, b, StringComparison.InvariantCulture);

    private int MinLength { get; }

    public string Prefix { get; }

    public string Suffix { get; }

    public Pattern(string prefix, string suffix)
    {
        Prefix = prefix;
        Suffix = suffix;
        MinLength = prefix.Length + 1 + Suffix.Length;
    }

    public bool Match(ReadOnlySpan<char> input, out decimal version)
    {
        var prefixLength = Prefix.Length;
        if (Suffix.Length == 0)
        {
            if (input.Length < MinLength || !Eq(Prefix, input[..prefixLength]))
            {
                version = default;
                return false;
            }
            return decimal.TryParse(input[prefixLength..], NumberStyles.Float, CultureInfo.InvariantCulture, out version);
        }
        var suffixLength = Suffix.Length;
        if (input.Length < MinLength || !Eq(Prefix, input[..(Prefix.Length)]) || !Eq(Suffix, input[^suffixLength..]))
        {
            version = default;
            return false;
        }
        return decimal.TryParse(input[prefixLength..^suffixLength], NumberStyles.Float, CultureInfo.InvariantCulture, out version);
    }
}