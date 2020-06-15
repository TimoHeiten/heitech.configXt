using System.Collections.Generic;
using heitech.configXt.Core.Entities;

namespace heitech.configXt.Application
{
    public class JProperty : IJitem
    {
        private IJitem _value;
        public JProperty(string name, IJitem parent)
        {
            Parent = parent;
            Name = Parent.Name != string.Empty 
                   ? Parent.Name + ":" + name 
                   : name;
        }
        public string Name { get; }

        public IJitem Parent { get; }

        public void AddValue(IJitem value)
        {
            _value = value;
            Parent.AddValue(this);
        }

        public IEnumerable<ConfigEntity> Yield()
        {
            return _value.Yield();
        }
    }
}