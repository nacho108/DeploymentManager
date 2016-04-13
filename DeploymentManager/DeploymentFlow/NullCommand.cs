
using System.Diagnostics;
using System.Threading.Tasks;

namespace DeploymentFlow
{
    public class NullCommand : ICommand
    {
        async public Task Execute()
        {
            Debug.WriteLine("Step executed.");
            await Task.Delay(1);
        }
    }
}