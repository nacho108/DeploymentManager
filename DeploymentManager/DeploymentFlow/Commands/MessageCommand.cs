using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DeploymentFlow.Annotations;
using DeploymentFlow.Interfaces;

namespace DeploymentFlow.Commands
{
    public class MessageCommand : ICommand
    {
        private readonly string _destination;
        private readonly string _message;
        private readonly IMessageSender _messageSender;

        public MessageCommand([NotNull] string destination, [NotNull] string message)
        {
            if (destination == null) throw new ArgumentNullException(nameof(destination));
            if (message == null) throw new ArgumentNullException(nameof(message));
            _destination = destination;
            _message = message;
        }

        public MessageCommand([NotNull] IMessageSender messageSender)
        {
            if (messageSender == null) throw new ArgumentNullException(nameof(messageSender));
            _messageSender = messageSender;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public async Task<CommandResult> Execute()
        {
            await _messageSender.SendMessageAsync()
        }

        public string CommandDescription { get; private set; }
        public string Output { get; private set; }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }