using System.Threading.Tasks;

namespace DeploymentFlow.Interfaces
{
    public interface IMessageSender
    {
        Task<string> SendMessageAsync(string destination, string message);
    }
}