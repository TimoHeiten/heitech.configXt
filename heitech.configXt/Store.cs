using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace heitech.configXt
{
    internal class FileStore : IStore
    {
        private SemaphoreSlim mutex = new SemaphoreSlim(1);
        private readonly string path;
        private readonly Encoding encoding = Encoding.UTF8;

        internal FileStore(string path)
        {
            this.path = System.IO.Path.Combine(path, "map.json");
        }

        public async Task FlushAsync(Dictionary<string, ConfigModel> map)
        {
            await mutex.WaitAsync();
            try
            {
                var data = JsonConvert.SerializeObject(map, Formatting.Indented);
                await File.WriteAllTextAsync(path, data, encoding);
            }
            finally
            {
                mutex.Release();
            }   
        }

        internal async Task<Dictionary<string, ConfigModel>> LoadAsync()
        {
            string text = "";
            await mutex.WaitAsync();
            try
            {
                if (!File.Exists(path))
                {
                    System.Console.WriteLine("create file");
                    await File.WriteAllTextAsync(path, "");
                }

                text = await File.ReadAllTextAsync(path, encoding);
                if (string.IsNullOrWhiteSpace(text))
                    return new Dictionary<string, ConfigModel>();
            }
            finally
            {
                mutex.Release();
            }
            return JsonConvert.DeserializeObject<Dictionary<string, ConfigModel>>(text);
        }
    }
}