using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using heitech.configXt.Core.Entities;

namespace heitech.configXt.Application
{
    public class BackTransform
    {
        ///<summary>
        /// works only one level deep for now 
        ///</summary>
        public static ConfigEntityJson Transform(IEnumerable<ConfigEntity> entities)
        {
            throw new NotImplementedException();
        }
    }
}