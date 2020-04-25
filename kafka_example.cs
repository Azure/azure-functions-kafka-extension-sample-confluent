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
            [KafkaTrigger("pkc-epwny.eastus.azure.confluent.cloud:9092", "users",
                ConsumerGroup = "azfunc",
                Protocol = BrokerProtocol.SaslSsl,
                AuthenticationMode = BrokerAuthenticationMode.Plain,
                Username = "WHYYKOT6JMPYGJJV",
                Password = "KUtytGe72JzhCKeA7cZ6dJj6oRE4I5NyOjMxj157tQaiJoVy67UD0sAqJM9e4QAZ",
                SslCaLocation = "confluent_cloud_cacert.pem")]
            KafkaEventData<string> kafkaEvent,
            ILogger logger)
        {
	    Thread.Sleep(2000);
            logger.LogInformation(kafkaEvent.Value.ToString());
        }
    }
}