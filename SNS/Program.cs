using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;

public class Program
{
    static async Task Main(string[] args)
    {
        string message = "Oi, momo. Estou te mandando esse SMS através da aplicação que criei para fazer notificação. Te amo, denguinho!";

        var client = new AmazonSimpleNotificationServiceClient(awsAccessKeyId: "",
                                                               awsSecretAccessKey: "",
                                                               region: Amazon.RegionEndpoint.SAEast1);

        var topics = await client.ListTopicsAsync();
        
        var request = new PublishRequest { Message = message, TopicArn = "arn:aws:sns:sa-east-1:841371166667:MyTopic"};

        try
        {
            var response = client.PublishAsync(request);
            Console.WriteLine(response);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Caught exception publishing request:");
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}