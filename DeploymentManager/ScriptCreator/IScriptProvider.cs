using System.Threading.Tasks;

namespace ScriptCreator
{
    public interface IScriptProvider
    {
        Task<string[]> GetScripts(string path, Depth depth);
    }
}