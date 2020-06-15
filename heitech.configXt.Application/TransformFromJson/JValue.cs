using System;
using System.Collections.Generic;
using heitech.configXt.Core.Entities;

namespace heitech.configXt.Application
{
     public class JValue : IJitem
    {
        public JValue(string value, IJitem parent /*needs to be a Jproperty*/)
        {
            if (!(parent is JProperty))
                throw new ArgumentException();
            Name = value;
            Parent = parent;
        }
        public string Name { get; } // is the value 

        public IJitem Parent { get; }

        public void AddValue(IJitem value)
        {
            throw new NotSupportedException();
        }

        public IEnumerable<ConfigEntity> Yield()
        {
            return new ConfigEntity[]
            {
                new ConfigEntity { Name = Parent.Name, Value = Name }
            };
        }
    }
}