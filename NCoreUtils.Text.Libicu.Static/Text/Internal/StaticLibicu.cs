using System;
using System.Runtime.InteropServices;
using System.Security;

namespace NCoreUtils.Text.Internal
{
    public unsafe class StaticLibicu : ILibicu
    {
        private const string LibName = "NCoreUtils.Text.native";

        [DllImport(LibName, EntryPoint = "ncoreutils_text_get_normalizer", SetLastError = false)]
        [SuppressUnmanagedCodeSecurity]
        private static extern IntPtr ExternGetNormalizer(int normalizationForm, IntPtr err);

        // [DllImport(LibName, EntryPoint = "ncoreutils_text_normalize", SetLastError = false)]
        // [SuppressUnmanagedCodeSecurity]
        // private static extern int UnmanagedNormalize(IntPtr pNormalizer, IntPtr lpSrc, int cwSrcLength, IntPtr lpDst, int cwDstLength);

        [DllImport(LibName, EntryPoint = "ncoreutils_text_decompose", SetLastError = false)]
        [SuppressUnmanagedCodeSecurity]
        private static extern int ExternDecompose(IntPtr pNormalizer, int c, IntPtr decomposition, int capacity, IntPtr err);

        public int UnmanagedDecompose(IntPtr pNormalizer, int c, IntPtr decomposition, int capacity, out UErrorCode err)
        {
            int* ierr = stackalloc int[1];
            var res = ExternDecompose(pNormalizer, c, decomposition, capacity, (IntPtr)ierr);
            err = (UErrorCode)(*ierr);
            return res;
        }

        public IntPtr UnmanagedGetNormalizer(int normalizationForm, out UErrorCode err)
        {
            int* ierr = stackalloc int[1];
            var res = ExternGetNormalizer(normalizationForm, (IntPtr)ierr);
            err = (UErrorCode)(*ierr);
            return res;
        }
    }
}