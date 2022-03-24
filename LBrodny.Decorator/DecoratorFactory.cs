using System;
using System.Reflection;

namespace LBrodny.Decorator
{
    public class DecoratorFactory<TInterface> : DispatchProxy
    {
        static DecoratorFactory()
        {
            if (!typeof(TInterface).IsInterface)
            {
                throw new NotSupportedException($"Interface expected as a type parameter but '{typeof(TInterface).FullName}' provided instead.");
            }
        }


        public static TInterface Create(TInterface decorated)
        {
            if (decorated is null)
            {
                throw new ArgumentNullException(nameof(decorated));
            }

            TInterface proxiedInterface = DispatchProxy.Create<TInterface, DecoratorFactory<TInterface>>();

            return proxiedInterface;
        }

        protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
        {
            throw new NotImplementedException();
        }
    }
}
