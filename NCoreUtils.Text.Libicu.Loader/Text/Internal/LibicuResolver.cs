using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;

namespace NCoreUtils.Text.Internal
{
    public class LibicuResolver
    {
        private readonly object _sync = new();

        private readonly Pattern _icuucPattern;

        private readonly ILogger _logger;

        private ILibicu? _instance;

        public LibicuResolver(ILogger<LibicuResolver> logger)
        {
            _logger = logger;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD))
            {
                _icuucPattern = new("libicuuc.so.", string.Empty);
                _logger.LogInformation("Initializing libicu resolver for linux [OSDescription = {Os}].", RuntimeInformation.OSDescription);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                _icuucPattern = new("icuuc", ".dll");
                _logger.LogInformation("Initializing libicu resolver for windows [OSDescription = {Os}].", RuntimeInformation.OSDescription);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                _icuucPattern = new("libicuuc.", ".dylib");
                _logger.LogInformation("Initializing libicu resolver for osx [OSDescription = {Os}].", RuntimeInformation.OSDescription);
            }
            else
            {
                throw new InvalidOperationException($"Unsupported platform: {RuntimeInformation.OSDescription}");
            }
        }

        private static IEnumerable<string> GetSearchPaths()
        {
            var env = Environment.GetEnvironmentVariable("LIBICU_PATH");
            if (!string.IsNullOrEmpty(env))
            {
                yield return env;
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                yield return "/usr/lib/x86_64-linux-gnu";
                yield return "/lib/x86_64-linux-gnu";
                yield return "/usr/local/lib";
                yield return "/usr/lib";
                yield return "/lib";
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                yield return "C:\\Windows\\System32";
            }
        }

        private ILibicu LoadInstance()
        {
            foreach (var path in GetSearchPaths())
            {
                if (!Directory.Exists(path))
                {
                    _logger.LogDebug("Skipping path: {Path} (no such directory).", path);
                    continue;
                }
                _logger.LogDebug("Trying path: {Path}.", path);
                foreach (var fullPath in Directory.EnumerateFiles(path))
                {
                    if (_icuucPattern.Match(Path.GetFileName(fullPath), out var version))
                    {
                        try
                        {
                            if (NativeLibrary.TryLoad(fullPath, out var handle))
                            {
                                _logger.LogDebug("Successfully loaded {Path} [Version = {Version}].", fullPath, version);
                                var pGetNFDInstance = GetFunctionPtr(handle, "unorm2_getNFDInstance", version);
                                var pGetDecomposition = GetFunctionPtr(handle, "unorm2_getDecomposition", version);
                                var getNFDinstance = Marshal.GetDelegateForFunctionPointer<GetNormalizerInstanceDelegate>(pGetNFDInstance);
                                var getDecomposition = Marshal.GetDelegateForFunctionPointer<GetCompositionDelegate>(pGetDecomposition);
                                return new DynamicLibicu(getNFDinstance, getDecomposition);
                            }
                            else
                            {
                                _logger.LogWarning("Failed to load {Path}.", fullPath);
                            }
                        }
                        catch (Exception exn)
                        {
                            _logger.LogWarning(exn, "Failed to load {Path}.", fullPath);
                        }
                    }
                }
            }
            throw new InvalidOperationException($"Failed to load libicu from all sources Consider providing search path through LIBICU_PATH environment variable.");

            static IntPtr GetFunctionPtr(IntPtr libHandle, string name, decimal version)
            {
                if (!NativeLibrary.TryGetExport(libHandle, name, out var pFun))
                {
                    if (!NativeLibrary.TryGetExport(libHandle, $"{name}_{(int)Math.Floor(version)}", out pFun))
                    {
                        throw new InvalidOperationException($"Failed to get {name} from libicu.");
                    }
                }
                return pFun;
            }
        }

        public ILibicu GetInstance()
        {
            if (_instance is null)
            {
                lock (_sync)
                {
                    return _instance ??= LoadInstance();
                }
            }
            return _instance;
        }
    }
}