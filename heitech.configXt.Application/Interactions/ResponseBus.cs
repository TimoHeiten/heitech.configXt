using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using heitech.configXt.Application;
using heitech.configXt.Core;
using heitech.configXt.Core.Entities;
using heitech.configXt.Models;
using NetMQ;
using NetMQ.Sockets;
using Newtonsoft.Json;

namespace heitech.configXt.Application
{
    public class ResponseBus : IResponseBus
    {
        private readonly ResponseSocket _socket;
        private readonly string _connection;
        public ResponseBus(string connection)
        {
            _connection = connection;
            _socket = new ResponseSocket();
        }

        public void Connect() => _socket.Bind(_connection);

        public void Close() => Dispose();
        public void Dispose() => _socket?.Dispose();

        public async Task RespondAsync(Func<ContextModel, Task<OperationResult>> response)
        {
            UiOperationResult uiResult = null;
            try
            {
                var contextModel = _socket.ReceiveFrameString();
                ContextModel model = JsonConvert.DeserializeObject<ContextModel>(contextModel);
                if (model.User == null)
                {
                    throw new InvalidOperationException($"input: {contextModel} ain`t no ContextModel");
                }
                var opResult = await response(model);

                uiResult = FromOperationResult(opResult);
            }
            catch (System.Exception ex)
            {
                var oR = OperationResult.Failure
                (
                    ResultType.BadRequest,
                    $"could not parse contextModel: {ex.Message}"
                );
                uiResult = FromOperationResult(oR);
            }

            string json = JsonConvert.SerializeObject(uiResult);
            byte[] bytes =  Encoding.UTF8.GetBytes(json);
            _socket.SendFrame(bytes);
        }

        private static UiOperationResult FromOperationResult(OperationResult result)
        {
            ConfigEntity entity = result.Result;
            var models = new List<ConfigurationModel>();
            if (entity != null)
            {
                if (entity is ConfigCollection collection)
                {
                    models = collection.WrappedConfigEntities
                                                    .Select(x => new ConfigurationModel { Name = x.Name, Value = x.Value })
                                                    .ToList();
                }
                else
                {
                    models.Add
                    (
                        new ConfigurationModel 
                        { 
                            Name = entity.Name, 
                            Value = entity.Value 
                        }
                    );
                }
            }
            return new UiOperationResult
            (
                success: result.IsSuccess,
                resultType: result.ResultType.ToString(),
                errorMessage: result.ErrorMessage,
                model: models
            );
        }
    }
}