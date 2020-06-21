using System;

namespace NCoreUtils.Text.Internal
{
    public struct LibicuWrapper
    {
        internal readonly ILibicu _instance;

        internal IntPtr _pNormalizer;

        public LibicuWrapper(ILibicu instance)
        {
            _instance = instance;
            _pNormalizer = default;
        }

        public IntPtr GetNormalizer(int normalizationForm)
        {
            var ptr = _instance.UnmanagedGetNormalizer(normalizationForm , out var err);
            if (err.IsSuccess())
            {
                return ptr;
            }
            throw new LibicuException(err);
        }
    }
}