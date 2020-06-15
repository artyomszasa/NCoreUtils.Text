using System;
using System.Runtime.InteropServices;
using System.Security;

namespace NCoreUtils.Text.Internal
{
    internal readonly unsafe ref struct NativeNormalizer
    {
        private const string LibName = "NCoreUtils.Text.native";

        [DllImport(LibName, EntryPoint = "ncoreutils_text_get_normalizer", SetLastError = false)]
        [SuppressUnmanagedCodeSecurity]
        private static extern IntPtr UnmanagedGetNormalizer(int normalizationForm);

        [DllImport(LibName, EntryPoint = "ncoreutils_text_normalize", SetLastError = false)]
        [SuppressUnmanagedCodeSecurity]
        private static extern int UnmanagedNormalize(IntPtr pNormalizer, IntPtr lpSrc, int cwSrcLength, IntPtr lpDst, int cwDstLength);

        [DllImport(LibName, EntryPoint = "ncoreutils_text_decompose", SetLastError = false)]
        [SuppressUnmanagedCodeSecurity]
        private static extern int UnmanagedDecompose(IntPtr pNormalizer, int c, IntPtr decomposition, int capacity);

        private readonly IntPtr _pNormalizer;

        public NativeNormalizer(int normalizationForm)
            => _pNormalizer = UnmanagedGetNormalizer(normalizationForm);

        public bool TryNormalize(ReadOnlySpan<char> source, Span<char> destination, out int written)
        {
            //fixed (char* pSource = source)
            //fixed (char* pDestination = destination)
            //{
            //
            //}
            throw new NotImplementedException();
        }

        public bool TryDecompose(int char32, Span<char> destination, out int written)
        {
            int res;
            fixed (char* pDestination = destination)
            {
                res = UnmanagedDecompose(_pNormalizer, char32, (IntPtr)pDestination, destination.Length);
            }
            if (res > 0)
            {
                written = res;
                return true;
            }
            written = default;
            return false;
        }
    }
}