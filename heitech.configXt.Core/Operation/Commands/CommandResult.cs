using heitech.configXt.Core.Entities;

namespace heitech.configXt.Core
{
    public class Result
    {
        public ConfigEntity Before { get; set; }
        public ConfigEntity After { get; set; }
        public bool Success { get; set; }
        public string RequestType { get; set; }
    }
}