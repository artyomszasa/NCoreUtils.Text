using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace NCoreUtils.Text.Internal
{
    public class LibicuResolver
    {
        private readonly object _sync = new object();

        private readonly Regex _icuucRegex;

        private readonly ILogger _logger;

        private ILibicu? _instance;

        public LibicuResolver(ILogger<LibicuResolver> logger)
        {
            _logger = logger;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD))
            {
                _icuucRegex = new Regex("^libicuuc.so.([0-9]+)$", RegexOptions.Compiled | RegexOptions.CultureInvariant);
                _logger.LogInformation("Initializing libicu resolver for linux [OSDescription = {0}].", RuntimeInformation.OSDescription);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                _icuucRegex = new Regex("^icuuc([0-9]+).dll$", RegexOptions.Compiled | RegexOptions.CultureInvariant);
                _logger.LogInformation("Initializing libicu resolver for windows [OSDescription = {0}].", RuntimeInformation.OSDescription);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                _icuucRegex = new Regex("^libicuuc.dylib.([0-9]+)$", RegexOptions.Compiled | RegexOptions.CultureInvariant);
                _logger.LogInformation("Initializing libicu resolver for osx [OSDescription = {0}].", RuntimeInformation.OSDescription);
            }
            else
            {
                throw new InvalidOperationException($"Unsupported platform: {RuntimeInformation.OSDescription}");
            }
        }

        private IEnumerable<string> GetSearchPaths()
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
        }

        private ILibicu LoadInstance()
        {
            Maybe<(IntPtr Handle, int Version)> lib = default;
            foreach (var path in GetSearchPaths())
            {
                _logger.LogDebug("Trying path: {0}.", path);
                List<(string Path, int Version)> candidates = Directory.EnumerateFiles(path).Choose(fullPath => _icuucRegex.Match(Path.GetFileName(fullPath)) switch
                {
                    Match m when m.Success => (fullPath, int.Parse(m.Groups[1].Value, NumberStyles.Integer, CultureInfo.InvariantCulture)).Just(),
                    _ => default
                }).ToList();
                lib = candidates.MaybePick(candidate =>
                {
                    try
                    {
                        var handle = NativeLibrary.Load(candidate.Path);
                        _logger.LogDebug("Successfully loaded {0}.", candidate.Path);
                        return (handle, candidate.Version).Just();
                    }
                    catch (Exception exn)
                    {
                        _logger.LogDebug(exn, "Failed to load {0}.", candidate.Path);
                        return default;
                    }
                });
                if (lib.HasValue)
                {
                    break;
                }
            }
            if (!lib.HasValue)
            {
                throw new InvalidOperationException($"Failed to load libicu from all sources Consider providing search path through LIBICU_PATH environment variable.");
            }
            var (libHandle, version) = lib.Value;
            var pGetNFCInstance = GetFunctionPtr(libHandle, "unorm2_getNFCInstance", version);
            var pGetNFDInstance = GetFunctionPtr(libHandle, "unorm2_getNFDInstance", version);
            var pGetNFKCInstance = GetFunctionPtr(libHandle, "unorm2_getNFKCInstance", version);
            var pGetNFKDInstance = GetFunctionPtr(libHandle, "unorm2_getNFKDInstance", version);
            var pGetDecomposition = GetFunctionPtr(libHandle, "unorm2_getDecomposition", version);
            var getNFCinstance = Marshal.GetDelegateForFunctionPointer<GetNormalizerInstanceDelegate>(pGetNFCInstance);
            var getNFDinstance = Marshal.GetDelegateForFunctionPointer<GetNormalizerInstanceDelegate>(pGetNFDInstance);
            var getNFKCinstance = Marshal.GetDelegateForFunctionPointer<GetNormalizerInstanceDelegate>(pGetNFKCInstance);
            var getNFKDinstance = Marshal.GetDelegateForFunctionPointer<GetNormalizerInstanceDelegate>(pGetNFKDInstance);
            var getDecomposition = Marshal.GetDelegateForFunctionPointer<GetCompositionDelegate>(pGetDecomposition);
            return new DynamicLibicu(
                getNFCinstance,
                getNFDinstance,
                getNFKCinstance,
                getNFKDinstance,
                getDecomposition
            );

            static IntPtr GetFunctionPtr(IntPtr libHandle, string name, int version)
            {
                if (!NativeLibrary.TryGetExport(libHandle, name, out var pFun))
                {
                    if (!NativeLibrary.TryGetExport(libHandle, $"{name}_{version}", out pFun))
                    {
                        throw new InvalidOperationException($"Failed to get {name} from libicu.");
                    }
                }
                return pFun;
            }
        }

        public ILibicu GetInstance()
        {
            if (null != _instance)
            {
                return _instance;
            }
            lock (_sync)
            {
                if (null != _instance)
                {
                    return _instance;
                }
                // load
                _instance = LoadInstance();
                return _instance;
            }
        }
    }
}