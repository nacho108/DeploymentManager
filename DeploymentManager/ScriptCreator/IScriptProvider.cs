using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScriptCreator
{
    public interface IScriptProvider
    {
        Task<IEnumerable<ScriptContainer>> GetScripts(string path, Depth depth);
    }
}