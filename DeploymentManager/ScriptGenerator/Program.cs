using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using ScriptCreator;

namespace ScriptGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("---------------");
            var generatorArguments = new GeneratorArguments();

            if (!Parser.Default.ParseArguments(args, generatorArguments))
            {
                return;
            }

            int mayorVersion=0, minorVersion=0, build=0;
            string requiredVersion="";
            if (generatorArguments.MinorVersion != null || generatorArguments.MayorVersion != null ||
                generatorArguments.Build != null || generatorArguments.RequiredVersion != null)
            {
                if (!int.TryParse(generatorArguments.MayorVersion, out mayorVersion)) throw new ArgumentException("The system was not able to understand the mayor version argument !!");
                if (!int.TryParse(generatorArguments.MinorVersion, out minorVersion)) throw new ArgumentException("The system was not able to understand the minor version argument !!");
                if (!int.TryParse(generatorArguments.Build, out build)) throw new ArgumentException("The system was not able to understand the build version argument !!");
            }
            else
            {
                Console.WriteLine("Using required version and current version from file");
                var currentVersionProvider = new CurrentVersionProvider(generatorArguments.DatabaProjectFolder).GetVersion();
                mayorVersion = currentVersionProvider.Mayor;
                minorVersion = currentVersionProvider.Minor;
                build = currentVersionProvider.Build+1;
                requiredVersion = mayorVersion + "." + minorVersion + "." + currentVersionProvider.Build + ".0";
            }
            Console.WriteLine("\nParameters from command line:");
            Console.WriteLine(generatorArguments);

            Console.WriteLine("\nCurrent parameters:");
            Console.WriteLine($"RequiredVersion: {requiredVersion}");
            Console.WriteLine($"MayorVersion: {mayorVersion}");
            Console.WriteLine($"MinorVersion: {minorVersion}");
            Console.WriteLine($"Build: {build}");

            var currentVersionWriter = new NullCurrentVersionWriter();
            var storeProceduresCreatorCommand = new StoreProceduresCreatorCommand(generatorArguments.DatabaProjectFolder, 
                generatorArguments.OutputFolder, OutputFolderSelect.Specified, new ScriptProvider(), 
                requiredVersion, mayorVersion, minorVersion, build, 0, currentVersionWriter);

            var result = storeProceduresCreatorCommand.Execute();
            Console.WriteLine(result.Result);
        }
    }
}
