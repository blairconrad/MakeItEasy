namespace MakeItEasy.Build
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Xml.Linq;
    using SimpleExec;
    using static Bullseye.Targets;
    using static SimpleExec.Command;

    public class Program
    {
        private static readonly Project[] ProjectsToPack =
        {
            "src/MakeItEasy/MakeItEasy.csproj",
        };

        public static void Main(string[] args)
        {
            var testProjects = Directory.GetDirectories("tests", "MakeItEasy.Specs.FIE.*").Reverse().Select(s => new Project(s));

            Target("default", DependsOn("specs", "check-api", "pack"));

            Target(
                "build",
                () => Run("dotnet", "build MakeItEasy.sln -c Release /maxcpucount /nr:false /verbosity:minimal /nologo /bl:artifacts/logs/build.binlog"));

            Target(
                "specs",
                DependsOn("build"),
                forEach: testProjects,
                action: testProject => Run("dotnet", "test --configuration Release --no-build --nologo -- RunConfiguration.NoAutoReporters=true", testProject.Path));

            Target(
                "check-api",
                DependsOn("build"),
                () => Run("dotnet", "test --configuration Release --no-build --nologo -- RunConfiguration.NoAutoReporters=true", "tests/MakeItEasy.Api"));

            Target(
                "pack",
                DependsOn("build"),
                forEach: ProjectsToPack,
                action: project => Run("dotnet", $"pack {project.Path} --configuration Release --no-build --nologo --output {Path.GetFullPath("artifacts/output")}"));

            RunTargetsAndExit(args, messageOnly: ex => ex is NonZeroExitCodeException);
        }

        private class Project
        {
            public Project(string path) => this.Path = path.Replace('\\', '/');

            public string Path { get; }

            public static implicit operator Project(string path) => new Project(path);

            public override string ToString()
            {
                return this.Path.Split('/').Last()
                    .Replace("MakeItEasy.Specs.FIE.", "FakeItEasy ");
            }
        }
   }
}
