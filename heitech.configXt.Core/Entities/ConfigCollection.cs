using System.Collections.Generic;
using System.Linq;

namespace heitech.configXt.Core.Entities
{
    ///<summary>
    /// Just for the Result
    ///</summary>
    public class ConfigCollection : ConfigEntity
    {
        public IEnumerable<ConfigEntity> WrappedConfigEntities { get; set; }
    }
}