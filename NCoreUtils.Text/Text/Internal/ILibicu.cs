using System;

namespace NCoreUtils.Text.Internal;

public interface ILibicu
{
    IntPtr UnmanagedGetFormDNormalizer(out UErrorCode err);

    int UnmanagedDecompose(IntPtr pNormalizer, int c, IntPtr decomposition, int capacity, out UErrorCode err);
}