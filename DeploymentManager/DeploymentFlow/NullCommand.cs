using System;
using System.Threading.Tasks;

namespace DeploymentFlow
{
    public class NullCommand : ICommand
    {
        async public Task Execute()
        {
            Console.WriteLine("Step executed.");
            await Task.Delay(1);
        }
    }
}