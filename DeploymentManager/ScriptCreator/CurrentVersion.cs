namespace ScriptCreator
{
    public class CurrentVersion
    {
        public int Mayor { get; set; }
        public int Minor { get; set; }
        public int Build { get; set; }
        public override string ToString()
        {
            return Mayor+"."+Minor+"."+Build;
        }
    }
}