using System.ComponentModel;
using System.Threading.Tasks;
using Contracts;

namespace ScriptCreator
{
    public class SchemaModifCreatorCommand : ICommand
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public Task<CommandResult> Execute()
        {
            throw new System.NotImplementedException();
        }

        public string CommandDescription { get; }
        public string Output { get; }
    }
}