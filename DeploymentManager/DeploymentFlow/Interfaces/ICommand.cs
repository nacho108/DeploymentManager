using System.Threading.Tasks;

namespace DeploymentFlow
{
    public interface ICommand
    {
        Task Execute();
    }
}