using System;
using System.IO;
using Newtonsoft.Json;
using ScriptCreator.Annotations;

namespace ScriptCreator
{
    public class CurrentVersionProvider : ICurrentVersionProvider
    {
        private readonly string _databaseProjectPath;

        public CurrentVersionProvider([NotNull] string databaseProjectPath)
        {
            if (databaseProjectPath == null) throw new ArgumentNullException(nameof(databaseProjectPath));
            _databaseProjectPath = databaseProjectPath;
        }

        public CurrentVersion GetVersion()
        {

            using (StreamReader file = File.OpenText(_databaseProjectPath+"\\Updates\\DBCurrentVersion.txt"))
            {
                JsonSerializer serializer = new JsonSerializer();
                CurrentVersion currentVersion = (CurrentVersion) serializer.Deserialize(file, typeof (CurrentVersion));
                return  currentVersion;
            }
        }
    }
}