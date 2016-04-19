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
            Directory.SetCurrentDirectory(@"C:\Projects\Trueview");
            var step2 = new FlowStep(new ShellCommand("git.exe", "checkout develop"), "Checkout Develop branch", 0);
            //var step3 = new FlowStep(new AskCommand(new WpfQuestion("https://www.google.com.ua/?gfe_rd=cr&ei=9GMUV5-jC86BtAHIm4_4Aw&gws_rd=ssl ")), "Third step", 2);

            stepList.Add(step2);
            var workFlowProvider = new WorkFlowProvider(stepList);
            MainViewModel mainviewModel = new MainViewModel(workFlowProvider);
            MainWindow mw = new MainWindow() { DataContext = mainviewModel };
            mw.Show();
        }


    }
}
