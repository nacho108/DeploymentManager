﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using Contracts;
using DeploymentFlow.Annotations;
using DeploymentFlow.Interfaces;

namespace DeploymentFlow
{
    public class AskCommand : ICommand
    {
        private readonly IQuestion _question;

        public AskCommand(IQuestion question)
        {
            _question = question;
        }

        public Task<CommandResult> Execute()
        {
            var a=_question.GetResponse();
            switch (a)
            {
                case Response.Ok:
                    Output = "Ok";
                    return Task.Run(() =>new CommandResult(0, "Ok"));
                case Response.Cancel:
                    Output = "Cancel";
                    return Task.Run(() => new CommandResult(1, "Cancelled"));
            }
            return Task.Run(() => new CommandResult(0, "Cancelled"));
        }

        public string CommandDescription => "Question";

        public string Output { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}