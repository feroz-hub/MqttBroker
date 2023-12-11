using MQTTnet;
using MQTTnet.Client;
using System.Text;
using MQTTnet.Protocol;

class Program
{
    static async Task Main(string[] args)
    {
        var factory = new MqttFactory();
        var mqttClient = factory.CreateMqttClient();

        var options = new MqttClientOptionsBuilder()
            .WithTcpServer("localhost", 1883) // Replace with the specific IP address and port of your MQTT server
            .WithClientId("Publisher")
            .Build();

        await mqttClient.ConnectAsync(options);

        string topic = "test";
        string message = "Hello, MQTT!";

        MqttApplicationMessage applicationMessage = new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            .WithPayload(Encoding.UTF8.GetBytes(message))
            .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
            .WithRetainFlag(false)
            .Build();

        MqttClientPublishResult result = await mqttClient.PublishAsync(applicationMessage);

        Console.WriteLine($"Message published with result: {result.ReasonCode}");

        await mqttClient.DisconnectAsync();

        Console.WriteLine("Press Enter to exit.");
        Console.ReadLine();
    }
}
