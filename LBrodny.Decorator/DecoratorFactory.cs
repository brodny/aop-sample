using System;
using System.Reflection;

namespace LBrodny.Decorator
{
    public class DecoratorFactory<TInterface> : DispatchProxy
    {
        public static TInterface Create(TInterface decorated)
        {
            throw new NotImplementedException();
        }

        protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
        {
            throw new NotImplementedException();
        }
    }
}
