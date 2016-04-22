using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Contracts;
using SKYPE4COMLib;
using ICommand = Contracts.ICommand;

namespace SkypeMessengerProvider
{
    public class SkypeMessage : ICommand
    {
        private readonly string _destination;
        private readonly string _message;

        public SkypeMessage(string destination, string message)
        {
            _destination = destination;
            _message = message;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public async Task<CommandResult> Execute()
        {
            ChatMessage cm=null;
            try
            {
                await Task.Run(() =>
                {
                    Skype skype = new Skype();
                    skype.Attach(5, true);
                    cm=skype.Chat[_destination].SendMessage(_message);
                });
            }
            catch (Exception ex)
            {
                return new CommandResult(1, ex.ToString());
            }
            return new CommandResult(0,$"Ok \n {cm}");
        }

        public string CommandDescription => $"Message to group {_destination}";
        public string Output { get { return "ok"; } }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}