using System.Collections.Generic;
using heitech.configXt.Core;
using heitech.configXt.Core.Entities;

namespace heitech.configXt.Application
{
    ///<summary>
    /// Encapsulates Transformation
    ///</summary>
    public interface ITransform
    {
        OperationResult Parse(string inputString);
        OperationResult Format(IEnumerable<ConfigEntity> entities);
    }
}