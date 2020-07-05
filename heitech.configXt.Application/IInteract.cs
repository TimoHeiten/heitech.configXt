using System.Threading.Tasks;
using heitech.configXt.Core;
using heitech.configXt.Models;

namespace heitech.configXt
{
    public interface IInteract
    {
        Task<OperationResult> Run(ContextModel model);
        Task<OperationResult> RunNoAuth(ContextModel model);

        Task<OperationResult> Upload(string fileItems, string indicator);
        Task<OperationResult> DownloadAs(string indicator);
    }
}