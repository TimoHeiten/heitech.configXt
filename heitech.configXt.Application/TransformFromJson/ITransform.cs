using heitech.configXt.Core;

namespace heitech.configXt.Application
{
    public interface ITransform
    {
        OperationResult Transform(string inputString);
    }
}