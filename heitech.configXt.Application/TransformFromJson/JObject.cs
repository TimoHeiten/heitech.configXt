using System.Collections.Generic;
using System.Linq;
using heitech.configXt.Core.Entities;

namespace heitech.configXt.Application
{
    public class Jobj2 : IJitem
    {
        private List<ConfigEntity> _values = new List<ConfigEntity>();
        public Jobj2(string name, IJitem parent)
        {
            Name = name;
            Parent = parent;
        }
        public string Name { get; }

        public IJitem Parent {get;}

        public void AddValue(IJitem value)
        {
            if (Parent != null)
                Parent.AddValue(value);
            else
                _values.Add(value.Yield().Single());
        }

        public IEnumerable<ConfigEntity> Yield()
        {
            return _values;
        }
    }
}