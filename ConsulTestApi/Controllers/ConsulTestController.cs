using Microsoft.AspNetCore.Mvc;

namespace ConsulTestApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ConsulTestController : ControllerBase
{


    [HttpGet]
    public string Get()
    {
        return "Consul test success";
    }
}
