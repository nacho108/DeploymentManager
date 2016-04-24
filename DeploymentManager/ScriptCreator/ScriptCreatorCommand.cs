using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;

namespace ScriptCreator
{
    class ScriptCreatorCommand:ICommand
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public async Task<CommandResult> Execute()
        {
            return new CommandResult(0,"ok");
        }

        public string CommandDescription { get; }
        public string Output { get; }
    }
}
