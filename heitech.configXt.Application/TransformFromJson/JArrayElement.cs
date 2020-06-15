using System;
using System.Collections.Generic;
using System.Linq;
using heitech.configXt.Core.Entities;

namespace heitech.configXt.Application
{
    public class JArrayElement : IJitem
    {
        public JArrayElement(string value)
        {
            Name = value;

        }
        public string Name { get; }

        public IJitem Parent => throw new NotSupportedException();

        public void AddValue(IJitem value)
        {
            // dont care 
        }

        public IEnumerable<ConfigEntity> Yield()
        {
            // dont care
            throw new NotSupportedException();
        }
    }
}