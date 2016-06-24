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
    public enum OutputFolderSelect
    {
        Auto,
        Specified
    }
    public class StoreProceduresCreatorCommand:ICommand
    {
        private readonly string _databaseProjectPath;
        private string _outputFolder;
        private readonly OutputFolderSelect _outputFolderSelect;
        readonly IScriptProvider _scriptProvider;
        private  string _requiredVersion;
        private readonly int _mayorVersion;
        private readonly int _minorVersion;
        private  int _build;
        private readonly int _revision;
        private readonly ICurrentVersionWriter _currentVersionWriter;
        private readonly int _finaBuild;
        private string _output;
        private string _header;
        private string _footer;
        private int _finalBuild;

        public StoreProceduresCreatorCommand([NotNull] string databaseProjectPath, string outputFolder, OutputFolderSelect outputFolderSelect,
            [NotNull] IScriptProvider scriptProvider,string requiredVersion,int newMayorVersion, int newMinorVersion, int newBuild,
            int newRevision,[NotNull] ICurrentVersionWriter currentVersionWriter, ref int finaBuild)
        {
            if (databaseProjectPath == null) throw new ArgumentNullException(nameof(databaseProjectPath));
            if (newMayorVersion < 1) throw new ArgumentException("newMayorVersion version should be >0");
            if (newMinorVersion < 0) throw new ArgumentException("newMinorVersion version should be >=0");
            if (newBuild < 0) throw new ArgumentException("newBuild should be >=0");
            if (newRevision < 0) throw new ArgumentException("newRevision should be >=0");
            if (scriptProvider == null) throw new ArgumentNullException(nameof(scriptProvider));
            if (currentVersionWriter == null) throw new ArgumentNullException(nameof(currentVersionWriter));
            _finalBuild = finaBuild;
            _databaseProjectPath = databaseProjectPath;
            _outputFolder = outputFolder;
            _outputFolderSelect = outputFolderSelect;
            _scriptProvider = scriptProvider;
            _requiredVersion = requiredVersion;
            _mayorVersion = newMayorVersion;
            _minorVersion = newMinorVersion;
            _build = newBuild;
            _revision = newRevision;
            _currentVersionWriter = currentVersionWriter;
            _finaBuild = finaBuild;
            LoadTemplatesFromDisk();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void LoadTemplatesFromDisk()
        {
            _header=  File.ReadAllText("Templates\\TV-UpdateTemplate-Header.sql");
            _footer = File.ReadAllText("Templates\\TV-UpdateTemplate-Footer.sql");
        }

        private string GetHeaderWithVersion()
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
            // Schema changes and data migration
            StringBuilder sb = new StringBuilder();
            Output += "Forming header...\n";
            var header = GetHeaderWithVersion();
            Output += "Getting current schema change scripts...\n";
            var schemaScripts = await _scriptProvider.GetScripts(_databaseProjectPath + "\\SchemaChangeScriptsCurrent", Depth.AllChilds);
            List<ScriptContainer> schemasScripts = schemaScripts.OrderBy(o => o.Name).ToList();
            if (schemasScripts.Count > 0)
            {
                sb.Append("/*\n");
                sb.Append("******************************************************************************************** \n");
                sb.Append("*        SCHEMA CHANGES & DATA MIGRATION                                                   * \n");
                sb.Append("******************************************************************************************** \n");
                sb.Append("Scripts found: " + schemasScripts.Count + "\n\n");
                sb.Append("*/\n");
                sb.Append("PRINT '***** Deploying schema changes/data migrations...' \n\n");

                foreach (var scriptContainer in schemasScripts)
                {
                    header = GetHeaderWithVersion();
                    sb.Append(header);
                    sb.Append(scriptContainer.ScriptBody);
                    sb.Append(_footer);
                    sb.Append("\n/* *************************************************************************** */ \n\n");
                    _requiredVersion = _mayorVersion + "." + _minorVersion + "." + _build + ".0";
                    _build++;
                }
            }
            else
            {
                sb.Append("/* *********************   No schema changes or data migration found. */ \n ");
            }

            Output += "Getting programmability scripts...\n";
            var programScripts = await _scriptProvider.GetScripts(_databaseProjectPath + "\\Programmability", Depth.AllChilds);
            List<ScriptContainer> progScripts = programScripts.ToList();
            sb.Append("/*\n");
            sb.Append("**************************************************************************************** \n");
            sb.Append("*        STORE PROCEDURES/TYPES/FUNCTIONS                                              * \n");
            sb.Append("**************************************************************************************** \n");
            sb.Append("Scripts found: " + progScripts.Count + "\n\n");
            sb.Append("*/\n");
            sb.Append("PRINT '***** Deploying Store procedures, types and functions...' \n\n");
            
            // Preparing store procedures
            Output += "Getting script for deleting currents...\n";
            var deleteCurrent = File.ReadAllText("Templates\\DeleteScripts.sql");
            RemoveVersionControlProcedures(progScripts);
            Output += "Replacing single quotes with doubles...\n";
            ReplaceQuotesWithDoubleQuotes(progScripts);
            Output += "Wrapping everything with executeSql...\n";
            AddExecuteSql(progScripts);
            Output += "Merging all scrits in one...\n";
            string totalScript =MergeAllScriptsTogether(progScripts);
            Output += "Adding header and footer...\n";
            sb.Append(GetHeaderWithVersion());
            sb.Append("\n--******* Delete all procedures/functions and custom types \n\n");
            sb.Append(deleteCurrent);
            sb.Append("\n--******* Re-deploy all procedures/functions and custom types \n\n");
            sb.Append(totalScript);
            sb.Append(_footer);
            Output += $"Total scripts processed: {progScripts.Count()}";
            string lastVersion = _mayorVersion + "." + _minorVersion.ToString("00") + "." + _build.ToString("000");
            if (_outputFolderSelect==OutputFolderSelect.Auto)
            {
                _outputFolder = $"\\Updates\\Release {_mayorVersion}.{_minorVersion}\\";
            }

            File.WriteAllText(_outputFolder + "\\TV-"+ lastVersion+".000.sql", sb.ToString());
            _currentVersionWriter.WriteCurrentVersion(new CurrentVersion() {Mayor = _mayorVersion, Minor = _minorVersion, Build = _build });
            _finalBuild = _build;
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
                sb.Append("\n /* *************************************************************************** */ \n\n");

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
