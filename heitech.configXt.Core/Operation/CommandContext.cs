using heitech.configXt.Core.Entities;

namespace heitech.configXt.Core.Operation
{
    public class CommandContext : ConfigurationOperationContext
    {
        public ConfigChangeRequest ChangeRequest { get; }
        public CommandTypes CommandType { get; }

        public CommandContext(AdministratorEntity admin, CommandTypes types, ConfigChangeRequest changeRequest, IStorageModel model)
            : base(admin, model)
        {
            Check(changeRequest);
            CommandType = types;
            ChangeRequest = changeRequest;
        }
    }
}