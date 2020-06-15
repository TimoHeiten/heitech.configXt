using System;
using System.Linq;
using System.Threading.Tasks;
using heitech.configXt.Core;
using heitech.configXt.Core.Entities;

namespace heitech.configXt.Application.UseCases
{
    ///<summary>
    /// Not done yet -> todo
    ///</summary>
    public class UploadFileAsync : IUseCase
    {
        private readonly string _json;
        private readonly IStorageModel _model;
        private readonly ITransform _transform;
        private readonly IUseCase _storeUseCase;
        public UploadFileAsync(string json, ITransform transform, IStorageModel model, IUseCase storeUseCase)
        {
            _storeUseCase = storeUseCase;
            _transform = transform;
            _model = model;
            _json = json;
        }

        public async Task<OperationResult> RunUseCaseAsync()
        {
            OperationResult transformed = _transform.Transform(_json);
            if (transformed.IsSuccess == false)
            {
                return transformed;
            }
            
            if (transformed.Result is ConfigCollection collection)
            {
                // todo unit of work is required
                foreach (var item in collection.WrappedConfigEntities)
                {
                    var result = await RunNextItem(item);
                    if (result.IsSuccess == false)
                    {
                        return result;
                    }
                }
            }
            else
            {
                return OperationResult.Failure
                (
                    ResultType.InternalError, 
                    $"expected typeof({typeof(ConfigCollection)}) but got {transformed.Result.GetType()}"
                );
            }

            return OperationResult.Success(collection);
        }

        private Task<OperationResult> RunNextItem(ConfigEntity entity)
        {
            IUseCase store = StoreUseCase(entity.Name, entity.Value);

            return store.RunUseCaseAsync();
        }

        // test seam for Usecase instance
        protected virtual IUseCase StoreUseCase(string name, string value)
        {
            return new ConfigurationModel(name, value, _model);
        }

        // test seam for static Factory invoke
        protected virtual Task<OperationResult> OperateAsync(ConfigurationContext context)
        {
            return Factory.RunOperationAsync(context);
        }
    }
}