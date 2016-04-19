using System.Threading.Tasks;

namespace DeploymentFlow
{
    public interface ICommand
    {
        Task<CommandResult> Execute();
        string Output { get; }
    }
}