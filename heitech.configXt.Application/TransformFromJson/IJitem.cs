using System.Collections.Generic;
using heitech.configXt.Core.Entities;

namespace heitech.configXt.Application
{
    ///<summary>
    /// Interface for the Reverse Construction
    ///</summary>
    public interface IJitem
    {
        string Name { get; }
        IJitem Parent { get; }
        void AddValue(IJitem value);

        IEnumerable<ConfigEntity> Yield();
    }
}