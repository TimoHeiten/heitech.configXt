using heitech.configXt.Core;

namespace heitech.configXt.Application
{
    ///<summary>
    /// Encapsulates Transformation
    ///</summary>
    public interface ITransform
    {
        OperationResult Transform(string inputString);
    }
}