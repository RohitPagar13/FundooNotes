using Confluent.Kafka;

namespace KafkaConsumer
{
    public class Program
    {
        public static void ReadMessage()
        {
            string topic = "Trashed";
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                AutoOffsetReset = AutoOffsetReset.Latest,
                ClientId = "KafkaConsumerClient",
                GroupId = "Fundoo",
                BrokerAddressFamily = BrokerAddressFamily.V4,
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();

            consumer.Assign(new List<TopicPartitionOffset>
        {
            new TopicPartitionOffset(topic, 1, Offset.Beginning),
            new TopicPartitionOffset(topic, 2, Offset.Beginning)
        });

            using CancellationTokenSource cts = new CancellationTokenSource();

            Console.CancelKeyPress += (_, e) =>
            {
                e.Cancel = true;
                cts.Cancel();
                Console.WriteLine("Exiting");
            };

            Console.WriteLine("Consumer 1: Press Enter to Exit");

            try
            {
                while (!cts.Token.IsCancellationRequested)
                {
                    try
                    {
                        var consumeResult = consumer.Consume(cts.Token);
                        Console.WriteLine($"Message received from {consumeResult.TopicPartitionOffset}: {consumeResult.Message.Value}: Partition no: {consumeResult.Partition.Value}");
                    }
                    catch (ConsumeException ex)
                    {
                        Console.WriteLine($"Consume error: {ex.Error.Reason}");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Consumption was cancelled.");
            }
            finally
            {
                consumer.Close();
                Console.WriteLine("Consumer closed.");
            }
        }

        static void Main(string[] args)
        {
            ReadMessage();
        }
    }
}