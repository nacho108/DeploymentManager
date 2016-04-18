using System;
using System.Threading.Tasks;
using System.Windows;

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
                    return new CommandResult(0,"Ok");
                case Response.Cancel:
                    return new CommandResult(1, "Cancelled");
            }
            return new CommandResult(0, "Cancelled");
        }
    }
}