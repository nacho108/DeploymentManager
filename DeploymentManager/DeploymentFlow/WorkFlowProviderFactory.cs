using System.Collections.Generic;
using DeploymentFlow.Commands;
using DeploymentFlow.Interfaces;

namespace DeploymentFlow
{
    public class WorkFlowProviderFactory : IWorkFlowProviderFactory
    {
        public WorkFlowProvider CreateWorkFlow()
        {
            var stepList = new List<FlowStep>();
            stepList.Add(new FlowStep(new ShellCommand("git.exe", "-C C:\\Projects\\testing checkout develop"), "Checkout local Develop branch", 1));
            stepList.Add(new FlowStep(new ShellCommand("git.exe", "-C C:\\Projects\\testing pull"), "Pull remote develop", 2));
            stepList.Add(new FlowStep(new ShellCommand("git.exe", "-C C:\\Projects\\testing checkout release"), "Checkout local release branch", 3));
            stepList.Add(new FlowStep(new ShellCommand("git.exe", "-C C:\\Projects\\testing pull"), "Pull remote release", 4));
            stepList.Add(new FlowStep(new ShellCommand("git.exe", "-C C:\\Projects\\testing merge develop"), "Merging locally develop->Release", 5));
            stepList.Add(new FlowStep(new ShellCommand("git.exe", "-C C:\\Projects\\testing push"), "Pushing release to remote", 6));
            stepList.Add(new FlowStep(new ShellCommand("git.exe", "-C C:\\Projects\\testing checkout develop"), "Checkout local Develop branch", 7));
            stepList.Add(new FlowStep(new MessageCommand("git.exe", "-C C:\\Projects\\testing checkout develop"), "Checkout local Develop branch", 7));

            return new WorkFlowProvider(stepList);
        }
    }
}