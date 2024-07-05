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
                AutoOffsetReset = AutoOffsetReset.Latest,
                ClientId = "KafkaConsumerClient2",
                GroupId = "Fundoo",
                BrokerAddressFamily = BrokerAddressFamily.V4,
            };
            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            
            consumer.Assign(new List<TopicPartitionOffset>
            {
                new TopicPartitionOffset(topic, 0, Offset.Beginning)
            });

            CancellationTokenSource cts = new CancellationTokenSource();

            Console.CancelKeyPress += (_, e) =>
            {
                e.Cancel = true;
                cts.Cancel();
                Console.WriteLine("Exiting");
            };

            Console.WriteLine("Consumer 2: Press Enter to Exit");

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
            catch (OperationCanceledException ex)
            {
                Console.WriteLine(ex.Message);
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