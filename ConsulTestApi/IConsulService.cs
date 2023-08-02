namespace ConsulTestApi;

public interface IConsulService
{
    Task StartService();
    Task StopService();
}
