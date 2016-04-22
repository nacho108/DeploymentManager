using System.Collections.Generic;
using DeploymentFlow.Commands;
using DeploymentFlow.Interfaces;
using SkypeMessengerProvider;

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
            stepList.Add(new FlowStep(new ShellCommand("C:\\Program Files (x86)\\MSBuild\\14.0\\Bin\\MSBuild.exe",
                "c:\\projects\\testing\\ConsoleApplication1.sln /p:Configuration=Release /verbosity:quiet"), "Buil project", 6));
            stepList.Add(new FlowStep(new ShellCommand("git.exe", "-C C:\\Projects\\testing push"), "Pushing release to remote", 7));
            stepList.Add(new FlowStep(new ShellCommand("git.exe", "-C C:\\Projects\\testing checkout develop"), "Checkout local Develop branch", 7));
            //stepList.Add(new FlowStep(new SkypeMessage("#zigunova.olga/$1978b58b71643582", "Testing"), "Message for developers to change task state in TFS", 8));

            return new WorkFlowProvider(stepList);
        }
    }
}
