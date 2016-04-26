using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
    public class ScriptCreatorCommand:ICommand
    {
        private readonly string _databaseProjectPath;
        readonly IScriptProvider _scriptProvider;
        private readonly string _requiredVersion;
        private readonly int _mayorVersion;
        private readonly int _minorVersion;
        private readonly int _build;
        private readonly int _revision;
        private string _output;

        public ScriptCreatorCommand(string databaseProjectPath, [NotNull] IScriptProvider scriptProvider,
            string requiredVersion,int newMayorVersion, int newMinorVersion, int newBuild, int newRevision)
        {
            if (newMayorVersion < 1) throw new ArgumentException("newMayorVersion version should be >0");
            if (newMinorVersion < 0) throw new ArgumentException("newMinorVersion version should be >=0");
            if (newBuild < 0) throw new ArgumentException("newBuild should be >=0");
            if (newRevision < 0) throw new ArgumentException("newRevision should be >=0");
            if (scriptProvider == null) throw new ArgumentNullException(nameof(scriptProvider));

            _databaseProjectPath = databaseProjectPath;
            _scriptProvider = scriptProvider;
            _requiredVersion = requiredVersion;
            _mayorVersion = newMayorVersion;
            _minorVersion = newMinorVersion;
            _build = newBuild;
            _revision = newRevision;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public async Task<CommandResult> Execute()
        {
            Output += "Forming header...\n";
            var header = File.ReadAllText("Templates\\TV-UpdateTemplate-Header.sql");
            header = header.Replace("{REQUIRED VERSION}", _requiredVersion);
            header = header.Replace("{NEW MAJOR VERSION}", _mayorVersion.ToString());
            header = header.Replace("{NEW MINOR VERSION}", _minorVersion.ToString());
            header = header.Replace("{NEW BUILD VERSION}", _build.ToString());
            header = header.Replace("{NEW REVISION VERSION}", _revision.ToString());
            var footer = File.ReadAllText("Templates\\TV-UpdateTemplate-Footer.sql");
            Output += "Getting scripts...\n";
            var scripts = await  _scriptProvider.GetScripts(_databaseProjectPath + "", Depth.AllChilds);
            Output += "Putting functions in the beginning...\n";
            PutFunctionsInTheBeginning(scripts);
            Output += "Replacing single quotes with doubles...\n";
            ReplaceQuotesWithDoubleQuotes(scripts);
            Output += "Wrapping everything with executeSql...\n";
            AddExecuteSql(scripts);
            Output += "Merging all scrits in one...\n";
            string totalScript =MergeAllScriptsTogether(scripts);
            StringBuilder sb = new StringBuilder();
            Output += "Adding header and footer...\n";
            sb.Append(header);
            sb.Append(totalScript);
            sb.Append(footer);
            Output += $"Total scripts processed: {scripts.Length}";
            return new CommandResult(0,"ok");
        }

        private void PutFunctionsInTheBeginning(string[] scripts)
        {
            int loops = 0;
            bool somethingWentWrong=false;
            bool functionSwapped = false;
            int lastScriptSwapped = 0;
            do
            {
                for (int i = 0; i < scripts.Length; i++)
                {
                    if (scripts[i].IndexOf("CREATE FUNCTION", StringComparison.OrdinalIgnoreCase) != -1)
                    {
                        Output += $"function swapped index: {i}";
                        SwapScripts(scripts, i, lastScriptSwapped);
                        lastScriptSwapped++;
                        functionSwapped = true;
                    }
                }
                loops++;
                if (loops > scripts.Length)
                {
                    somethingWentWrong = true;
                }
            } while (functionSwapped && !somethingWentWrong);
            if (somethingWentWrong) throw new OperationCanceledException("There were too many ocurrences of function swaping. Probably you should check this algorithm. Good luck!");
        }

        private void SwapScripts(string[] scripts, int origin, int destination)
        {
            string auxstringContainer = scripts[origin];
            scripts[origin] = scripts[destination];
            scripts[destination] = auxstringContainer;
        }


        private void AddExecuteSql(string[] scripts)
        {
            string header = "\nEXEC sp_executesql N'\n";
            for (int i = 0; i < scripts.Length; i++)
            {

                scripts[i] = header + scripts[i]+"\n'";
            }
        }

        private string MergeAllScriptsTogether(string[] scripts)
        {
            StringBuilder sb=new StringBuilder();
            for (int i = 0; i < scripts.Length; i++)
            {
                sb.Append(scripts[i]);
            }
            return sb.ToString();
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
