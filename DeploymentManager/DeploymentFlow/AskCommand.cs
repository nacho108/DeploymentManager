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

        public Task<string> Execute()
        {
            var a=_question.GetResponse("");
            switch (a)
            {
                case Response.Ok:
                    return "ok";
                case Response.Cancel:
                    return "Cancelled";
            }
            return "Cancelled";
        }
    }
}