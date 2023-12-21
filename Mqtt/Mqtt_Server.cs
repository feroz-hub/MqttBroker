using MQTTnet;
using MQTTnet.Protocol;
using MQTTnet.Server;
using System;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace MqttBroker
{
    public static class Mqtt_Server
    {
        public static async Task Run_Minimal_Server()
        {
            var mqttFactory = new MqttFactory();

            var mqttServerOptions = new MqttServerOptionsBuilder().WithDefaultEndpoint().Build();

            using (var mqttServer = mqttFactory.CreateMqttServer(mqttServerOptions))
            {
                mqttServer.ValidatingConnectionAsync += e =>
                {
                    Console.WriteLine($"Client '{e.ClientId}' wants to connect. Accepting!");

                    Console.WriteLine($"Accepted connection from client '{e.ClientId}'");
                    return Task.CompletedTask;

                };

                await mqttServer.StartAsync();

                Console.WriteLine("Press Enter to exit.");
                Console.ReadLine();

                await mqttServer.StopAsync();
            }
        }

        public static async Task Run_Server_With_Mutual_Authentication()
        {
            var mqttFactory = new MqttFactory();

            // Load the server certificate and private key
            var serverCertificate = LoadCertificateFromTrustedCA("C:\\Certificates_MQTT\\server.pfx", "1234");

            var mqttServerOptions = new MqttServerOptionsBuilder()
                .WithDefaultEndpoint()
                .WithEncryptedEndpoint()
                .WithEncryptionCertificate(serverCertificate)
                .WithClientCertificate((sender, certificate, chain, sslPolicyErrors) =>
                    ValidateClientCertificate(new X509Certificate2(certificate), chain, sslPolicyErrors))
                .Build();

            using (var mqttServer = mqttFactory.CreateMqttServer(mqttServerOptions))
            {
                await mqttServer.StartAsync();

                Console.WriteLine("Press Enter to exit.");
                Console.ReadLine();

                // Stop and dispose the MQTT server if it is no longer needed!
                await mqttServer.StopAsync();
            }
        }

        static bool ValidateClientCertificate(X509Certificate2 certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {

            var caCertificate = LoadCertificateFromTrustedCA("C:\\Certificates_MQTT\\ca.pfx", "1234");
            var chainPolicy = new X509ChainPolicy
            {
                RevocationMode = X509RevocationMode.NoCheck,
                VerificationFlags = X509VerificationFlags.NoFlag,
                VerificationTime = DateTime.Now
            };
            chain.ChainPolicy = chainPolicy;

            var chainIsValid = chain.Build(certificate) && certificate.Issuer == caCertificate.Subject;

            if (!chainIsValid)
            {
                Console.WriteLine($"Client certificate validation failed: {sslPolicyErrors}");
                return false;
            }

            // Check additional criteria
            if (certificate.Subject != "CN=mqtt.server")
            {
                Console.WriteLine("Client certificate validation failed: Incorrect Common Name (CN).");
                return false;
            }

            // Check if the certificate is not expired
            if (DateTime.Now > certificate.NotAfter || DateTime.Now < certificate.NotBefore)
            {
                Console.WriteLine("Client certificate validation failed: Certificate expired.");
                return false;
            }

            // Add more checks if needed

            return true;
        }

        static X509Certificate2 LoadCertificateFromTrustedCA(string path, string password = null)
        {
            return new X509Certificate2(path, password);
        }

        public static async Task ValidMqttClient()
        {
            var mqttFactory = new MqttFactory();

            var mqttServerOptions = new MqttServerOptionsBuilder().WithDefaultEndpoint().Build();

            var allowedClientIds = new List<string> { "Client1", "Client2", "Client3", "Feroz" };

            using (var mqttServer = mqttFactory.CreateMqttServer(mqttServerOptions))
            {
                mqttServer.ValidatingConnectionAsync += e =>
                {
                    // Check if the provided ClientId is in the list of allowed client IDs
                    if (!allowedClientIds.Contains(e.ClientId))
                    {
                        e.ReasonCode = MqttConnectReasonCode.ClientIdentifierNotValid;
                        Console.WriteLine($"Rejected connection from client '{e.ClientId}' - Client ID not allowed.");
                        return Task.CompletedTask;
                    }

                    // If you have additional validation, you can add it here

                    Console.WriteLine($"Accepted connection from client '{e.ClientId}'");

                    return Task.CompletedTask;
                };

                await mqttServer.StartAsync();

                Console.WriteLine("Press Enter to exit.");
                Console.ReadLine();

                await mqttServer.StopAsync();
            }
        }

    }
}
