namespace MqttBroker;

internal static class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Select the server type:");
        Console.WriteLine("1. Server_Tls (Mutual Authentication)");
        Console.WriteLine("2. Simple Server (No Encryption)");
        Console.WriteLine("3. Valid Connection");
        Console.WriteLine("4. Publish Message From Broker");

        Console.Write("Enter any option (1, 2, 3, 4): ");

        if (int.TryParse(Console.ReadLine(), out int option))
        {
            switch (option)
            {
                case 1:
                    Mqtt_Server.Run_Server_With_Mutual_Authentication().GetAwaiter().GetResult();
                    break;
                case 2:
                    Mqtt_Server.Run_Minimal_Server().GetAwaiter().GetResult();
                    break;
                case 3:
                    Mqtt_Server.ValidMqttClient().GetAwaiter().GetResult();
                    break;
                
                default:
                    Console.WriteLine("Invalid option. Exiting...");
                    break;
            }
        }
        else
        {
            Console.WriteLine("Invalid input. Exiting...");
        }
    }
}