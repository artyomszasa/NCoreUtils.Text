using System;
using System.Runtime.Serialization;

namespace NCoreUtils.Text.Internal;

#if !NET8_0_OR_GREATER
[Serializable]
#endif
public class LibicuException : Exception
{
    private static string FormatErrorMessage(UErrorCode errorCode)
        => $"Libicu operation failed with code {errorCode}";

    public UErrorCode ErroCode { get; }

    public LibicuException(UErrorCode errorCode) : base(FormatErrorMessage(errorCode)) { }

#if !NET8_0_OR_GREATER
    protected LibicuException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
        ErroCode = (UErrorCode)info.GetInt32(nameof(ErroCode));
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue(nameof(UErrorCode), (int)ErroCode);
    }
#endif
}