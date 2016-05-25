using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace ScriptGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var generatorArguments = new GeneratorArguments();

            foreach (var s in args)
            {
                if (s == "?" || s == "--?" || s == "-?")
                {
                    //ShowHelp();      
                    return;
                }
            }

            if (!Parser.Default.ParseArguments(args, generatorArguments))
            {
                return;
            }

        }
    }
}
