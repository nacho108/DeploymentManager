using ScriptCreator;

namespace ScriptGenerator
{
    public class NullCurrentVersionWriter:ICurrentVersionWriter
    {
        public void WriteCurrentVersion(CurrentVersion currentVersion)
        {
            // does nothing
        }
    }
}