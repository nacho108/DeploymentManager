using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ScriptCreator.Tests
{
    [TestClass]
    public class ScriptProviderTests
    {
        [TestMethod]
        public async Task ScriptProviderShouldReturnAllFiles()
        {
            ScriptProvider sp=new ScriptProvider();
            var scripts=await sp.GetScripts("C:\\Projects\\Testing\\Database\\Programmability", Depth.AllChilds);
            Debug.WriteLine($"Number of scripts: {scripts.Count()}");
        }
    }
}
