using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ScriptProvider.Tests
{
    [TestClass]
    public class ScriptProviderTests
    {
        [TestMethod]
        public void ScriptProvider()
        {
            ScriptProvider sp=new ScriptProvider("c:\\");
        }
    }
}
