namespace heitech.configXt.Core
{
    public class ConfigClaim : StorageEntity
    {
        public const string CAN_READ = "CAN_READ";
        public const string CAN_CREATE = "CAN_CREATE";
        public const string CAN_DELETE = "CAN_DELETE";
        public const string CAN_UPDATE = "CAN_UPDATE";
        public const string CAN_UPDATE_CLAIMS = "CAN_UPDATE_CLAIMS";
        public string Value { get; set; }

        internal static ConfigClaim CanCreate() => 
            new ConfigClaim { Value = CAN_CREATE, Name =CAN_CREATE };
    }
}