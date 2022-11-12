using System;
using System.Runtime.InteropServices;

namespace NCoreUtils.Text.Internal;

public readonly struct LibicuWrapper
{
    private static IntPtr GetNormalizer(ILibicu icu)
    {
        var ptr = icu.UnmanagedGetFormDNormalizer(out var err);
        if (err.IsSuccess())
        {
            return ptr;
        }
        throw new LibicuException(err);
    }

    internal readonly ILibicu _instance;

    internal readonly IntPtr _pNormalizer;

    public LibicuWrapper(ILibicu instance)
    {
        _instance = instance;
        _pNormalizer = GetNormalizer(instance);
    }

    internal unsafe int Decompose(int value, Span<char> decomposition, out UErrorCode err)
    {
        fixed (char* pDecomposition = &MemoryMarshal.GetReference(decomposition))
        {
            return _instance.UnmanagedDecompose(_pNormalizer, value, (IntPtr)pDecomposition, decomposition.Length, out err);
        }
    }
}