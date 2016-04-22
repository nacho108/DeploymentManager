using System.ComponentModel;
using System.Threading.Tasks;

namespace Contracts
{
    public interface ICommand: INotifyPropertyChanged
    {
        Task<CommandResult> Execute();
        string CommandDescription { get; }
        string Output { get; }
    }
}