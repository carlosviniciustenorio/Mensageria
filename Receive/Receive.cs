using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

var factory = new ConnectionFactory() { HostName = "localhost", UserName = "carlos.tenorio", Password = "passwordtenorio" };
using (var connection = factory.CreateConnection())
using (var channel = connection.CreateModel())
{
    channel.QueueDeclare(queue: "work",
                         durable: true, //Com o parâmetro durável = true, talvez o RabbitMQ grave as mensagens no disco e caso o servidor pare, a mensagem e a fila não serão apagadas. Isso traz mais garantia que não perderemos uma mensagem.
                         exclusive: false, 
                         autoDelete: false, 
                         arguments: null);

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

    channel.BasicConsume(queue:"work", autoAck: false, consumer: consumer); //AutoAck como false significa que o RabbitMQ usará o reconhecimento de mensagens manual e
                                                                            // não perderá as mensagens caso o consumidor seja parado ou contenha algum erro no processamento.
    Console.WriteLine(" Press [enter] to exit.");
    Console.ReadLine();
}