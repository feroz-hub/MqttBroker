//using MQTTnet.Samples.Server;
//using MQTTnet.Server;

//public class Program
//{
//    public static async Task Main(string[] args)
//    {
//        await Server_TLS_Samples.Run_Server_With_Self_Signed_Certificate();
//    }
//}


using System;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Server;

namespace MqttServerExample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var mqttFactory = new MqttFactory();
            var mqttServerOption = new MqttServerOptionsBuilder().WithDefaultEndpoint().Build();

            var mqttServer = mqttFactory.CreateMqttServer(mqttServerOption);

            await mqttServer.StartAsync();

            Console.WriteLine("MQTT server started on port 1883.");

            Console.ReadLine();

            await mqttServer.StopAsync();
        }
    }
}
