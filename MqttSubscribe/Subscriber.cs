using MQTTnet;
using MQTTnet.Client;
using MqttSubscribe;
using System.Text;
using System.Text.Json;

class Program
{
    static async Task Main(string[] args)
    {
        var factory = new MqttFactory();



        using (var mqttClient = factory.CreateMqttClient())
        {
            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("localhost", 1883) // Replace with the specific IP address and port of your MQTT server
                .WithClientId("Subscriber")
                .Build();
            mqttClient.ApplicationMessageReceivedAsync += e =>
            {
            Console.WriteLine("Received message on topic");
            e.DumpToConsole();
                return Task.CompletedTask;

            };

            await mqttClient.ConnectAsync(options, CancellationToken.None);

            var mqttSubcribeOption = factory.CreateSubscribeOptionsBuilder()
                           .WithTopicFilter(f =>
                           {
                               f.WithTopic("test");
                           })
                           .Build();

            await mqttClient.SubscribeAsync(mqttSubcribeOption, CancellationToken.None);








            Console.WriteLine("Subscribed to the topic. Press Enter to exit.");
            Console.ReadLine();
        }
    }
   
}
