using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Contracts;
using ScriptCreator.Annotations;

namespace ScriptCreator
{
    public class StoreProceduresCreatorCommand:ICommand
    {
        private readonly string _databaseProjectPath;
        readonly IScriptProvider _scriptProvider;
        private readonly string _requiredVersion;
        private readonly int _mayorVersion;
        private readonly int _minorVersion;
        private readonly int _build;
        private readonly int _revision;
        private string _output;
        private string _header;
        private string _footer;

        public StoreProceduresCreatorCommand(string databaseProjectPath, [NotNull] IScriptProvider scriptProvider,
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
            LoadTemplatesFromDisk();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void LoadTemplatesFromDisk()
        {
            _header=  File.ReadAllText("Templates\\TV-UpdateTemplate-Header.sql");
            _footer = File.ReadAllText("Templates\\TV-UpdateTemplate-Footer.sql");
        }

        private string GetHeaderWithVersion(string requiredVersion, int mayorVersion, int minorVersion, int build)
        {
            string filledHeader= _header.Replace("{REQUIRED VERSION}", _requiredVersion);
            filledHeader = filledHeader.Replace("{NEW MAJOR VERSION}", _mayorVersion.ToString());
            filledHeader = filledHeader.Replace("{NEW MINOR VERSION}", _minorVersion.ToString());
            filledHeader = filledHeader.Replace("{NEW BUILD VERSION}", _build.ToString());
            filledHeader = filledHeader.Replace("{NEW REVISION VERSION}", _revision.ToString());

            return filledHeader;
        }

        public async Task<CommandResult> Execute()
        {
            Output += "Forming header...\n";
            var header = GetHeaderWithVersion(_requiredVersion, _mayorVersion, _minorVersion, _build);
            Output += "Getting script for deleting currents...\n";
            var deleteCurrent = File.ReadAllText("Templates\\DeleteScripts.sql");
            Output += "Getting programmability scripts...\n";
            var programScripts = await  _scriptProvider.GetScripts(_databaseProjectPath + "\\Programmability", Depth.AllChilds);
            Output += "Getting current schema change scripts...\n";
            var schemaScripts = await _scriptProvider.GetScripts(_databaseProjectPath + "\\CurrentScripts", Depth.AllChilds);
            List<ScriptContainer> progScripts=programScripts.ToList();
            List<ScriptContainer> schemasScripts = schemaScripts.ToList();

            StringBuilder sb = new StringBuilder();

            RemoveVersionControlProcedures(progScripts);
            Output += "Replacing single quotes with doubles...\n";
            ReplaceQuotesWithDoubleQuotes(progScripts);
            Output += "Wrapping everything with executeSql...\n";
            AddExecuteSql(progScripts);
            Output += "Merging all scrits in one...\n";
            string totalScript =MergeAllScriptsTogether(progScripts);
            Output += "Adding header and footer...\n";
            sb.Append(header);
            sb.Append(deleteCurrent);
            sb.Append(totalScript);
            sb.Append(_footer);
            Output += $"Total scripts processed: {progScripts.Count()}";
            File.WriteAllText("NewScripts.sql", sb.ToString());
            Debug.WriteLine(sb);
            return new CommandResult(0,"ok");
        }

        private void RemoveVersionControlProcedures(List<ScriptContainer> scripts)
        {
            var r1 = scripts.Find(o => o.Name == "pri_SystemSchemaWriteLog.sql");
            var r2 = scripts.Find(o => o.Name == "pub_SystemSchemaAddLogError.sql");
            var r3 = scripts.Find(o => o.Name == "pub_SystemSchemaAddLogInfo.sql");
            var r4 = scripts.Find(o => o.Name == "pub_SystemSchemaAddLogWarn.sql");
            var r5 = scripts.Find(o => o.Name == "pub_SystemSchemaGetMaxVersion.sql");
            var r6 = scripts.Find(o => o.Name == "pub_SystemSchemaGetVersion.sql");
            var r7 = scripts.Find(o => o.Name == "pub_SystemSchemaSetVersion.sql");
            var r8 = scripts.Find(o => o.Name == "pub_SystemSchemaIsVersionApplied.sql"); //function

            scripts.Remove(r1);
            scripts.Remove(r2);
            scripts.Remove(r3);
            scripts.Remove(r4);
            scripts.Remove(r5);
            scripts.Remove(r6);
            scripts.Remove(r7);
            scripts.Remove(r8);
        }

        private void AddExecuteSql(IEnumerable<ScriptContainer> scripts)
        {
            string header = "\nEXEC sp_executesql N'\n";
            foreach (var script in scripts)
            {
                script.ScriptBody = header + script.ScriptBody + "\n'";

            }
        }

        private string MergeAllScriptsTogether(IEnumerable<ScriptContainer> scripts)
        {
            StringBuilder sb=new StringBuilder();
            List<ScriptContainer> scl=new List<ScriptContainer>(scripts.ToArray());
            List<ScriptContainer> orderedScript = scl.OrderBy(o => o.Order).ToList();

            for (int i = 0; i < orderedScript.Count; i++)
            {
                sb.Append(orderedScript[i].ScriptBody);
            }
            return sb.ToString();
        }

        private void ReplaceQuotesWithDoubleQuotes(IEnumerable<ScriptContainer> scripts)
        {
            foreach (var scriptContainer in scripts)
            {
                scriptContainer.ScriptBody = scriptContainer.ScriptBody.Replace("'", "''");
            }
        }

        public string CommandDescription { get { return "Creating resulting scripts"; } }

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
