using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace heitech.configXt.TraceBullet
{
    class Program
    {
        static Task Main(string[] args)
        {
            string key = args.Any() == false ? "Usage" : args.FirstOrDefault();
            Func<Action<object>, Task> bullet = _map[key];

            return bullet(Print);
        }

        static Program()
        {
            _map.Add("TestStorage", a => TestStorageAndGeneralFlow.Run(a));
            _map.Add("Usage", a => UsingApplication.Run(a));
            _map.Add("Cli", a => InteractCli.Run(a));
        }

        public static void Print(object o)
        {
            System.Console.WriteLine(o);
        }

        private static Dictionary<string, Func<Action<object>, Task>> _map = new Dictionary<string, Func<Action<object>, Task>>();
    }
}
