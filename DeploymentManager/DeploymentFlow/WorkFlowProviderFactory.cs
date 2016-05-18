using System.Collections.Generic;
using DeploymentFlow.Commands;
using DeploymentFlow.Interfaces;
using ScriptCreator;
using SkypeMessengerProvider;

namespace DeploymentFlow
{
    public class WorkFlowProviderFactory : IWorkFlowProviderFactory
    {
        public WorkFlowProvider CreateWorkFlow()
        {
            string repositoryPath = "C:\\Projects\\testing";
            string solutionPath = "C:\\Projects\\testing\\ConsoleApplication1.sln";
            string databaseProjectPath = "C:\\Projects\\Testing\\Database";
            var stepList = new List<FlowStep>();
            stepList.Add(new FlowStep(new ShellCommand("git.exe", "-C " + repositoryPath+" checkout develop"), "Checkout local Develop branch", 1));
            stepList.Add(new FlowStep(new ShellCommand("git.exe", "-C " + repositoryPath + " pull"), "Pull remote develop", 2));
            stepList.Add(new FlowStep(new ShellCommand("git.exe", "-C " + repositoryPath + " checkout release"), "Checkout local release branch", 3));
            stepList.Add(new FlowStep(new ShellCommand("git.exe", "-C " + repositoryPath + " pull"), "Pull remote release", 4));
            stepList.Add(new FlowStep(new ShellCommand("git.exe", "-C " + repositoryPath + " merge develop"), "Merging locally develop->Release", 5));
            stepList.Add(new FlowStep(new ShellCommand("C:\\Program Files (x86)\\MSBuild\\14.0\\Bin\\MSBuild.exe",solutionPath +" /p:Configuration=Release /verbosity:quiet"), "Buil project", 6));
            stepList.Add(new FlowStep(new StoreProceduresCreatorCommand(databaseProjectPath, new ScriptProvider(), "1.0.0.0", 5, 6, 7, 0),"Create SQL deployment script",9));
            stepList.Add(new FlowStep(new ShellCommand("git.exe", "-C " + repositoryPath + " push"), "Pushing release to remote", 7));
            stepList.Add(new FlowStep(new ShellCommand("git.exe", "-C " + repositoryPath + " checkout develop"), "Checkout local Develop branch", 8));

            //stepList.Add(new FlowStep(new SkypeMessage("#zigunova.olga/$1978b58b71643582", "Testing"), "Message for developers to change task state in TFS", 8));

            return new WorkFlowProvider(stepList);
        }
    }
}
