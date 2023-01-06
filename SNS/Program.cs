using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;

public class Program
{
    static async Task Main(string[] args)
    {
        string message = "Message to SNS";

        var client = new AmazonSimpleNotificationServiceClient(awsAccessKeyId: "",
                                                               awsSecretAccessKey: "",
                                                               region: Amazon.RegionEndpoint.SAEast1);

        var request = new PublishRequest { Message = message, TopicArn = "arn:aws:sns:sa-east-1:841371166667:SMSTopic" };

        for (int i = 0; i < 10; i++)
        {
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
}
