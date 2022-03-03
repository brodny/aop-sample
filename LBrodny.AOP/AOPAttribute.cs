using System;
using System.Reflection;

#nullable enable

namespace LBrodny.AOP
{
    [AttributeUsage(AttributeTargets.Method)]
    public abstract class AOPAttribute : Attribute
    {
        public abstract object? Execute(
            object? target,
            MethodInfo method,
            object?[] args);
    }
}
