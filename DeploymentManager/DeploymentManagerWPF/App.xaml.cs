using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using DeploymentFlow;
using DeploymentFlow.Commands;

namespace DeploymentManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var stepList = new List<FlowStep>();
            stepList.Add(new FlowStep(new ShellCommand("git.exe", "-C C:\\Projects\\testing checkout develop"), "Checkout local Develop branch", 1));
            stepList.Add(new FlowStep(new ShellCommand("git.exe", "-C C:\\Projects\\testing pull"), "Pull remote develop", 2));
            stepList.Add(new FlowStep(new ShellCommand("git.exe", "-C C:\\Projects\\testing checkout release"), "Checkout local release branch", 3));
            stepList.Add(new FlowStep(new ShellCommand("git.exe", "-C C:\\Projects\\testing pull"), "Pull remote release", 4));
            stepList.Add(new FlowStep(new ShellCommand("git.exe", "-C C:\\Projects\\testing merge develop"), "Merging locally develop->Release", 5));

            var workFlowProvider = new WorkFlowProvider(stepList);
            MainViewModel mainviewModel = new MainViewModel(workFlowProvider);
            MainWindow mw = new MainWindow() { DataContext = mainviewModel };
            mw.Show();
        }


    }
}
