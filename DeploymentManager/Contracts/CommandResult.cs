namespace Contracts
{
    public class CommandResult
    {
        public CommandResult(int exitCode, string output)
        {
            ExitCode = exitCode;
            Output = output;
        }

        public int ExitCode { get; }
        public string Output { get; }

        public override string ToString()
        {
            return ExitCode+"-"+ Output;
        }
    }
}