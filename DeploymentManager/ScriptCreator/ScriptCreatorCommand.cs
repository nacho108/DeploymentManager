using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Contracts;
using ScriptCreator.Annotations;

namespace ScriptCreator
{
    class ScriptCreatorCommand:ICommand
    {
        private string[] _scriptList;
        private readonly string _databaseProjectPath;
        IScriptProvider _scriptProvider;
        private string _output;

        public ScriptCreatorCommand(string databaseProjectPath, [NotNull] IScriptProvider scriptProvider)
        {
            if (scriptProvider == null) throw new ArgumentNullException(nameof(scriptProvider));
            _databaseProjectPath = databaseProjectPath;
            _scriptProvider = scriptProvider;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public async Task<CommandResult> Execute()
        {
            var header = File.ReadAllText("TV-UpdateTemplate-Header.sql");
            var footer = File.ReadAllText("TV-UpdateTemplate-Footer.sql");
            var scripts = await  _scriptProvider.GetScripts(_databaseProjectPath + "\\Programmability", Depth.AllChilds);
            Output += $"Total script: {scripts.Length}";
            ReplaceQuotesWithDoubleQuotes(scripts);
            
            return new CommandResult(0,"ok");
        }

        private void ReplaceQuotesWithDoubleQuotes(string[] scripts)
        {
            for (int i = 0; i < scripts.Length; i++)
            {
                scripts[i] = scripts[i].Replace("'", "''");
            }
        }

        public string CommandDescription { get { return "Creating resultin scripts"; } }

        public string Output
        {
            get { return _output; }
            private set
            {
                if (value == _output) return;
                _output = value;
                OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
