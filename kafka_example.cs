using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.Kafka;
using System.Threading;

namespace kdotnet
{
    public static class kafka_example
    {
        [FunctionName("kafkaApp")]
        public static void ConfluentCloudStringTrigger(
             [KafkaTrigger(
                "BootstrapServer",
                "users",
                ConsumerGroup = "<ConsumerGroup>",
                Protocol = BrokerProtocol.SaslSsl,
                AuthenticationMode = BrokerAuthenticationMode.Plain,
                Username = "<APIKey>",
                Password = "<APISecret>",
                SslCaLocation = "confluent_cloud_cacert.pem")]
        KafkaEventData<string> kafkaEvent,
        ILogger logger)
        {	    
            logger.LogInformation(kafkaEvent.Value.ToString());
        }
    }
}