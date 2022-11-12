using System;
using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Versioning;
using System.Threading;
using NCoreUtils.Text.Internal;

namespace NCoreUtils.Text.Wasm;

[SupportedOSPlatform("browser")]
public partial class JsInteropDecomposer : IDecomposer
{
    private const string ModuleName = "NCoreUtils.Text.Wasm.JsInterop";

    private const string JsCode = @"export function decompose(code, buffer) {
    const input = String.fromCodePoint(code);
    const normalized = input.normalize('NFD');
    if (input === normalized || normalized.length > buffer.length) { return 0; }
    const data = new Int32Array(normalized.length);
    for (let i = 0; i < normalized.length; ++i) {
        data[i] = normalized.codePointAt(i);
    }
    buffer.set(data);
    return data.length;
}";

    private static int _isInitialized;

    static JsInteropDecomposer()
    {
        InitializeAsync();
    }

    internal static void InitializeAsync()
    {
        if (0 == Interlocked.CompareExchange(ref _isInitialized, 1, 0))
        {
            _ = JSHost.ImportAsync(ModuleName, "data:text/javascript," + JsCode);
        }
    }

    [JSImport("decompose", ModuleName)]
    internal static partial int JsDecompose(int unicodeScalar, [JSMarshalAs<JSType.MemoryView>] Span<int> buffer);

    public bool TryDecompose(int unicodeScalar, Span<char> decomposition, out int written)
    {
        Span<int> buffer = stackalloc int[8];
        var size = JsDecompose(unicodeScalar, buffer);
        if (size == 0 || size > decomposition.Length)
        {
            written = 0;
            return false;
        }
        for (var i = 0; i < size; ++i)
        {
            decomposition[i] = unchecked((char)buffer[i]);
        }
        written = size;
        return true;
    }
}