using System;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace MEssageConsumer;

public partial class Program
{
    static async Task Main(string[] args)
    {
        string connectionString = "Endpoint=sb://svcbus-lab-namespace.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=RSXIjCRCmL9f9GelfuH+vEUIcipIQzkOi+ASbMgR6Ls=";
        string queueName = "messagequeue";

        await using var client = new ServiceBusClient(connectionString);
        ServiceBusReceiver receiver = client.CreateReceiver(queueName);

        while (true)
        {
            ServiceBusReceivedMessage message = await receiver.ReceiveMessageAsync();
            if (message != null)
            {
                string body = message.Body.ToString();
                Console.WriteLine($"Received: {body}");

                try
                {
                    Person person = JsonSerializer.Deserialize<Person>(body);
                    Console.WriteLine($"Person - Name: {person.Name}, Age: {person.Age}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Message is not in JSON format, treating as plain text: {e.Message}");
                }

                await receiver.CompleteMessageAsync(message);
            }
        }
    }
}

    public class Person
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; } = 0;
    }

    public class MessageBody
    {
        public string Name { get; set; } = string.Empty;
        public string? Message { get; set; } = string.Empty;
    }