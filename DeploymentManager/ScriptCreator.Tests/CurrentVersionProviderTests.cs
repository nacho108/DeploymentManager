using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ScriptCreator.Tests
{
    [TestClass]
    public class CurrentVersionProviderTests
    {
        [TestMethod]
        public void GetVersion()
        {
            CurrentVersionProvider cvp=new CurrentVersionProvider("C:\\Projects\\Testing\\Deloitte.TrueView.Database");
            Debug.WriteLine(cvp.GetVersion().ToString());
        }
    }
}