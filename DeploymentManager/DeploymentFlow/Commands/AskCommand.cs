using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using DeploymentFlow.Annotations;

namespace DeploymentFlow
{
    public class AskCommand : ICommand
    {
        private readonly IQuestion _question;

        public AskCommand(IQuestion question)
        {
            _question = question;
        }

        public async Task<CommandResult> Execute()
        {
            var a=_question.GetResponse();
            switch (a)
            {
                case Response.Ok:
                    Output = "Ok";
                    return new CommandResult(0,"Ok");
                case Response.Cancel:
                    Output = "Cancel";
                    return new CommandResult(1, "Cancelled");
            }
            return new CommandResult(0, "Cancelled");
        }

        public string Output { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}