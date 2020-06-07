#r "nuget: Newtonsoft.Json, 12.0.1"

// using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

var json = File.ReadAllText
(
    Path.Combine(Environment.CurrentDirectory, "config-test.json")
);

// var reader = new JsonTextReader(new StringReader(json));
// while (reader.Read())
// {
//     if (reader.Value != null)
//     {
//         Console.WriteLine("Token: {0}, Value: {1}", reader.TokenType, reader.Value);
//     }
//     else
//     {
//         Console.WriteLine("Token: {0}", reader.TokenType);
//     }
// }

// solve with state machine!