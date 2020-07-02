using System;
using System.Collections.Generic;

namespace heitech.configXt.Client.Mvc
{
    public class ConfigItems
    {
        public List<ConfigItem> List { get; set; }
    }

    public class ConfigItem
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public Guid Id { get; set; }
    }
}