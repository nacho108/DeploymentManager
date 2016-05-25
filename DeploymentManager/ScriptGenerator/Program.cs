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
                Console.WriteLine("Something failed !");
                return;
            }
            Console.WriteLine("Running scriptgenerator with the following arguments:");
            Console.WriteLine(generatorArguments);

            int mayorVersion, minorVersion, build;
            if (generatorArguments.MinorVersion != null || generatorArguments.MayorVersion != null ||
                generatorArguments.Build != null || generatorArguments.RequiredVersion != null)
            {
                if (!int.TryParse(generatorArguments.MayorVersion, out mayorVersion)) throw new ArgumentException("The system was not able to understand the mayor version argument !!");
                if (!int.TryParse(generatorArguments.MinorVersion, out minorVersion)) throw new ArgumentException("The system was not able to understand the minor version argument !!");
                if (!int.TryParse(generatorArguments.Build, out build)) throw new ArgumentException("The system was not able to understand the build version argument !!");
            }
            else
            {
                var currentVersionProvider = new CurrentVersionProvider(generatorArguments.DatabaProjectFolder).GetVersion();
                mayorVersion = currentVersionProvider.Mayor;
                minorVersion = currentVersionProvider.Minor;
                build = currentVersionProvider.Build;
            }

            var currentVersionWriter = new NullCurrentVersionWriter();

            var storeProceduresCreatorCommand = new StoreProceduresCreatorCommand(generatorArguments.DatabaProjectFolder,
                new ScriptProvider(), generatorArguments.RequiredVersion, mayorVersion, minorVersion, build, 0, currentVersionWriter);

            var result = storeProceduresCreatorCommand.Execute();
            Console.WriteLine(result);
        }
    }
}
