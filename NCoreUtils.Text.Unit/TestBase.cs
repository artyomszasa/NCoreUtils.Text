using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Extensions.Logging;
using NCoreUtils.Text.Internal;

namespace NCoreUtils.Text.Unit
{
    public abstract class TestBase
    {
        private sealed class DummyLogger<T> : ILogger<T>
        {
            private sealed class DummyDisposable : IDisposable
            {
                public void Dispose() { }
            }

            public IDisposable BeginScope<TState>(TState state) => new DummyDisposable();

            public bool IsEnabled(LogLevel logLevel) => true;

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
            { }
        }

        private readonly ILibicu _dynIcu;

        [Obsolete("Backward compatibility test")]
        protected ISimplifier DynamicSimplifier
        {
            get
            {
                return new Simplifier(_dynIcu, '-', CharacterSimplifiers.Russian, CharacterSimplifiers.Hungarian);
            }
        }

        protected IStringSimplifier DynamicStringSimplifier
        {
            get
            {
                return new StringSimplifier(new LibicuDecomposer(_dynIcu), '-', RuneSimplifiers.German, RuneSimplifiers.Russian);
            }
        }

        public TestBase()
        {
            var resolver = new LibicuResolver(new DummyLogger<LibicuResolver>());
            _dynIcu = resolver.GetInstance();
        }
    }
}