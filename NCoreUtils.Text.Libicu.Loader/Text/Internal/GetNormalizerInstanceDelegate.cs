using System;
using System.Runtime.InteropServices;
using System.Security;

namespace NCoreUtils.Text.Internal
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, SetLastError = false)]
    [SuppressUnmanagedCodeSecurity]
    public delegate IntPtr GetNormalizerInstanceDelegate(IntPtr perr);
}