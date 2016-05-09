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
            var sc = new StoreProceduresCreatorCommand("C:\\Projects\\Testing\\Database\\Programmability", 
                new ScriptProvider(),"1.0.0.0",5,6,7,8);
            var a=await sc.Execute();
            Debug.WriteLine(a.Output);
        }

    }
}