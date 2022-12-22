using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Kafka;
using Microsoft.Extensions.Logging;

namespace azure_functions_kafka_extension_sample_confluent
{
    public class kafka_example
    {
        
        [FunctionName("kafka_example")]
        public void Run(
            [KafkaTrigger("pkc-4j8dq.southeastasia.azure.confluent.cloud:9092",
                          "topic_0",
                          Username = "XA7O3AF2JBXUQDVU",
                          Password = "9tu6Ywa5fKRezcxJ7327uSvr7aLWsIvWAwNFyzSPD+rMx0K5Oe3c8lgdQhA9OezO",
                          Protocol = BrokerProtocol.SaslSsl,
                          AuthenticationMode = BrokerAuthenticationMode.Plain,
                          ConsumerGroup = "$Default")] KafkaEventData<string>[] events,
            ILogger log)
        {
            foreach (KafkaEventData<string> eventData in events)
            {
                log.LogInformation($"C# Kafka trigger function processed a message: {eventData.Value}");
            }
        }
    }
}
