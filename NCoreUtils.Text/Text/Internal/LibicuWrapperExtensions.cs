using System;
using System.Runtime.CompilerServices;

namespace NCoreUtils.Text.Internal
{
    public static class LibicuWrapperExtensions
    {
        public static unsafe bool TryDecompose(this in LibicuWrapper inst, int value, Span<char> decomposition, out int written)
        {
            if (inst._pNormalizer == IntPtr.Zero)
            {
                Unsafe.AsRef(inst._pNormalizer) = inst.GetNormalizer(IcuNormalizationForms.FormD);
            }
            UErrorCode err;
            int size;
            fixed (char* pDecomposition = decomposition)
            {
                size = inst._instance.UnmanagedDecompose(inst._pNormalizer, value, (IntPtr)pDecomposition, decomposition.Length, out err);
            }
            if (size > 0 && err.IsSuccess())
            {
                written = size;
                return true;
            }
            if (size <= 0 || err == UErrorCode.BUFFER_OVERFLOW_ERROR)
            {
                written = 0;
                return false;
            }
            throw new LibicuException(err);
        }
    }
}