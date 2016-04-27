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
            var parameters = GetAllProceduresExecuted("\n\tEXEC   [dbo].[fsfdsf]    EXEC  sp_executesql   EXECute mongo  EXEC  1gfdgdfg3");
            foreach (var p in parameters)
            {
                Debug.WriteLine("*"+p+"*");
            }
        }

        private IEnumerable<string> GetAllProceduresExecuted(string text)
        {
            var lista=new List<string>();
            for (int i = 0; i < text.Length; i++)
            {
                var indexExec = text.IndexOf("EXEC ", i,StringComparison.OrdinalIgnoreCase);
                var indexExecute = text.IndexOf("EXECUTE ", i, StringComparison.OrdinalIgnoreCase);
                if (indexExec == -1 && indexExecute == -1)
                {
                    return lista;
                }
                int indexCommand;
                if (indexExec == -1 || indexExecute == -1)
                {
                    indexCommand = Math.Max(indexExec, indexExecute);
                }
                else
                {
                    indexCommand = Math.Min(indexExec, indexExecute);
                }
                var indexParameter = GetIndexNextWord(text, indexCommand);
                var indexNextSpace = text.IndexOf(" ", indexParameter, StringComparison.OrdinalIgnoreCase);
                if (indexNextSpace == -1)
                {
                    indexNextSpace = text.Length;
                }
                var lengthParameter = indexNextSpace - indexParameter;
                string parameter = text.Substring(indexParameter, lengthParameter);
                parameter = parameter.Replace("[", "");
                parameter = parameter.Replace("]", "");
                if (parameter.ToLower() != "sp_executesql")
                {
                    lista.Add(parameter);
                }
                i = indexNextSpace;
            }
            return lista;
        }

        int GetIndexNextWord(string text, int startIndex)
        {
            bool spaceFound = false;
            for (int i = startIndex; i < text.Length; i++)
            {
                if (text[i] == ' ')
                {
                    spaceFound = true;
                    continue;
                }
                if (spaceFound) return i;
            }
            return -1;
        }
    }
}