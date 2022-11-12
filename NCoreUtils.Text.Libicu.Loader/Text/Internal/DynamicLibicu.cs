using System;

namespace NCoreUtils.Text.Internal
{
    public unsafe class DynamicLibicu : ILibicu
    {
        private readonly GetNormalizerInstanceDelegate _getNFDinstance;

        private readonly GetCompositionDelegate _getDecomposition;

        public DynamicLibicu(GetNormalizerInstanceDelegate getNFDinstance, GetCompositionDelegate getDecomposition)
        {
            _getNFDinstance = getNFDinstance;
            _getDecomposition = getDecomposition;
        }

        public int UnmanagedDecompose(IntPtr pNormalizer, int c, IntPtr decomposition, int capacity, out UErrorCode err)
        {
            int* ierr = stackalloc int[1];
            int normalizedLen = _getDecomposition(pNormalizer, c, decomposition, capacity, (IntPtr)ierr);
            err = (UErrorCode)(*ierr);
            return err.IsSuccess() ? normalizedLen : 0;
        }

        public IntPtr UnmanagedGetFormDNormalizer(out UErrorCode err)
        {
            int* ierr = stackalloc int[1];
            var res = _getNFDinstance((IntPtr)ierr);
            err = (UErrorCode)(*ierr);
            return res;
        }
    }
}