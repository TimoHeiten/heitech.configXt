using System;
using System.Threading.Tasks;
using heitech.configXt.Application.UseCases;
using heitech.configXt.Core;
using heitech.configXt.Models;

namespace heitech.configXt.Application
{
    public class MemoryInteract : IInteract
    {
        private readonly IStorageModel _model;
        public MemoryInteract(IStorageModel model)
        {
            _model = model;
        }

        public Task<OperationResult> DownloadAs(string indicator)
        {
            // todo implement Format
            throw new NotSupportedException("download is not supported yet");
        }

        public async Task<OperationResult> Run(ContextModel model)
        {
            // check is allowed
            bool isAllowed = true;
            string access = "write";
            if (model.Type == ContextType.ReadEntry || model.Type == ContextType.ReadAllEntries)
            {
                access = "read";
                isAllowed =  await _model.IsAllowedReadAsync(model.User, model.AppName);
            }
            else
            {
                isAllowed = await _model.IsAllowedWriteAsync(model.User, model.AppName);
            }
            if (!isAllowed)
            {
                return OperationResult.Failure
                (
                    ResultType.Forbidden,
                    $"Not allowed: Access [{access}] with AuthModel.Name [{model.User.Name}] and AppName [{model.AppName}]"
                );
            }

            switch (model.Type)
            {
                case ContextType.CreateEntry:
                    return await Factory.RunOperationAsync(_model.CreateContext(model.Key, model.Value));
                case ContextType.ReadEntry:
                    return await Factory.RunOperationAsync(_model.QueryOne(model.Key));
                case ContextType.ReadAllEntries:
                    return await Factory.RunOperationAsync(_model.QueryAll(model.Key));
                case ContextType.UpdateEntry:
                    return await Factory.RunOperationAsync(_model.UpdateContext(model.Key, model.Value));
                case ContextType.DeleteEntry:
                    return await Factory.RunOperationAsync(_model.DeleteContext(model.Key));
                case ContextType.DownloadAsFile:
                    return await DownloadAs(model.Key);
                case ContextType.UploadAFile:
                    var result = await Upload(model.Value, model.Key);
                    return result;
                default:
                    throw new NotSupportedException($"model.contextType : [{model.Type.ToString()}] is not supported");
            }
        }

        public Task<OperationResult> Upload(string fileItems, string indicator)
        {
            indicator = indicator.ToLowerInvariant();
            switch (indicator)
            {
                case "json":
                    var useCase = new UploadFileAsync(fileItems, new JsonTransform(), _model);
                    return useCase.RunUseCaseAsync();
                default:
                    throw new NotSupportedException($"indicator: {indicator} is not yet supported");
            }
        }
    }
}