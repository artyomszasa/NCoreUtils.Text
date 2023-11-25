using System;
using System.Runtime.InteropServices;
using System.Security;

namespace NCoreUtils.Text.Internal;

[UnmanagedFunctionPointer(CallingConvention.Cdecl, SetLastError = false)]
[SuppressUnmanagedCodeSecurity]
public delegate int GetCompositionDelegate(IntPtr pNormalizer, int value, IntPtr decomposition, int capacity, IntPtr pErr);