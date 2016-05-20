using System;
using System.IO;
using Newtonsoft.Json;
using ScriptCreator.Annotations;

namespace ScriptCreator
{
    public class CurrentVersionWriter : ICurrentVersionWriter
    {
        private readonly string _databaseProjectPath;

        public CurrentVersionWriter([NotNull] string databaseProjectPath)
        {
            if (databaseProjectPath == null) throw new ArgumentNullException(nameof(databaseProjectPath));
            _databaseProjectPath = databaseProjectPath;
        }

        public void WriteCurrentVersion(CurrentVersion currentVersion)
        {
            using (StreamWriter file = File.CreateText(_databaseProjectPath + "\\Updates\\DBCurrentVersion.txt"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, currentVersion);
            }
        }
    }
}