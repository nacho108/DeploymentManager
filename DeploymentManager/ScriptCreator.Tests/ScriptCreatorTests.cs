using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ScriptCreator.Tests
{
    [TestClass]
    public class ScriptCreatorTests
    {
        [TestMethod]
        public async Task ScriptCreatorSomething()
        {
            var mock=new Mock<ICurrentVersionWriter>();
            var sc = new StoreProceduresCreatorCommand("C:\\Projects\\Testing\\Database", null,OutputFolderSelect.Auto, 
                new ScriptProvider(),"1.0.0.0",5,6,7,0, mock.Object);
            var a=await sc.Execute();
            Debug.WriteLine(a.Output);
        }

    }
}