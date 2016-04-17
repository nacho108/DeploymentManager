using System.Threading.Tasks;
using System.Windows;
using DeploymentFlow;

namespace DeploymentManager
{
    public class WpfQuestion : IQuestion
    {
        public Response GetResponse(string question)
        {
            var a = MessageBox.Show(question, "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            switch (a)
            {
                case MessageBoxResult.Cancel:
                    return Response.Cancel;
                case MessageBoxResult.OK:
                    return Response.Ok;
            }
            return Response.Cancel;
        }
    }
}