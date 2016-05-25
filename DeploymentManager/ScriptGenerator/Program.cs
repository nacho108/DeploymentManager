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
            Console.WriteLine("*********************");
            var generatorArguments = new GeneratorArguments();

            if (!Parser.Default.ParseArguments(args, generatorArguments))
            {
                return;
            }

            int mayorVersion, minorVersion, build;

            if (!int.TryParse(generatorArguments.MayorVersion, out mayorVersion)) throw new ArgumentException("The system was not able to understand the mayor version argument !!");
            if (!int.TryParse(generatorArguments.MinorVersion, out minorVersion)) throw new ArgumentException("The system was not able to understand the minor version argument !!");
            if (!int.TryParse(generatorArguments.Build, out build)) throw new ArgumentException("The system was not able to understand the build version argument !!");

            var currentVersionWriter = new CurrentVersionWriter(generatorArguments.DatabaProjectFolder);
            var currentVersionProvider = new CurrentVersionProvider(generatorArguments.DatabaProjectFolder);
            var storeProceduresCreatorCommand = new StoreProceduresCreatorCommand(generatorArguments.DatabaProjectFolder, 
                new ScriptProvider(), generatorArguments.RequiredVersion, mayorVersion, minorVersion,build, 0, currentVersionWriter);
        }
    }
}
