using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

var factory = new ConnectionFactory() { HostName = "localhost"};
using (var connection = factory.CreateConnection())
using (var channel = connection.CreateModel())
{
    channel.QueueDeclare("autorizacoes", false, false, false, null);

    var consumer = new EventingBasicConsumer(channel);
    consumer.Received += (model, ea) =>
    {
        var message = Encoding.UTF8.GetString(ea.Body.ToArray());
        Console.WriteLine(" [x] Received {0}", message);
    };

    channel.BasicConsume("autorizacoes", true, consumer);
    Console.WriteLine(" Press [enter] to exit.");
    Console.ReadLine();
}