#nullable enable

using LBrodny.AOP;
using System;

namespace AopSample
{
    public interface ITestService
    {
        [PerformanceLogger]
        string SayHello(string name);
    }

    public class TestService : ITestService
    {
        public string SayHello(string name)
        {
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            System.Threading.Thread.Sleep(2000);

            return $"Hello, {name}!";
        }
    }
}
