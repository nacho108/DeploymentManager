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
            var currentVersionWriter=new CurrentVersionWriter(_databaseProjectPath);
            var currentVersionProvider=new CurrentVersionProvider(_databaseProjectPath);
            var storeProceduresCreatorCommand=new StoreProceduresCreatorCommand(_databaseProjectPath, new ScriptProvider(), requiredVersion, mayorVersion, minorVersion, build, 0, currentVersionWriter);
            var stepList = new List<FlowStep>();
            stepList.Add(new FlowStep(new ShellCommand("git.exe", "-C " + _repositoryPath+ " checkout develop",null), "Checkout local Develop branch", 1));
            stepList.Add(new FlowStep(new ShellCommand("git.exe", "-C " + _repositoryPath + " pull --progress origin",null), "Pull remote develop", 2));
            stepList.Add(new FlowStep(new ShellCommand("git.exe", "-C " + _repositoryPath + " checkout --merge Release",null), "Checkout local release branch", 3));
            stepList.Add(new FlowStep(new ShellCommand("git.exe", "-C " + _repositoryPath + " pull  --progress origin",null), "Pull remote release", 4));
            stepList.Add(new FlowStep(new ShellCommand("git.exe", "-C " + _repositoryPath + " merge develop",null), "Merging locally develop->Release", 5));
            stepList.Add(new FlowStep(new ShellCommand("C:\\Program Files (x86)\\MSBuild\\14.0\\Bin\\MSBuild.exe",_solutionPath +" /p:Configuration=Release /verbosity:quiet", null), "Build project", 6));
            stepList.Add(new FlowStep(storeProceduresCreatorCommand,$"Create SQL deployment script ({requiredVersion}-{mayorVersion}.{minorVersion}.{build}.0)",7));
            //stepList.Add(new FlowStep(new ShellCommand("C:\\Program Files (x86)\\Microsoft Visual Studio 14.0\\Common7\\IDE\\CommonExtensions\\Microsoft\\TestWindow\\vstest.console.exe", "Deloitte.TrueView.Entities.Tests.dll", "C:\\Projects\\TrueView\\Deloitte.TrueView.Entities.Tests\\bin\\Debug"), "Unit testing Testing", 8));
            stepList.Add(new FlowStep(new TestRunnerCommand(_repositoryPath+"\\Tests"), "Run unit tests", 9));

            //stepList.Add(new FlowStep(new ShellCommand(_databaseProjectPath+"\\maintenance\\reinstall-database.bat", "", _databaseProjectPath+"\\maintenance"),"Deploying DB locally", 8));
            stepList.Add(new FlowStep(new BackupSchemaScriptsCommand(_databaseProjectPath,currentVersionProvider),"Backup current schema scripts", 10));
            stepList.Add(new FlowStep(new ShellCommand("cmd.exe","/c gulp prod-site", _repositoryPath), "Minimize project", 11));

            //stepList.Add(new FlowStep(new ShellCommand("git.exe", "-C " + _repositoryPath + " add .",null), "Adding script to repo", 9));
            //stepList.Add(new FlowStep(new ShellCommand("git.exe", "-C " + _repositoryPath + " commit -m \"DB deployment\""), "Commiting script", 10));
            //stepList.Add(new FlowStep(new ShellCommand("git.exe", "-C " + repositoryPath + " push"), "Pushing release to remote", 11));
            //stepList.Add(new FlowStep(new ShellCommand("git.exe", "-C " + repositoryPath + " checkout develop"), "Checkout local Develop branch", 12));
            //stepList.Add(new FlowStep(new SkypeMessage("#zigunova.olga/$1978b58b71643582", "Testing"), "Message for developers to change task state in TFS", 15));

            return new WorkFlowProvider(stepList);
        }

    }
}
