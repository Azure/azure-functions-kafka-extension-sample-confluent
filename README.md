---
page_type: sample
languages:
- csharp
products:
- azure-functions
description: "This is a simple sample which shows how to set up and write a function app which writes to a kafka topic"
---

# Azure Functions Kafka extension sample using Confluent Cloud

<!-- 
Guidelines on README format: https://review.docs.microsoft.com/help/onboard/admin/samples/concepts/readme-template?branch=master

Guidance on onboarding samples to docs.microsoft.com/samples: https://review.docs.microsoft.com/help/onboard/admin/samples/process/onboarding?branch=master

Taxonomies for products and languages: https://review.docs.microsoft.com/new-hope/information-architecture/metadata/taxonomies?branch=master
-->

This sample shows how to set up an write a .NET Function app which writes to a Kafka Topic. It is using Confluent Cloud for the Kafka cluster. It also shows how to deploy this app on a Premium Function app.

## Prerequisites

* [Azure Functions Core Tools](https://docs.microsoft.com/en-us/azure/azure-functions/functions-run-local?tabs=windows%2Ccsharp%2Cbash)
* [Azure Account](https://azure.microsoft.com/en-us/free/) and access to [Azure Portal](https://azure.microsoft.com/en-us/features/azure-portal/) OR [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/get-started-with-azure-cli?view=azure-cli-latest)
* [Confluent Cloud Account](https://www.confluent.io/confluent-cloud/)
Create a Confluent Cloud account. Confluent Cloud is a fully managed pay-as-you-go service. 

## Steps to set up 

### Setup the Kafka Cluster 

After you create a Confluent Cloud account follow these [steps](https://docs.confluent.io/current/quickstart/cloud-quickstart/index.html#cloud-quickstart) to get set up. Some of the main ones are also highlighted below.

* Log into in your Confluent Cloud account and create a new Kafka cluster.

![CreateConfluentCluster](https://github.com/Azure/azure-functions-kafka-extension-sample-confluent/blob/master/images/kafka-cluster-new.png)

* Create a new Kafka Topic called "users"
![CreateKafkaTopic](https://github.com/Azure/azure-functions-kafka-extension-sample-confluent/blob/master/images/kafka-new-topic.png)

* Create a new API Key and Secret - note these values


### Update the code 

* Clone this repository using Git to a folder

* Change thge code in kafka_example.cs to point to your Kafka cluster that you set up in the previous step
```c#
public static class ConfluentCloudTrigger
{
    [FunctionName(nameof(ConfluentCloudStringTrigger))]
    public static void ConfluentCloudStringTrigger(
        [KafkaTrigger(
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
```

Replace the following values:
* **BootstrapServer**: should contain the value of Bootstrap server found in Confluent Cloud settings page. Will be something like "xyz-xyzxzy.westeurope.azure.confluent.cloud:9092".<br>
* Set any string for your ConsumerGroup
* **APIKey**: is you API access key, obtained from the Confluent Cloud web site.<br>
* **APISecret**: is you API secret, obtained from the Confluent Cloud web site.<br>

* Note about the CA certificate: 
As described in [Confluent documentation](https://github.com/confluentinc/examples/tree/5.4.0-post/clients/cloud/csharp#produce-records), the .NET library does not have the capability to access root CA certificates.<br>
Missing this step will cause your function to raise the error "sasl_ssl://xyz-xyzxzy.westeurope.azure.confluent.cloud:9092/bootstrap: Failed to verify broker certificate: unable to get local issuer certificate"<br>
To overcome this, you need to:
    - Download CA certificate (i.e. from https://curl.haxx.se/ca/cacert.pem).
    - Rename the certificate file to anything other than cacert.pem to avoid any conflict with existing EventHubs Kafka certificate that is part of the extension.
    - Include the file in the project, setting "copy to output directory" and set the **SslCaLocation** trigger attribute property.     
    - In the example we have already downloaded this file and named it to `confluent_cloud_cacert.pem`  

### Running the sample

* Send some messages to the users Topic. You can do so either using the sample application given in the quick start or using the Confluent Cloud interface.

![CreateKafkaMessages](https://github.com/Azure/azure-functions-kafka-extension-sample-confluent/blob/master/images/kafka-cluster-create-messages.png)

* Run the following from the folder where you cloned the project to start the Function app locally

```
func host start
```

The Function app starts executing and should connect to your Confluent Cloud Kafka cluster.<br>

You should see the Partitions of your Topic that have been assigned to this client show up and messages that were sent before being processed.

![CreateKafkaMessages](https://github.com/Azure/azure-functions-kafka-extension-sample-confluent/blob/master/images/kafka-func-consume-messages.png)


### Deploying the sample to a Azure Functions Premium Plan

* Now you are ready to deploy this Function app to a [Azure Functions Premium Plan](https://docs.microsoft.com/en-us/azure/azure-functions/functions-premium-plan)

* Use the following [link](https://docs.microsoft.com/en-us/azure/azure-functions/functions-premium-plan#create-a-premium-plan) for instructions on how to first create an Azure Functions Premium plan Function app. Note the name of the Function app.

* To enable scaling in the Premium Function app currently you have to toggle a property on the Function app. 

You can use the Azure CLI

```
az resource update -g <resource_group> -n <function_app_name>/config/web --set properties.functionsRuntimeScaleMonitoringEnabled=1 --resource-type Microsoft.Web/sites
``
or you can use resources.azure.com 

* Once you are done you can deploy you locally created Function app to the app created in Azure by using the following [func command](https://docs.microsoft.com/en-us/azure/azure-functions/functions-run-local?tabs=windows%2Ccsharp%2Cbash#publish) by replacing the **NameOfFunctionApp** with the name of the Function app created in Azure in the previous step. <br>
Note: To use this command you have to be logged into Azure using Azure CLI

`
func azure function publish <NameOfFunctionApp>
`

* Finally, you can head over to the portal and for example use the [Live Metrics view](https://docs.microsoft.com/en-us/azure/azure-monitor/app/live-stream) to see the logs and requests.

![KafkaPortal](https://github.com/Azure/azure-functions-kafka-extension-sample-confluent/blob/master/images/kafka-function-portal.png)



### Contributing

This project welcomes contributions and suggestions.  Most contributions require you to agree to a
Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us
the rights to use your contribution. For details, visit https://cla.opensource.microsoft.com.

When you submit a pull request, a CLA bot will automatically determine whether you need to provide
a CLA and decorate the PR appropriately (e.g., status check, comment). Simply follow the instructions
provided by the bot. You will only need to do this once across all repos using our CLA.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or
contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.
