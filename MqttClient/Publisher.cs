using System.Text;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;

namespace MqttPublish;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        // Create an instance of the MQTT factory
        var factory = new MqttFactory();

        // Create an MQTT client instance
        var mqttClient = factory.CreateMqttClient();

        // Configure MQTT client options
        var options = new MqttClientOptionsBuilder()
            .WithTcpServer("localhost", 1883) // Replace with the specific IP address and port of your MQTT server
            .WithClientId("Publisher")
            .WithCleanSession()
            .Build();

        // Connect to the MQTT server
        var connectResult = await mqttClient.ConnectAsync(options);

        // Define MQTT topic and message
        const string topic = "test";
        const string message = "Hello, MQTT!";

        // Check if the connection to the MQTT server was successful
        if (connectResult.ResultCode == MqttClientConnectResultCode.Success)
        {
            Console.WriteLine("Connected to the Mqtt Broker Successfully");

            // Build MQTT application message
            var applicationMessage = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(Encoding.UTF8.GetBytes(message))
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                .WithRetainFlag()
                .Build();

            // Publish the message to the MQTT server
            var result = await mqttClient.PublishAsync(applicationMessage);
            await Task.Delay(1000);//Wait for 1 Second

            Console.WriteLine($"Message published with result: {result.ReasonCode}");

            // Disconnect from the MQTT server
            await mqttClient.DisconnectAsync();

            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();
        }
        else
        {
            // Display an error message if the connection to the MQTT server fails
            Console.WriteLine($"Failed to connect to MQTT Broker: {connectResult.ResultCode}");
        }
    }
}