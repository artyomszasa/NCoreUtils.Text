// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.Logging;
using NCoreUtils;
using NCoreUtils.Text;
using NCoreUtils.Text.Internal;



var loader = new LibicuResolver(new DummyLogger<LibicuResolver>());
var decomposer = new LibicuDecomposer(loader.GetInstance());
var simplifier = new StringSimplifier(decomposer, '-', ServiceCollectionTextExtensions.DefaultRuneSimplifiers);

Console.WriteLine(simplifier.Simplify("Helló, Wоrld!"));

internal class DummyDisposable : IDisposable
{
    public static DummyDisposable Singleton { get; } = new();

    public void Dispose() { /* noop */ }
}

internal class DummyLogger<T> : ILogger<T>
{
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        => DummyDisposable.Singleton;

    public bool IsEnabled(LogLevel logLevel)
        => false;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) { /* noop */ }
}