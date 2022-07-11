﻿using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

var factory = new ConnectionFactory() { HostName = "localhost", UserName = "carlos.tenorio", Password = "passwordtenorio" };
using (var connection = factory.CreateConnection())
using (var channel = connection.CreateModel())
{
    channel.QueueDeclare("work", false, false, false, null);

    var consumer = new EventingBasicConsumer(channel);
    consumer.Received += (model, ea) =>
    {
        var message = Encoding.UTF8.GetString(ea.Body.ToArray());
        Console.WriteLine(" [x] Received {0}", message);
        
        int dots = message.Split('.').Length - 1;
        Thread.Sleep(dots * 1000);

        Console.WriteLine(" [x] Done");

        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
    };

    channel.BasicConsume(queue:"work", autoAck: false, consumer: consumer);
    Console.WriteLine(" Press [enter] to exit.");
    Console.ReadLine();
}