using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ScriptCreator
{
    public class ScriptProvider : IScriptProvider
    {
        public async Task<IEnumerable<ScriptContainer>> GetScripts(string path, Depth depth)
        {
            List< ScriptContainer > scl=new List<ScriptContainer>();
            if (path == null) throw new ArgumentNullException(nameof(path));
            string[] scriptList = {};
            var so = depth == Depth.OnlyParent ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories;
            await Task.Run(() => { scriptList= Directory.GetFiles(path, "*.sql", so); });
            Array.Sort(scriptList);
            for (int i = 0; i < scriptList.Length; i++)
            {
                var body = File.ReadAllText(scriptList[i]);
                var scriptType = GetScriptType(body);
                int defaultOrder=100;
                switch (scriptType)
                {
                    case ScriptType.CustomType:
                        defaultOrder = 300;
                        break;
                    case ScriptType.Function:
                        defaultOrder = 700;
                        break;
                    case ScriptType.View:
                        defaultOrder = 1000;
                        break;
                    case ScriptType.StoreProcedure:
                        defaultOrder = 1200;
                        break;
                }
                scl.Add(new ScriptContainer()
                {
                    ScriptBody= body,
                    Order= GetOrderFromScript(body, defaultOrder),
                    Name= Path.GetFileName(scriptList[i]),
                    ScriptType = scriptType
                });
            }
            return scl;
        }

        private int GetOrderFromScript(string scriptBody, int defaultValue)
        {
            int order = defaultValue;
            var indexOrderKey = scriptBody.IndexOf("--Order=", 0, StringComparison.OrdinalIgnoreCase);
            if (indexOrderKey == -1) return order;
            var indexOrderValue = indexOrderKey + 8;
            var indexEndOfLine = scriptBody.IndexOf(";", indexOrderKey, StringComparison.OrdinalIgnoreCase);
            if (indexEndOfLine == -1) return order;
            var parameterLength = indexEndOfLine - indexOrderValue;
            string valueString = scriptBody.Substring(indexOrderValue, parameterLength);
            int.TryParse(valueString, out order);
            return order;
        }

        private ScriptType GetScriptType(string scriptBody)
        {
            var indexFunction = scriptBody.IndexOf("CREATE FUNCTION", 0, StringComparison.OrdinalIgnoreCase);
            var indexType = scriptBody.IndexOf("CREATE TYPE", 0, StringComparison.OrdinalIgnoreCase);
            var indexView = scriptBody.IndexOf("CREATE VIEW", 0, StringComparison.OrdinalIgnoreCase);
            if (indexFunction >= 0 ) return ScriptType.Function;
            if (indexType >= 0) return ScriptType.CustomType;
            if (indexView >= 0) return ScriptType.View;
            return ScriptType.StoreProcedure;
        }
    }
}