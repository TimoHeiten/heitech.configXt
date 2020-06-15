using System.Collections.Generic;


var queue = new Queue<int>();

queue.Enqueue(1);
queue.Enqueue(2);
queue.Enqueue(3);
queue.Enqueue(4);

var list = queue.ToList();
System.Console.WriteLine(string.Join(":", list));
