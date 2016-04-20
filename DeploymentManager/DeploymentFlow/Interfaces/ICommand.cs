using System.ComponentModel;
using System.Threading.Tasks;

namespace DeploymentFlow.Interfaces
{
    public interface ICommand: INotifyPropertyChanged
    {
        Task<CommandResult> Execute();
        string CommandDescription { get; }
        string Output { get; }
    }
}