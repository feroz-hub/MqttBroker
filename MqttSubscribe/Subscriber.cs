using MQTTnet;
using MQTTnet.Client;

namespace MqttSubscribe;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        // Create an instance of the MQTT factory
        var factory = new MqttFactory();

        // Using statement ensures resources are properly disposed when the client is no longer needed
        using var mqttClient = factory.CreateMqttClient();
        // Configure MQTT client options
        var options = new MqttClientOptionsBuilder()
            .WithTcpServer("localhost", 1883) // Replace with the specific IP address and port of your MQTT server
            .WithClientId("Subscriber")
            .Build();

        // Connect to the MQTT server
        await mqttClient.ConnectAsync(options, CancellationToken.None);

        // Subscribe to the "test" topic
        var subscriptionResult = await mqttClient.SubscribeAsync("test");

        // Display subscription results
        subscriptionResult.Items.ToList().ForEach(s => Console.WriteLine($"Subscribed to '{s.TopicFilter.Topic}' with '{s.ResultCode}'"));

        // Event handler for handling incoming application messages
        mqttClient.ApplicationMessageReceivedAsync += async e =>
        {
            // Display received message information
            await Console.Out.WriteLineAsync($"Received message on topic: '{e.ApplicationMessage.Topic}' with Content '{e.ApplicationMessage.ConvertPayloadToString()}'\n\n");
        };

        Console.WriteLine("Subscribed to the topic. Press Enter to exit.");
        Console.ReadLine();
    }
}