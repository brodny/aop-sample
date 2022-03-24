using System;
using System.Reflection;

namespace LBrodny.Decorator
{
    public class DecoratorFactory<TInterface> : DispatchProxy
    {
        public static TInterface Create(TInterface decorated)
        {
            if (decorated is null)
            {
                throw new ArgumentNullException(nameof(decorated));
            }

            throw new NotImplementedException();
        }

        protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
        {
            throw new NotImplementedException();
        }
    }
}
