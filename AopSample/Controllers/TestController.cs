using LBrodny.AOP;
using Microsoft.AspNetCore.Mvc;

namespace AopSample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ITestService _testService;

        public TestController(
            ITestService testService)
        {
            _testService = testService;
        }

        [HttpGet]
        public string Get()
        {
            var name = "John";

            ////var result = _testService.SayHello(name);

            var impl = ProxyFactory<ITestService>.Create(_testService);
            var result = impl.SayHello(name);

            return result;
        }
    }
}
