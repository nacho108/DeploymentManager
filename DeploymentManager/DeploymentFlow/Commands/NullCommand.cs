
using System.Diagnostics;
using System.Threading.Tasks;

namespace DeploymentFlow
{
    public class NullCommand : ICommand
    {
        private readonly string _name;

        public NullCommand(string name)
        {
            _name = name;
        }

        public async Task<string> Execute()
        {
            await Task.Delay(200);
            return "Ok";
        }
    }
}