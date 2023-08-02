using Consul;

var serviceName = "ConsulTestApi";

var exitRequested = false;

Console.WriteLine("Starting Consul test App...");
using (var consulClient = new ConsulClient())
{
    while (!exitRequested)
    {
        // Get available instances of the service
        //var queryResult = await consulClient.Catalog.Service(serviceName);
        var queryResult = await consulClient.Health.Service(serviceName, tag: null, passingOnly: true);
        var services = queryResult.Response;       
        if (services != null && services.Length > 0)
        {
            // Choose one of the available instances
            AgentService service;
            if (services.Length > 1)
            {
                var _random = new Random();
                service = services[_random.Next(0, services.Length)].Service;
            }
            else
            {
                service = services.FirstOrDefault()!.Service;
            }

            // Call the service using HttpClient
            Console.WriteLine($"------------------------");
            Console.WriteLine($"Sending request to {service.Address}:{service.Port}");
            var serviceUrl = $"{service.Address}:{service.Port}/api/ConsulTest";
            using var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync(serviceUrl);
            Console.WriteLine($"Response from {serviceName}: {response}");
            Console.WriteLine($"------------------------");
        }
        else
        {
            Console.WriteLine("No healthy services found");
        }
    
        Console.WriteLine("Press Q key to exit or anykey to continue...");
        var key = Console.ReadKey(true);
        if (key.Key == ConsoleKey.Q)
        {
            exitRequested = true;
        }
    }
}

Console.WriteLine("Exiting the Consul test App...");