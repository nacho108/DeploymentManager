using System.ComponentModel;
using System.Threading.Tasks;

namespace DeploymentFlow
{
    public interface ICommand: INotifyPropertyChanged
    {
        Task<CommandResult> Execute();
        string Description { get; }
        string Output { get; }
    }
}