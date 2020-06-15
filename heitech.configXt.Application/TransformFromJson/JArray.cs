using System;
using System.Collections.Generic;
using System.Linq;
using heitech.configXt.Core.Entities;

namespace heitech.configXt.Application
{
    public class JArray : IJitem
    {
        public JArray(string name, IJitem parent)
        {
            Name = name;
            Parent = parent;
        }
        public string Name { get; }

        public IJitem Parent {get;}

        private List<IJitem> _values = new List<IJitem>();

        public void AddValue(IJitem value)
        {
            _values.Add(value);
        }

        public IEnumerable<ConfigEntity> Yield()
        {
            return new ConfigEntity[]
            {
                new ConfigEntity { Name = Name, Value = string.Join(",", _values.Select(x => x.Name)) }
            };
        }
    }
}