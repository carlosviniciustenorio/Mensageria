using System;
using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory() { HostName = "localhost" };
using (var connection = factory.CreateConnection())
using (var channel = connection.CreateModel())
{
    channel.QueueDeclare("autorizacoes",false,false,false, null);
    string message = "O usuário Isabelly Ribeiro Tenorio solicita a autorização do usuário Carlos Vinícius Tenorio";

    var body = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish(exchange: "", routingKey: "autorizacoes", basicProperties: null, body: body);
    Console.WriteLine("[X] Sent {0}", message);
}

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();