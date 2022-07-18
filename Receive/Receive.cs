using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

var factory = new ConnectionFactory() { HostName = "localhost", UserName = "guest", Password = "guest" };
using (var connection = factory.CreateConnection())
using (var channel = connection.CreateModel())
{
    channel.ExchangeDeclare("direct_logs", ExchangeType.Direct);
    
    channel.QueueBind(queue: "fake", exchange: "direct_logs", routingKey: "severity", arguments: null);

    channel.QueueBind(queue: "blackLogs",
                      exchange: "direct_logs",
                      routingKey: "severity");

    Console.WriteLine(" [*] Waiting for messages.");

    var consumer = new EventingBasicConsumer(channel);
    consumer.Received += (model, ea) =>
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        var routingKey = ea.RoutingKey;
        Console.WriteLine(" [x] Received '{0}':'{1}'",
                            routingKey, message);

        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple:false);
    };
    
    channel.BasicConsume(queue:"blackLogs",
                         autoAck: true, 
                         consumer: consumer);

    channel.BasicConsume(queue: "fake", autoAck: true, consumer: consumer);

    Console.WriteLine(" Press [enter] to exit.");
    Console.ReadLine();
}