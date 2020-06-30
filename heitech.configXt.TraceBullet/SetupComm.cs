using System;
using System.IO;
using heitech.configXt.Application;
using heitech.configXt.Core;
using heitech.configXt.Core.Entities;
using NetMQ.Sockets;

namespace heitech.configXt.TraceBullet
{
    public static class SetupComm
    {
        ///<summary>
        ///setup communcation with the CLI or any other client via ZeroMQ
        ///</summary>
        public static Communication GetCommunication(string tcpConnection)
        {
            var store = new InMemoryStore();
            // upload file from the current directory
            string json = JsonFromConfigFile();
            ITransform transform = new JsonTransform();

            OperationResult result = transform.Parse(json);

            var socket = new RequestSocket();
            socket.Connect(tcpConnection);

            return new Communication
            {
                Storage = store,
                Socket = socket,
                JsonString = json,
                Transform = transform,
                ConfigEntities = result.Result as ConfigCollection
            };
        }

        private static string JsonFromConfigFile()
        {
            var path = System.IO.Path.Combine(Environment.CurrentDirectory, "config.json");
            return File.ReadAllText(path);
        }
    }

    public class Communication
    {
        public IStorageModel Storage { get; set; }
        public ITransform Transform { get; set; }
        public RequestSocket Socket { get; set; }
        public ConfigCollection ConfigEntities { get; set; }
        public string JsonString { get; set; }
    }
}