using System;
using System.Buffers;
using Microsoft.JSInterop;
using NCoreUtils.Text.Internal;

namespace NCoreUtils.Text.Wasm
{
    public class JsInteropDecomposer : IDecomposer
    {
        private bool IsInitialized { get; set; }

        public IJSInProcessRuntime Runtime { get; }

        public JsInteropDecomposer(IJSRuntime runtime)
        {
            if (runtime is IJSInProcessRuntime r)
            {
                Runtime = r;
            }
            else
            {
                throw new InvalidOperationException("JsInteropDecomposer can only be used on the client side.");
            }
        }

        public bool TryDecompose(int unicodeScalar, Span<char> decomposition, out int written)
        {
            if (!IsInitialized)
            {
                Runtime.InvokeVoid("eval", "window.jsNCoreUtilsTextDecompose=function(code){return Array.from(String.fromCodePoint(code).normalize('NFD')).map(c => c.codePointAt(0))}");
                IsInitialized = true;
            }
            var data = Runtime.Invoke<int[]>("jsNCoreUtilsTextDecompose", unicodeScalar);
            if (!(data is null) && data.Length > 1 && data.Length <= decomposition.Length)
            {
                var buffer = ArrayPool<char>.Shared.Rent(data.Length);
                try
                {
                    for (var i = 0; i < data.Length; ++i)
                    {
                        buffer[i] = (char)data[i];
                    }
                    buffer.AsSpan().Slice(0, data.Length).CopyTo(decomposition);
                    written = data.Length;
                    return true;
                }
                finally
                {
                    ArrayPool<char>.Shared.Return(buffer);
                }
            }
            written = 0;
            return false;
        }
    }
}