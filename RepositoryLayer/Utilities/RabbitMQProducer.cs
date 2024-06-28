using ModelLayer;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Utilities
{
    public class RabbitMQProducer
    {
        public void SendMessage(string message, string email, string subject)
        {
            //Here we specify the Rabbit MQ Server. we use rabbitmq docker image and use it
            var factory = new ConnectionFactory()
            {
                HostName = "localhost"
            };

            //Create the RabbitMQ connection using connection factory details as i mentioned above
            var connection = factory.CreateConnection();

            //Here we create channel with session and model
            using var channel = connection.CreateModel();

            //declare the queue after mentioning name and a few property related to that
            channel.QueueDeclare("NoteQueue", exclusive: false);

            //Serialize the message
            var json = JsonConvert.SerializeObject(new EmailModel { Body = message, To=email, Subject =subject }) ;
            var body = Encoding.UTF8.GetBytes(json);

            //put the data on to the note queue
            channel.BasicPublish(exchange: "", routingKey: "NoteQueue", body: body);
        }
    }
}
