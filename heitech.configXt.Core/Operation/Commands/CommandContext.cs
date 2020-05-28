using heitech.configXt.Core.Entities;

namespace heitech.configXt.Core
{
    public class CommandContext : ConfigurationContext
    {
        public ConfigChangeRequest ChangeRequest { get; }
        public CommandTypes CommandType { get; }

        public CommandContext(string admin, CommandTypes types, ConfigChangeRequest changeRequest, IStorageModel model)
            : base(admin, model)
        {
            CommandType = types;
            ChangeRequest = changeRequest;
            Check(changeRequest);
            Check(ChangeRequest.Claims);
        }
    }
}