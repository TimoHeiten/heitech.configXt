using System.Threading.Tasks;
using heitech.configXt.Core.Entities;

namespace heitech.configXt.Core.Operation
{
    public interface IConfigureQuery
    {
        Task<Result> ExecuteQueryAsync(AdministratorEntity admin, ConfigEntity entity);
    }
}