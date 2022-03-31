using System;
using System.Reflection;

namespace LBrodny.Decorator
{
    public interface IDecorateObject
    {
        void MethodCalling(MethodInfo methodInfo, object?[] args);
        void MethodCalled(MethodInfo methodInfo, object?[] args, object? result);
        void MethodThrewException(MethodInfo methodInfo, object?[] args, Exception exception);
    }
}
