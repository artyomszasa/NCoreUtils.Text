using System;

namespace NCoreUtils.Text.Internal
{
    public unsafe class DynamicLibicu : ILibicu
    {
        private readonly GetNormalizerInstanceDelegate _getNFCinstance;

        private readonly GetNormalizerInstanceDelegate _getNFDinstance;

        private readonly GetNormalizerInstanceDelegate _getNFKCinstance;

        private readonly GetNormalizerInstanceDelegate _getNFKDinstance;

        private readonly GetCompositionDelegate _getDecomposition;

        public DynamicLibicu(
            GetNormalizerInstanceDelegate getNFCinstance,
            GetNormalizerInstanceDelegate getNFDinstance,
            GetNormalizerInstanceDelegate getNFKCinstance,
            GetNormalizerInstanceDelegate getNFKDinstance,
            GetCompositionDelegate getDecomposition)
        {
            _getNFCinstance = getNFCinstance;
            _getNFDinstance = getNFDinstance;
            _getNFKCinstance = getNFKCinstance;
            _getNFKDinstance = getNFKDinstance;
            _getDecomposition = getDecomposition;
        }

        public int UnmanagedDecompose(IntPtr pNormalizer, int c, IntPtr decomposition, int capacity, out UErrorCode err)
        {
            int* ierr = stackalloc int[1];
            int normalizedLen = _getDecomposition(pNormalizer, c, decomposition, capacity, (IntPtr)ierr);
            err = (UErrorCode)(*ierr);
            return err.IsSuccess() ? normalizedLen : 0;
        }

        public IntPtr UnmanagedGetNormalizer(int normalizationForm, out UErrorCode err)
        {
            int* ierr = stackalloc int[1];
            IntPtr res;
            switch (normalizationForm)
            {
                case IcuNormalizationForms.FormC:
                    res = _getNFCinstance((IntPtr)ierr);
                    err = (UErrorCode)(*ierr);
                    return res;
                case IcuNormalizationForms.FormD:
                    res = _getNFDinstance((IntPtr)ierr);
                    err = (UErrorCode)(*ierr);
                    return res;
                case IcuNormalizationForms.FormKC:
                    res = _getNFKCinstance((IntPtr)ierr);
                    err = (UErrorCode)(*ierr);
                    return res;
                case IcuNormalizationForms.FormKD:
                    res = _getNFKDinstance((IntPtr)ierr);
                    err = (UErrorCode)(*ierr);
                    return res;
                default:
                    err = UErrorCode.ILLEGAL_ARGUMENT_ERROR;
                    return default;
            };
        }
    }
}