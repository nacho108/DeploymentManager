using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Contracts;
using DeploymentFlow.Annotations;
using ScriptCreator;

namespace DeploymentFlow
{
    public class BackupSchemaScriptsCommand : ICommand
    {
        private readonly string _databaseProjectPath;
        private readonly CurrentVersionProvider _currentVersionProvider;
        private string _output;

        public string Output
        {
            get { return _output; }
            set
            {
                if (value == _output) return;
                _output = value;
                OnPropertyChanged();
            }
        }

        public BackupSchemaScriptsCommand([NotNull] string databaseProjectPath, [NotNull] CurrentVersionProvider currentVersionProvider)
        {
            if (databaseProjectPath == null) throw new ArgumentNullException(nameof(databaseProjectPath));
            if (currentVersionProvider == null) throw new ArgumentNullException(nameof(currentVersionProvider));
            _databaseProjectPath = databaseProjectPath;
            _currentVersionProvider = currentVersionProvider;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public async Task<CommandResult> Execute()
        {
            int exitCode = 0;
            await Task.Run(() =>
            {
                Output = Output + "Getting current version...\n";
                var version = _currentVersionProvider.GetVersion();
                Output = Output + "Creating new backup folder...\n";
                string sourceFolder = _databaseProjectPath + "\\SchemaChangeScriptsCurrent";
                string backupFolder = _databaseProjectPath +$"\\SchemaChangeScriptsBackup\\{version.Mayor}.{version.Minor}.{version.Build}";
                Directory.CreateDirectory(backupFolder);
                Output = Output + "Moving files...\n";
                string[] files = Directory.GetFiles(sourceFolder, "*.*");
                foreach (string file in files)
                {
                    FileInfo fileInfo = new FileInfo(file);
                    File.Copy(file, backupFolder+"\\" + fileInfo.Name);
                    File.Delete(file);
                }
            });
            return new CommandResult(exitCode, _output);
        }

        public string CommandDescription => "Backup current schema scripts.";

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}