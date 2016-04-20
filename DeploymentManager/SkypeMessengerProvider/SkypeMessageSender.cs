using System.Threading.Tasks;
using DeploymentFlow.Interfaces;
using SKYPE4COMLib;

namespace SkypeMessengerProvider
{
    class SkypeMessageSender : IMessageSender
    {
        public async Task<string> SendMessageAsync(string destination, string message)
        {
            await Task.Run(() =>
            {
                ISkype skype = new Skype();
                skype.Attach(5, true);
                skype.Chat[destination].SendMessage(message);
            });
            return "";
        }
    }
}