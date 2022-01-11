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

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            { }
        }

        private static int _initialized = 0;

        static TestBase()
        {
            if (0 == Interlocked.CompareExchange(ref _initialized, 1, 0))
            {
                NativeLibrary.SetDllImportResolver(typeof(StaticLibicu).Assembly, (libraryName, assembly, paths) =>
                {
                    if (libraryName == "NCoreUtils.Text.native")
                    {
                        var path = Path.Combine(Path.GetDirectoryName(assembly.Location)!, "runtimes", PlatformToFolder(Environment.OSVersion.Platform), "native", libraryName + ".so");
                        return NativeLibrary.Load(path);
                    }
                    return default;
                });
            }

            static string PlatformToFolder(PlatformID platformID)
                => platformID switch
                {
                    PlatformID.Unix => "linux-x64",
                    _ => "win-x64"
                };
        }

        private readonly ILibicu _dynIcu;

        private readonly ILibicu _staticIcu;

        [Obsolete]
        protected ISimplifier DynamicSimplifier
        {
            get
            {
                return new Simplifier(_dynIcu, '-', CharacterSimplifiers.Russian, CharacterSimplifiers.Hungarian);
            }
        }

        [Obsolete]
        protected ISimplifier StaticSimplifier
        {
            get
            {
                return new Simplifier(_staticIcu, '-', CharacterSimplifiers.Russian, CharacterSimplifiers.Hungarian);
            }
        }

        protected IStringSimplifier DynamicStringSimplifier
        {
            get
            {
                return new StringSimplifier(new LibicuDecomposer(_dynIcu), '-', RuneSimplifiers.German, RuneSimplifiers.Russian);
            }
        }

        protected IStringSimplifier StaticStringSimplifier
        {
            get
            {
                return new StringSimplifier(new LibicuDecomposer(_staticIcu), '-', RuneSimplifiers.German, RuneSimplifiers.Russian);
            }
        }

        public TestBase()
        {
            var resolver = new LibicuResolver(new DummyLogger<LibicuResolver>());
            _dynIcu = resolver.GetInstance();
            _staticIcu = new StaticLibicu();
        }
    }
}