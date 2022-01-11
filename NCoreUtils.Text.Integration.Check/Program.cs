using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace NCoreUtils.Text.Integration.Check
{
    class Program
    {
        private sealed class Logger<T> : ILogger<T>
        {
            private sealed class DummyDisposable : IDisposable
            {
                public void Dispose() { }
            }

            public IDisposable BeginScope<TState>(TState state)
                => new DummyDisposable();

            public bool IsEnabled(LogLevel logLevel)
                => true;

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
            {
                Console.Error.WriteLine($"[{logLevel}] {formatter(state, exception)}");
            }
        }

        static void Main(string[] args)
        {
            var services = new ServiceCollection()
                // .AddLogging(b => b.ClearProviders())
                .AddTransient(typeof(ILogger<>), typeof(Logger<>))
                .AddLibicu()
                .AddDefaultStringSimplifier()
                .BuildServiceProvider(true);
            try
            {
                using var scope = services.CreateScope();
                var simplifier = scope.ServiceProvider.GetRequiredService<IStringSimplifier>();
                Console.WriteLine(simplifier.Simplify("ábra"));
            }
            finally
            {
                (services as IDisposable)?.Dispose();
            }
        }
    }
}
