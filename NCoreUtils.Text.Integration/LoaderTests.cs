using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using Docker.DotNet;
using Xunit;

namespace NCoreUtils.Text.Integration
{
    public class LoaderTests
    {
        private static readonly UTF8Encoding _utf8 = new(false);

        private static string GetDockerfileTemplate()
        {
            using var stream = typeof(LoaderTests).Assembly.GetManifestResourceStream(typeof(LoaderTests).Namespace + ".loader.Dockerfile.template");
            using var reader = new StreamReader(stream!, _utf8, false, 8192, true);
            return reader.ReadToEnd();
        }

        private static void Build(string targetName, string dockerfile, string wd)
        {
            var process = new Process();
            var startInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "docker",
                Arguments = $"build -t {targetName} -f {dockerfile} {wd}"
            };
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
            if (process.ExitCode != 0)
            {
                throw new InvalidOperationException("Could not build image");
            }
        }

        private static void RmImage(string targetName)
        {
            var process = new Process();
            var startInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "docker",
                Arguments = $"rmi {targetName}"
            };
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
            if (process.ExitCode != 0)
            {
                throw new InvalidOperationException($"Could not remove image {targetName}.");
            }
        }

        private static string RunImage(string targetName)
        {
            using var output = new StringWriter();
            using var error = new StringWriter();
            using var outputDone = new ManualResetEventSlim(false);
            using var errorDone = new ManualResetEventSlim(false);
            var process = new Process();
            var startInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                FileName = "docker",
                Arguments = $"run --rm -i {targetName}"
            };
            process.StartInfo = startInfo;
            process.EnableRaisingEvents = true;
            process.OutputDataReceived += (_, arg) =>
            {
                if (!string.IsNullOrEmpty(arg.Data))
                {
                    output.WriteLine(arg.Data);
                }
                else
                {
                    outputDone.Set();
                }
            };
            process.ErrorDataReceived += (_, arg) =>
            {
                if (!string.IsNullOrEmpty(arg.Data))
                {
                    error.WriteLine(arg.Data);
                }
                else
                {
                    errorDone.Set();
                }
            };
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();
            errorDone.Wait();
            outputDone.Wait();
            if (process.ExitCode != 0)
            {
                throw new InvalidOperationException($"Could not run image {targetName}, error: {error}");
            }
            return output.ToString();
        }

        // [InlineData("net6.0", "mcr.microsoft.com/dotnet/sdk:6.0.403-bullseye-slim-amd64", "mcr.microsoft.com/dotnet/6.0.11-bullseye-slim-amd64", "linux-x64", "")]
        [InlineData("net7.0", "mcr.microsoft.com/dotnet/sdk:7.0.100-bullseye-slim-amd64", "mcr.microsoft.com/dotnet/runtime-deps:7.0.0-bullseye-slim-amd64", "linux-x64", "")]
        [Theory]
        public void RunInDocker(string framework, string tagSdk, string tagRuntime, string rid, string run)
        {
            var dockerfile = Path.ChangeExtension(Path.GetTempFileName(), "Dockerfile");
            try
            {
                var dockerfileTemplate = GetDockerfileTemplate()
                    .Replace("%TAG_SDK%", tagSdk)
                    .Replace("%TAG_RUNTIME%", tagRuntime)
                    .Replace("%RID%", rid)
                    .Replace("%RUN%", run)
                    .Replace("%FW%", framework);
                File.WriteAllText(dockerfile, dockerfileTemplate, _utf8);
                var imageName = $"ncoreutils-text-integration-check-{rid}:0.0.0";
                var path = Environment.CurrentDirectory;
                while (path!.Length > 5 && Path.GetFileName(path) != "NCoreUtils.Text")
                {
                    path = Path.GetDirectoryName(path);
                }
                Build(imageName, dockerfile, path);
                try
                {
                    var res = RunImage(imageName);
                    Assert.Equal("abra", res.Trim());
                }
                finally
                {
                    RmImage(imageName);
                }
            }
            finally
            {
                if (File.Exists(dockerfile))
                {
                    File.Delete(dockerfile);
                }
            }
        }
    }
}