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
        public async Task CreateMessageAsync(string input, string? email, int partitionKey)
        {
            
            var config = new ProducerConfig
            {
                BootstrapServers = "localhost:9092",
                ClientId = email,
                BrokerAddressFamily = BrokerAddressFamily.V4,
            };

            using var adminClient = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = config.BootstrapServers }).Build();
            var topicSpecification = new TopicSpecification
            {
                Name = "Trashed",
                NumPartitions = 3,
                ReplicationFactor = 1 // Adjust replication factor as needed
            };
            await adminClient.CreateTopicsAsync(new List<TopicSpecification> { topicSpecification });

            using var producer = new ProducerBuilder<int,
                string>(config).Build();
            
            var deliveryReport = await producer.ProduceAsync(new TopicPartition(topicSpecification.Name, new Partition(partitionKey)), new Message<int, string> { Key = partitionKey, Value = input });
            Console.WriteLine($"Message delivered to {deliveryReport.TopicPartitionOffset}");
        }
    }
}
