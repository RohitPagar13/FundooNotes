using Confluent.Kafka;
using Confluent.Kafka.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Utilities
{
    public class KafkaProduser
    {
        private readonly string bootstrapServers;
        private readonly string topicName;
        private readonly int numPartitions;
        private readonly short replicationFactor;

        public KafkaProduser(string bootstrapServers = "localhost:9092", string topicName = "Trashed", int numPartitions = 3, short replicationFactor = 1)
        {
            this.bootstrapServers = bootstrapServers;
            this.topicName = topicName;
            this.numPartitions = numPartitions;
            this.replicationFactor = replicationFactor;

            CreateTopicIfNotExists().GetAwaiter().GetResult();
        }

        private async Task CreateTopicIfNotExists()
        {
            using var adminClient = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = bootstrapServers }).Build();
            try
            {
                    var topicSpecification = new TopicSpecification
                    {
                        Name = topicName,
                        NumPartitions = numPartitions,
                        ReplicationFactor = replicationFactor
                    };
                    await adminClient.CreateTopicsAsync(new List<TopicSpecification> { topicSpecification });
                    Console.WriteLine($"Topic '{topicName}' created.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred creating the topic: {ex.Message}");
            }
        }

        public async Task CreateMessageAsync(string input, string? email, int partitionKey)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = bootstrapServers,
                ClientId = email,
                BrokerAddressFamily = BrokerAddressFamily.V4,
            };

            using var producer = new ProducerBuilder<int, string>(config).Build();

            try
            {
                var deliveryReport = await producer.ProduceAsync(
                    new TopicPartition(topicName, new Partition(partitionKey)),
                    new Message<int, string> { Key = partitionKey, Value = input });

                Console.WriteLine($"Message delivered to {deliveryReport.TopicPartitionOffset}");
            }
            catch (ProduceException<int, string> ex)
            {
                Console.WriteLine($"An error occurred: {ex.Error.Reason}");
            }
        }
    }
}
