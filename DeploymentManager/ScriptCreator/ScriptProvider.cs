using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ScriptCreator
{
    public class ScriptProvider : IScriptProvider
    {
        public async Task<string[]> GetScripts(string path, Depth depth)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));
            string[] scriptList = {};
            var so = depth == Depth.OnlyParent ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories;
            await Task.Run(() => { scriptList= Directory.GetFiles(path, "*.sql", so); });
            return scriptList.Select(File.ReadAllText).ToArray();
        }
    }
}