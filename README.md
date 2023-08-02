# Consul Test Application
Experimenting with Consul by HashiCorp and .NET.
Simple api with a single get controller being called from a console app.

## Technologies used:
- [.NET 7.x](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)
- [Consul](https://www.consul.io/)
- [Visual Studio](https://visualstudio.microsoft.com/vs/)


## Major Libraries
- [Consul](https://www.nuget.org/packages/Consul)


## How to Run:
- Download [Consul](https://developer.hashicorp.com/consul/downloads) for your OS
- Add Consul to the path or open command at folder 
- Run `consul agent -dev`
- You can open the consul ui at http://localhost:8500
- Open 1 or more commands for ConsulTestApi
- Start each api with a different Port `dotnet run --urls https://localhost:{Port}`
- You will observe in the http://localhost:8500/ui/dc1/services there's a ConsulTestApi with the number of instance you created
- Open another command for ConsulTest and run `dotnet run`
- Press any button and observe that the app choices a random instance to call


## Notes
Added health check to the api so consul can check every 30 seconds if the api still active and if not mark as inactive.
After one minute it will remove the instace as well.
There are more then one way to get the service instance.
1.  `consulClient.Catalog.Service(serviceName);`
2. `consulClient.Health.Service(serviceName, tag: null, passingOnly: true);`

I opted to use the latter since this filters only healthy instances for us