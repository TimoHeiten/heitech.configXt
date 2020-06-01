using heitech.configXt.Core.Entities;

namespace heitech.configXt.Core.Commands
{
    public class CommandContext : ConfigurationContext
    {
        public ConfigChangeRequest ChangeRequest { get; }
        public CommandTypes CommandType { get; }

        public CommandContext(CommandTypes types, ConfigChangeRequest changeRequest, IStorageModel model)
            : base(model)
        {
            CommandType = types;
            Check(changeRequest);
            Check(changeRequest.Name);
            Check(changeRequest.Value);

            ChangeRequest = changeRequest;
            ConfigName = ChangeRequest.Name;
        }
    }
}