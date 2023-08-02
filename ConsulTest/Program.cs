using Consul;

var serviceName = "ConsulTestApi";

var exitRequested = false;

Console.WriteLine("Starting Consul test App...");
using (var consulClient = new ConsulClient())
{
    while (!exitRequested)
    {
        // Get available instances of the service
        var queryResult = await consulClient.Catalog.Service(serviceName);
        var services = queryResult.Response;
        if (services != null)
        {
            // Choose one of the available instances
            CatalogService service;
            if (services.Length > 1)
            {
                var _random = new Random();
                service = queryResult.Response[_random.Next(0, services.Length)];
            }
            else
            {
                service = queryResult.Response.FirstOrDefault()!;
            }

            // Call the service using HttpClient
            Console.WriteLine($"------------------------");
            Console.WriteLine($"Sending request to {service.ServiceAddress}:{service.ServicePort}");
            var serviceUrl = $"{service.ServiceAddress}:{service.ServicePort}/api/ConsulTest";
            using var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync(serviceUrl);
            Console.WriteLine($"Response from {serviceName}: {response}");
            Console.WriteLine($"------------------------");
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