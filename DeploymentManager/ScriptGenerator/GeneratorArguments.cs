using CommandLine;
using CommandLine.Text;

namespace ScriptGenerator
{
    public class GeneratorArguments
    {
        [Option("dbproject", Required = true, HelpText = "Database project path")]
        public string DatabaProjectFolder { get; set; }

        [Option("outputFolder", HelpText = "Output folder for the resulting script")]
        public string OutputFolder { get; set; }

        [Option("requiredVersion", HelpText = "Required version in format M.m.b.0")]
        public string RequiredVersion { get; set; }

        [Option("mayorVersion", HelpText = "Mayor version in format M")]
        public string MayorVersion { get; set; }

        [Option("minorVersion", HelpText = "Minor version in format m")]
        public string MinorVersion { get; set; }

        [Option("buildVersion", HelpText = "Build version in format B")]
        public string Build { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
                (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }

        public override string ToString()
        {
            return $"DatabaProjectFolder:{DatabaProjectFolder}\n" +
                   $"outputFolder:{OutputFolder}\n" +
                   $"requiredVersion:{RequiredVersion}\n" +
                   $"mayorVersion:{MayorVersion}\n" +
                   $"minorVersion:{MinorVersion}\n" +
                   $"build:{Build}\n";
        }
    }
}