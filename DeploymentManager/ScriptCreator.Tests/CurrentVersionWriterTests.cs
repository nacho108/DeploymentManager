using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ScriptCreator.Tests
{
    [TestClass]
    public class CurrentVersionWriterTests
    {
        [TestMethod]
        public void WriteCurrentVersion()
        {
            CurrentVersionWriter cvw = new CurrentVersionWriter("C:\\Projects\\Testing\\Deloitte.TrueView.Database");
            cvw.WriteCurrentVersion(new CurrentVersion() {Mayor = 1, Minor = 2, Build = 3});
        }
    }
}