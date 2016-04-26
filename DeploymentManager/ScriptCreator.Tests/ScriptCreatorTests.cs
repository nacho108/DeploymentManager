using System;
using System.Diagnostics;
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
            var sc = new ScriptCreatorCommand("C:\\Projects\\Testing\\Database\\Programmability\\dbo\\Tags", new ScriptProvider());
            var a=await sc.Execute();
            Debug.WriteLine("");
        }
    }
}