using Microsoft.Azure.ServiceBus;
using ServiceBusShared.Models;
using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceBusReceiver
{
    class Program
    {
        const string connectionString = "Endpoint=sb://atanasservicebus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=mg9ss+ieBaMfQBsweG7yxLE73Q0iTi8fXuufA8KsGTQ=";
        const string queueName = "messagequeue";

        static IQueueClient queueClient;


        static async Task Main(string[] args)
        {
            queueClient = new QueueClient(connectionString, queueName);

            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };

            queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);

            Console.ReadLine();

            await queueClient.CloseAsync();
        }

        private static async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            var jsonString = Encoding.UTF8.GetString(message.Body);
            MessageModel message1 = JsonSerializer.Deserialize<MessageModel>(jsonString);

            Console.WriteLine(DateTime.Now + " : Message received: " + message1.Sender + " - " + message1.Content);

            await queueClient.CompleteAsync(message.SystemProperties.LockToken);
        }

        private static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs arg)
        {
            Console.WriteLine("Message Handler exception: " + arg.Exception);
            return Task.CompletedTask;
        }
    }
}
