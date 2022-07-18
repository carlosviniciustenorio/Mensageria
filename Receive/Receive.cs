using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

var factory = new ConnectionFactory() { HostName = "localhost", UserName = "guest", Password = "guest" };
using (var connection = factory.CreateConnection())
using (var channel = connection.CreateModel())
{
    channel.ExchangeDeclare("direct_logs", ExchangeType.Topic, durable: true);

    channel.QueueDeclare(queue: "fakeLog1", durable: true, exclusive: false, autoDelete: false, arguments: null);
    channel.QueueDeclare(queue: "fakeLog2", durable: true, exclusive: false, autoDelete: false, arguments: null);
    
    channel.QueueBind(queue: "fakeLog1", exchange: "direct_logs", routingKey: "severity", arguments: null);
    channel.QueueBind(queue: "fakeLog2", exchange: "direct_logs", routingKey: "fakeRoutingKey", arguments: null);

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
    
    channel.BasicConsume(queue:"fakeLog1",
                         autoAck: true, 
                         consumer: consumer);

    channel.BasicConsume(queue: "fakeLog2",
                         autoAck: true, 
                         consumer: consumer);

    Console.WriteLine(" Press [enter] to exit.");
    Console.ReadLine();
}