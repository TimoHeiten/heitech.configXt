using System.Threading.Tasks;
using heitech.configXt.Core.Entities;

namespace heitech.configXt.Core.Operation
{
    public interface IConfigureCommand
    {
        Task<Result> ExecuteCommandAsync(AdministratorEntity administrator, ConfigEntity entity);
    }
}