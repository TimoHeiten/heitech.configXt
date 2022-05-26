using heitech.configXt.Interface;

namespace heitech.configXt.api.Models
{
    public class UpsertModel
    {
        public string Key { get; set; } = null!;
        public object Value { get; set; } = null!;
        public ConfigKind Kind { get; set; } = ConfigKind.None;
    }
}