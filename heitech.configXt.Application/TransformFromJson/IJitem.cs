using System.Collections.Generic;
using heitech.configXt.Core.Entities;

namespace heitech.configXt.Application
{
    public interface IJitem
    {
        string Name { get; }
        IJitem Parent { get; }
        void AddValue(IJitem value);

        IEnumerable<ConfigEntity> Yield();
    }
}