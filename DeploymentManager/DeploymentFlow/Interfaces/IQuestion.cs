using System.Threading.Tasks;

namespace DeploymentFlow
{
    public interface IQuestion
    {
        Response GetResponse();
    }
}