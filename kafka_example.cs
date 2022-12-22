using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Kafka;
using Microsoft.Extensions.Logging;

namespace azure_functions_kafka_extension_sample_confluent
{
    public class kafka_example
    {
        
        [FunctionName("kafkaApp")]
        public void Run(
            [KafkaTrigger("BootstrapServer",
                          "users",
                          Username = "<APIKey>",
                          Password = "<APISecret>",
                          Protocol = BrokerProtocol.SaslSsl,
                          AuthenticationMode = BrokerAuthenticationMode.Plain,
                          ConsumerGroup = "<ConsumerGroup>")] KafkaEventData<string>[] events, ILogger log)
        {
            foreach (KafkaEventData<string> eventData in events)
            {
                log.LogInformation($"C# Kafka trigger function processed a message: {eventData.Value}");
            }
        }
    }
}
