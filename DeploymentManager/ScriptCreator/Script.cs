using System;

namespace ScriptCreator
{
    public class ScriptContainer
    {
        public ScriptType ScriptType { get; set; }
        public string Name { get; set; } 
        public int Order { get; set; }
        public string ScriptBody { get; set; }
    }

    public enum ScriptType
    {
        StoreProcedure,
        Function,
        CustomType
    }
}