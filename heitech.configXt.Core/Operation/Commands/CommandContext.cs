namespace heitech.configXt.Core.Commands
{
    ///<summary>
    /// CommandContext Subtype for ConfigurationContext
    ///</summary>
    public class CommandContext : ConfigurationContext
    {
        public ConfigChangeRequest ChangeRequest { get; }
        public CommandTypes CommandType { get; }

        public CommandContext(CommandTypes types, ConfigChangeRequest changeRequest, IStorageModel model)
            : base(model)
        {
            CommandType = types;
            Check(changeRequest, "changeRequest");
            Check(changeRequest.Name, "changeRequest.Name");
            Check(changeRequest.Value, "changeRequest.Value");

            ChangeRequest = changeRequest;
            ConfigurationEntryKey = ChangeRequest.Name;
        }
    }
}