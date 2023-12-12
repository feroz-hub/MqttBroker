using MQTTnet;
using MQTTnet.Server;

namespace MqttServerExample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Create an instance of the MQTT factory
            var mqttFactory = new MqttFactory();

            // Build MQTT server options with default endpoint
            var mqttServerOption = new MqttServerOptionsBuilder().WithDefaultEndpoint().Build();

            // Define a list of allowed client IDs
            var allowedClientId = new List<string> { "Publisher", "Subscriber", "Client3" };

            // Using statement ensures resources are properly disposed when the server is no longer needed
            using (var mqttServer = mqttFactory.CreateMqttServer(mqttServerOption))
            {
                // Event handler for validating incoming connections
                mqttServer.ValidatingConnectionAsync += e =>
                {
                    // Check if the client ID is allowed
                    if (!allowedClientId.Contains(e.ClientId))
                    {
                        // Reject connection if client ID is not allowed
                        e.ReasonCode = MQTTnet.Protocol.MqttConnectReasonCode.ClientIdentifierNotValid;
                        Console.WriteLine($"Rejected connection from server '{e.ClientId}' - Client ID not Allowed");
                        return Task.CompletedTask;
                    }

                    // Accept connection if client ID is allowed
                    Console.WriteLine($"Accepted connection from Server '{e.ClientId}'");
                    return Task.CompletedTask;
                };

                // Start the MQTT server
                await mqttServer.StartAsync();

                Console.WriteLine("MQTT server started on port 1883.");

                // Wait for user input before stopping the server
                Console.ReadLine();

                // Stop the MQTT server
                await mqttServer.StopAsync();
            }
        }
    }
}
