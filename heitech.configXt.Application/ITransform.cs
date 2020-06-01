using System.Threading.Tasks;
using heitech.configXt.Core.Entities;

namespace heitech.configXt.Application
{
    public interface ITransform
    {
        Task<ConfigEntity> TransformInput(string inputValues);
    }
}