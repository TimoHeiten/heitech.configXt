using NetMQ;
using System.Threading.Tasks;
using NetMQ.Sockets;
using System.Text;
using heitech.configXt.Models;

namespace heitech.configXt.Application
{
    public class RequestBus : IRequestBus
    {
        private readonly RequestSocket _socket;
        private readonly string _connection;
        public RequestBus(string connection)
        {
            _connection = connection;
            _socket = new RequestSocket();
        }

        public void Close() => Dispose();

        public void Connect()
            => _socket.Connect(_connection);

        public void Dispose() => _socket.Close();

        public Task<UiOperationResult> RequestAsync(ContextModel request)
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(request);

            byte[] bytes = Encoding.UTF8.GetBytes(json);
            _socket.SendFrame(bytes);

            string result = _socket.ReceiveFrameString();
            var uiResult = Newtonsoft.Json.JsonConvert.DeserializeObject<UiOperationResult>(result);

            return Task.FromResult(uiResult);
        }
    }
}