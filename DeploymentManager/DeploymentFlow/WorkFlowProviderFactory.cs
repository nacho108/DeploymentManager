using System;
using System.Collections.Generic;
using DeploymentFlow.Annotations;
using DeploymentFlow.Commands;
using DeploymentFlow.Interfaces;
using ScriptCreator;
using SkypeMessengerProvider;

namespace DeploymentFlow
{
    public class WorkFlowProviderFactory : IWorkFlowProviderFactory
    {
        private readonly string _repositoryPath;
        private readonly string _solutionPath;
        private readonly string _databaseProjectPath;

        public WorkFlowProviderFactory([NotNull] string repositoryPath, [NotNull] string solutionPath,
            [NotNull] string databaseProjectPath)
        {
            if (repositoryPath == null) throw new ArgumentNullException(nameof(repositoryPath));
            if (solutionPath == null) throw new ArgumentNullException(nameof(solutionPath));
            if (databaseProjectPath == null) throw new ArgumentNullException(nameof(databaseProjectPath));
            _repositoryPath = repositoryPath;
            _solutionPath = solutionPath;
            _databaseProjectPath = databaseProjectPath;
        }

        public WorkFlowProvider CreateWorkFlow(string requiredVersion, int mayorVersion, int minorVersion, int build)
        {
            var stepList = new List<FlowStep>();
            stepList.Add(new FlowStep(new ShellCommand("git.exe", "-C " + _repositoryPath+ " checkout --merge develop"), "Checkout local Develop branch", 1));
            stepList.Add(new FlowStep(new ShellCommand("git.exe", "-C " + _repositoryPath + " pull --progress origin"), "Pull remote develop", 2));
            stepList.Add(new FlowStep(new ShellCommand("git.exe", "-C " + _repositoryPath + " checkout --merge Release"), "Checkout local release branch", 3));
            stepList.Add(new FlowStep(new ShellCommand("git.exe", "-C " + _repositoryPath + " pull  --progress origin"), "Pull remote release", 4));
            stepList.Add(new FlowStep(new ShellCommand("git.exe", "-C " + _repositoryPath + " merge develop"), "Merging locally develop->Release", 5));
            stepList.Add(new FlowStep(new ShellCommand("C:\\Program Files (x86)\\MSBuild\\14.0\\Bin\\MSBuild.exe",_solutionPath +" /p:Configuration=Release /verbosity:quiet"), "Buil project", 6));
            stepList.Add(new FlowStep(new StoreProceduresCreatorCommand(_databaseProjectPath, new ScriptProvider(), requiredVersion, mayorVersion, minorVersion, build, 0),
                $"Create SQL deployment script ({requiredVersion}-{mayorVersion}.{minorVersion}.{build}.0)",7));
            stepList.Add(new FlowStep(new ShellCommand(_databaseProjectPath + "\\maintenance\\reinstall-database.bat", ""),"Deploying DB locally", 8));
            //stepList.Add(new FlowStep(new ShellCommand("git.exe", "-C " + repositoryPath + " add ."), "Adding script to repo", 9));
            //stepList.Add(new FlowStep(new ShellCommand("git.exe", "-C " + repositoryPath + " commit -m \"DB deployment\""), "Commiting script", 10));
            //stepList.Add(new FlowStep(new ShellCommand("git.exe", "-C " + repositoryPath + " push"), "Pushing release to remote", 11));
            //stepList.Add(new FlowStep(new ShellCommand("git.exe", "-C " + repositoryPath + " checkout develop"), "Checkout local Develop branch", 12));
            //stepList.Add(new FlowStep(new SkypeMessage("#zigunova.olga/$1978b58b71643582", "Testing"), "Message for developers to change task state in TFS", 8));

            return new WorkFlowProvider(stepList);
        }
    }
}
