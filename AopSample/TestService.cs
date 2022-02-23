#nullable enable

using System;

namespace AopSample
{
    public interface ITestService
    {
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

            return $"Hello, {name}!";
        }
    }
}
