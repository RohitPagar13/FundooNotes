using Confluent.Kafka;

namespace KafkaConsumer2
{
    public class Program
    {
        public static void ReadMessage()
        {
            string topic = "Trashed";
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                ClientId = "KafkaConsumerClient2",
                GroupId = "Fundoo",
                BrokerAddressFamily = BrokerAddressFamily.V4,
            };
            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();

            consumer.Assign(new List<TopicPartitionOffset>
            {
                new TopicPartitionOffset(topic, 3, Offset.Beginning)
            });

            CancellationTokenSource cts = new CancellationTokenSource();

            Console.CancelKeyPress += (_, e) =>
            {
                e.Cancel = true;
                cts.Cancel();
                Console.WriteLine("Exiting");
            };

            try
            {
                while (true)
                {
                    var consumeResult = consumer.Consume();
                    Console.WriteLine($"Message received from {consumeResult.TopicPartitionOffset}: {consumeResult.Message.Value}");
                }
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                consumer.Close();
            }
        }
        static void Main(string[] args)
        {
            ReadMessage();
        }
    }
}