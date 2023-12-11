using System;
using MQTTnet.Samples.Server;

namespace MQTTServerSample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Choose a sample to run:");
            Console.WriteLine("1. Run Minimal Server");
            Console.WriteLine("2. Run Server With Logging");
            Console.WriteLine("3. Validating Connections");
            Console.WriteLine("4. Force Disconnecting Client");
            Console.WriteLine("5. Publish Message From Broker");
            Console.Write("Enter the number of the sample to run: ");

            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                switch (choice)
                {
                    case 1:
                        Server_Simple_Samples.Run_Minimal_Server().Wait();
                        break;
                    case 2:
                        Server_Simple_Samples.Run_Server_With_Logging().Wait();
                        break;
                    case 3:
                        Server_Simple_Samples.Validating_Connections().Wait();
                        break;
                    case 4:
                        Server_Simple_Samples.Force_Disconnecting_Client().Wait();
                        break;
                    case 5:
                        Server_Simple_Samples.Publish_Message_From_Broker().Wait();
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Invalid input.");
            }
        }
    }
}
