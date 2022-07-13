using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

var factory = new ConnectionFactory() { HostName = "localhost", UserName = "guest", Password = "guest" };
using (var connection = factory.CreateConnection())
using (var channel = connection.CreateModel())
{
    channel.ExchangeDeclare("logs", ExchangeType.Fanout);
    channel.QueueBind(queue: "work", exchange: "logs", routingKey: "");
    channel.QueueBind(queue: "logfake", exchange: "logs", routingKey: "");

    Console.WriteLine(" [*] Waiting for logs.");

    var consumer = new EventingBasicConsumer(channel);
    consumer.Received += (model, ea) =>
    {
        var message = Encoding.UTF8.GetString(ea.Body.ToArray());
        Console.WriteLine(" [x] Received {0}", message);
        
        int dots = message.Split('.').Length - 1;
        Thread.Sleep(dots * 1000);

        Console.WriteLine(" [x] Done");

        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false); //BasicAck confirma que a mensagem foi entregue
    };

    channel.BasicConsume(queue:"work",
                         autoAck: true, 
                         consumer: consumer);

    channel.BasicConsume(queue:"logfake",
                         autoAck: true, 
                         consumer: consumer);

    Console.WriteLine(" Press [enter] to exit.");
    Console.ReadLine();
}