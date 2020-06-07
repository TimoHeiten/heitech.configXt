using System.Threading.Tasks;
using heitech.configXt.Core;

namespace heitech.configXt.Application.UseCases
{
    public interface IUseCase
    {
        Task<OperationResult> RunUseCaseAsync();
    }
}