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
        private readonly IAuthStorageModel _authStorage;

        public MemoryInteract(IStorageModel model, IAuthStorageModel authStorage)
        {
            _model = model;
            _authStorage = authStorage;
        }

        public Task<OperationResult> DownloadAs(string indicator)
        {
            // todo implement Format
            throw new NotSupportedException("download is not supported yet");
        }

        private bool IsUserInteraction(ContextModel model)
        {
            return model.Type == ContextType.AddUser 
               || model.Type == ContextType.DeleteUser 
               || model.Type == ContextType.UpdateUser
               || model.Type == ContextType.GetUser;
        }

        public async Task<OperationResult> Run(ContextModel model)
        {
            // check is allowed
            bool isAllowed = true;
            string access = "write";
            if (IsUserInteraction(model))
            {
                isAllowed = true;
            }
            else if (model.Type == ContextType.ReadEntry || model.Type == ContextType.ReadAllEntries)
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

            return await RunNoAuth(model);
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

        public async Task<OperationResult> RunNoAuth(ContextModel model)
        {
            ConfigurationContext ctxt;
            switch (model.Type)
            {
            // Commands
                case ContextType.CreateEntry:
                    ctxt = _model.CreateContext(model.Key, model.Value);
                    return await Factory.RunOperationAsync(ctxt);
                case ContextType.UpdateEntry:
                    ctxt = _model.UpdateContext(model.Key, model.Value);
                    return await Factory.RunOperationAsync(ctxt);
                case ContextType.DeleteEntry:
                    ctxt = _model.DeleteContext(model.Key);
                    return await Factory.RunOperationAsync(ctxt);
            // Queries
                case ContextType.ReadEntry:
                    ctxt = _model.QueryOne(model.Key);
                    return await Factory.RunOperationAsync(ctxt);
                case ContextType.ReadAllEntries:
                    ctxt = _model.QueryAll(model.Key);
                    return await Factory.RunOperationAsync(ctxt);
            // Import / Export
                case ContextType.DownloadAsFile:
                    return await DownloadAs(model.Key);
                case ContextType.UploadAFile:
                    var result = await Upload(model.Value, model.Key);
                    return result;
            // User Operations
                case ContextType.AddUser:
                    ctxt = _model.CreateUserContext(model.User, _authStorage, model.AppClaims);
                    return await Factory.RunOperationAsync(ctxt);
                case ContextType.UpdateUser:
                    ctxt = _model.UpdateUserContext(model.User, _authStorage, model.AppClaims);
                    return await Factory.RunOperationAsync(ctxt);
                case ContextType.DeleteUser:
                    ctxt = _model.DeleteUserContext(model.User, _authStorage);
                    return await Factory.RunOperationAsync(ctxt);
                case ContextType.GetUser:
                    ctxt = _model.ReadUserContext(model.User, _authStorage);
                    return await Factory.RunOperationAsync(ctxt);
                default:
                    throw new NotSupportedException($"model.contextType : [{model.Type.ToString()}] is not supported");
            }
        }
    }
}