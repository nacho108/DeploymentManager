
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
            Debug.WriteLine(string.Format("{0} executed.", _name));
            await Task.Delay(1000);
            return "Ok";
        }
    }
}