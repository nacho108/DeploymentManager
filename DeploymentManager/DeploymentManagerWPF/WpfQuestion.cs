using System.Threading.Tasks;
using System.Windows;
using DeploymentFlow;

namespace DeploymentManager
{
    public class WpfQuestion : IQuestion
    {
        private readonly string _questionText;

        public WpfQuestion(string questionText)
        {
            _questionText = questionText;
        }

        public Response GetResponse( )
        {
            var a = MessageBox.Show(_questionText, "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
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