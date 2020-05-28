namespace heitech.configXt.Core
{
    public class ConfigClaim : StorageEntity
    {
        public const string CAN_CREATE = "CAN_CREATE";
        public string Value { get; set; }

        internal static ConfigClaim CanCreate() => 
            new ConfigClaim { Value = CAN_CREATE, Name =CAN_CREATE };
    }
}