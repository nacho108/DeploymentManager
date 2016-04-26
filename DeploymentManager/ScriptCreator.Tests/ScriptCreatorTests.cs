using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ScriptCreator.Tests
{
    [TestClass]
    public class ScriptCreatorTests
    {
        [TestMethod]
        public async Task ScriptCreatorSomething()
        {
            var sc = new ScriptCreatorCommand("C:\\Projects\\Testing\\Database\\Programmability", 
                new ScriptProvider(),"1.0.0.0",5,6,7,8);
            var a=await sc.Execute();
            Debug.WriteLine("");
        }

        [TestMethod]
        public void Run()
        {
            var parameters = GetAllParametersFromCommand("\n\tEXEC   [dbo].[fsfdsf]    EXEC  sp_executesql1   EXEC  sp_executesql2  EXEC  gfdgdfg3", "EXEC ", "sp_executesql");
            foreach (var p in parameters)
            {
                Debug.WriteLine("*"+p+"*");
            }
        }

        private IEnumerable<string> GetAllParametersFromCommand(string text, string command, string exclude)
        {
            Debug.WriteLine("length: "+text.Length);
            var lista=new List<string>();
            for (int i = 0; i < text.Length; i++)
            {
                var indexExec = text.IndexOf(command, i,StringComparison.OrdinalIgnoreCase);
                if ( indexExec == -1)
                {
                    return lista;
                }
                var indexEndCommand = indexExec + command.Length;
                var indexParameter = GetIndexNextWord(text, indexEndCommand);
                var indexNextSpace = text.IndexOf(" ", indexParameter, StringComparison.OrdinalIgnoreCase);
                if (indexNextSpace == -1)
                {
                    indexNextSpace = text.Length;
                }
                var lengthParameter = indexNextSpace - indexParameter;
                Debug.WriteLine("-- {0} {1}", indexParameter, lengthParameter);
                string parameter = text.Substring(indexParameter, lengthParameter);
                parameter = parameter.Replace("[", "");
                parameter = parameter.Replace("]", "");
                lista.Add(parameter);
                Debug.WriteLine(parameter);
                i = indexNextSpace;
            }
            return lista;
        }

        int GetIndexNextWord(string text, int startIndex)
        {
            for (int i = startIndex; i < text.Length; i++)
            {
                if (text[i] == ' ') continue;
                return i;
            }
            return -1;
        }
    }
}