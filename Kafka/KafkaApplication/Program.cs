using System.Text.Json;
using Confluent.Kafka;
using KafkaApplication;
using Serilog;

class Program
{
    static async Task Main(string[] args)
    {
        var logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();
        
        string bootstrapServers = "localhost:9092";
        string topicName = "products-ecommerce";

        logger.Information($"BootstrapServers = {bootstrapServers}");
        logger.Information($"Topic = {topicName}");

        try
        {
            var config = new ProducerConfig
            {
                BootstrapServers = bootstrapServers
            };

            using (var producer = new ProducerBuilder<Null, string>(config).Build())
            {
                    Product productToSend = new(){Id = Guid.NewGuid(), Name = "Product to Topic", Description = Guid.NewGuid().ToString()};
                    var objToSend = JsonSerializer.Serialize(productToSend);
                    var result = await producer.ProduceAsync(
                        topicName,
                        new Message<Null, string>
                        { Value = objToSend });

                    logger.Information(
                        $"Mensagem: {objToSend} | " +
                        $"Status: {result.Status.ToString()}");
            }

            logger.Information("Concluído o envio de mensagens");
        }
        catch (Exception ex)
        {
            logger.Error($"Exceção: {ex.GetType().FullName} | " +
                         $"Mensagem: {ex.Message}");
        }
    }
}