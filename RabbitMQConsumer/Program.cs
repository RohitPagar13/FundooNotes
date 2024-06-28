using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQConsumer.Utilities;
using System.Text;

namespace RabbitMQConsumer
{
    public class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost"
            };

            //Create the RabbitMQ connection using connection factory details 
            var connection = factory.CreateConnection();

            //Here we create channel with session and model 
            using var channel = connection.CreateModel();

            //declare the queue after mentioning name and a few property related to that
            channel.QueueDeclare("NoteQueue", exclusive: false);

            //Set Event object which listen message from channel which is sent by producer
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                var data = JsonConvert.DeserializeObject<EmailModel>(json);
                Console.WriteLine($"Note Message received : {json}");
                if(data != null )
                {
                    EmailSender.SendEmail(new EmailModel { To = data.To, Subject = data.Subject, Body = data.Body });
                }
            };

            //read message 
            channel.BasicConsume(queue: "NoteQueue", autoAck: true, consumer: consumer);
            Console.ReadLine();
        }
    }
}