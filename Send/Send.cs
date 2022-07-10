using System;
using RabbitMQ.Client;
using System.Text;

static void Main(string[] args)
{
    var factory = new ConnectionFactory() { HostName = "localhost" };
    using (var connection = factory.CreateConnection())
    using (var channel = connection.CreateModel())
    {
        channel.QueueDeclare("work", false, false, false, null);
        string message = GetMessage(args);

        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(exchange: "", routingKey: "work", basicProperties: null, body: body);
        Console.WriteLine("[X] Sent {0}", message);
    }
}

static string GetMessage(string[] args)
{
    return ((args.Length > 0) ? string.Join(" ", args) : "Hello World!");
}

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();