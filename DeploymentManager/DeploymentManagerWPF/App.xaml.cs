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

            //var step3 = new FlowStep(new AskCommand(new WpfQuestion("https://www.google.com.ua/?gfe_rd=cr&ei=9GMUV5-jC86BtAHIm4_4Aw&gws_rd=ssl ")), "Third step", 2);

            stepList.Add(new FlowStep(new NullCommand("first"), "something", 0));
            stepList.Add(new FlowStep(new ShellCommand("cmd.exe", "/C git -C C:\\Projects\\testing checkout develop"), "Checkout local Develop branch", 0));
            stepList.Add(new FlowStep(new ShellCommand("cmd.exe", "/C git -C C:\\Projects\\testing pull"), "Pull remote", 0));

            var workFlowProvider = new WorkFlowProvider(stepList);
            MainViewModel mainviewModel = new MainViewModel(workFlowProvider);
            MainWindow mw = new MainWindow() { DataContext = mainviewModel };
            mw.Show();
        }


    }
}
