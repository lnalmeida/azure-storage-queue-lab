﻿using System;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace MessagePublisher;

    public partial class Program
    {
        public static async Task Main(string[] args)
        {
            Person personData = new Person
            {
                Name = "John Doe",
                Age = 30
            };
            string stringPersonData = JsonSerializer.Serialize(personData);
            string connectionString = "Endpoint=sb://lab-stgqueue-namespace.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=LJ3mjAaoauU2j3EsDJI1K6oTgjX4W7nWD+ASbNDlEdI=";
            string queueName = "messagequeue";
            ServiceBusMessage messageToSend = new ServiceBusMessage(stringPersonData);
            await SendMessage(connectionString, queueName, messageToSend);
        }

        private static async Task SendMessage(string connectionString, string queueName, ServiceBusMessage message)
        {
            await using (ServiceBusClient client = new ServiceBusClient(connectionString))
            {
                ServiceBusSender sender = client.CreateSender(queueName);
                await sender.SendMessageAsync(message);
                Console.WriteLine($"Sent a single message to the queue: {queueName}");
            }
        }
    }

    public class Person
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
    }
