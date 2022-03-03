using System;
using System.Diagnostics;
using System.Reflection;

#nullable enable

namespace LBrodny.AOP
{
    public class PerformanceLoggerAttribute : AOPAttribute
    {
        public override object? Execute(object? target, MethodInfo method, object?[] args)
        {
            Console.WriteLine($"Starting measuring operation {method.Name}");

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var result = method.Invoke(target, args);

            stopwatch.Stop();

            Console.WriteLine($"Operation {method.Name} run for {stopwatch.ElapsedMilliseconds} ms");

            return result;
        }
    }
}
