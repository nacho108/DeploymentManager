using System;
using ScriptCreator.Annotations;

namespace ScriptCreator
{
    class CurrentVersionProvider : ICurrentVersionProvider
    {
        private readonly string _databaseProjectPath;

        public CurrentVersionProvider([NotNull] string databaseProjectPath)
        {
            if (databaseProjectPath == null) throw new ArgumentNullException(nameof(databaseProjectPath));
            _databaseProjectPath = databaseProjectPath;
        }

        public CurrentVersion GetVersion()
        {
            return new CurrentVersion() {Mayor = 1, Minor = 1, Build = 25};
        }
    }
}