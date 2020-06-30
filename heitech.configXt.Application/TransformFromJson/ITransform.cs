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
        ///<summary>
        /// Parses json string to a ConfigurationCollection
        ///</summary>
        OperationResult Parse(string inputString);
        ///<summary>
        /// Format an Enumerable to a string
        ///</summary>
        OperationResult Format(IEnumerable<ConfigEntity> entities);
    }
}