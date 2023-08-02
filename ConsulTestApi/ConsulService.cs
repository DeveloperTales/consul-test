using Consul;

namespace ConsulTestApi;

public class ConsulService : IConsulService
{
    private readonly IConsulClient _client;
    private readonly IConfiguration _configuration;
    private readonly string _serviceId = Guid.NewGuid().ToString();
    private readonly string _serviceName = "ConsulTestApi";
    private string address = "https://localhost";
    private int port = 7076;

    public ConsulService(IConfiguration configuration)
    {
        _client = new ConsulClient();
        _configuration = configuration;
    }

    public async Task StartService()
    {
        if (_client != null)
        {
            var urls = _configuration.GetValue<string>("Urls");
            if (urls != null)
            {
                var url = urls.Split(';').FirstOrDefault();
                var splitIndex = url?.LastIndexOf(':');
                if (splitIndex != null) 
                {
                    address = url?[..splitIndex.Value].Trim();
                    var portString = url?[(splitIndex.Value + 1)..].Trim();
                    if (!string.IsNullOrWhiteSpace(portString))
                    {
                        port = int.Parse(portString);
                    }
                }                
            }

            var httpCheck = new AgentServiceCheck()
            {
                DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1),
                Interval = TimeSpan.FromSeconds(30),
                HTTP = $"{address}:{port}/healthz"
            };

            var agentReg = new AgentServiceRegistration()
            {
                Address = address,
                ID = _serviceId,
                Name = _serviceName,
                Port = port,
                Check = httpCheck
            };

            await _client.Agent.ServiceRegister(agentReg);
        }        
    }

    public async Task StopService() 
    {
        if (_client != null)
        {
            await _client.Agent.ServiceDeregister(_serviceId);
        }        
    }
}
