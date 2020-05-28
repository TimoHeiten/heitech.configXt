using System.Threading.Tasks;

namespace heitech.configXt.Core
{
    public interface IRunConfigOperation
    {
        Task<Result> ExecuteAsync();
    }
}