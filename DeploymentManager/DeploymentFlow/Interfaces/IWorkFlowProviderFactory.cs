namespace DeploymentFlow.Interfaces
{
    public interface IWorkFlowProviderFactory
    {
        WorkFlowProvider CreateWorkFlow(string requiredVersion, int mayorVersion, int minorVersion, int build);
    }
}