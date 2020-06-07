using System.Threading.Tasks;
using heitech.configXt.Core;
using heitech.configXt.Core.Commands;
using heitech.configXt.Core.Queries;

namespace heitech.configXt.Application.UseCases
{
    // 
    public class ConfigurationModel : IUseCase
    {
        private readonly string _name;
        private readonly string _value;
        private readonly IStorageModel _model;
        public ConfigurationModel(string name, string value, IStorageModel model)
        {
            _name = name;
            _model = model;
            _value = value;
        }

        ///<summary>
        /// set value to null explicitly to initiate delete
        ///</summary>
        public async Task<OperationResult> RunUseCaseAsync()
        {
            // check if context exists
            var query = new QueryContext(_name, QueryTypes.ValueRequest, _model);
            OperationResult result = await OperateAsync(query);

            var request = new ConfigChangeRequest()
            {
                Value = _value,
                Name = _name
            };
            bool isDelete = _value == null;
            CommandTypes type = isDelete
                                ? CommandTypes.Delete
                                : FindCommandType(result); 

            var cmd = new CommandContext(type, request, _model);
            var commandResult = await OperateAsync(cmd);

            return commandResult;
        }

        // test seam for static Factory invoke
        protected virtual Task<OperationResult> OperateAsync(ConfigurationContext context)
        {
            return Factory.RunOperationAsync(context);
        }

        private CommandTypes FindCommandType(OperationResult result)
        {
            return result.IsSuccess ? CommandTypes.UpdateValue : CommandTypes.Create;
        }
    }
}