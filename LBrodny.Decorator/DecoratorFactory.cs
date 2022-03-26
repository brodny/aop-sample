using System;
using System.Reflection;

namespace LBrodny.Decorator
{
    public class DecoratorFactory<TInterface> : DispatchProxy, IContainDecoratedObject<TInterface>
        where TInterface: class
    {
        static DecoratorFactory()
        {
            if (!typeof(TInterface).IsInterface)
            {
                throw new NotSupportedException($"Interface expected as a type parameter but '{typeof(TInterface).FullName}' provided instead.");
            }
        }

        public TInterface Decorated { get; private set; } = default!;

        public static TInterface Create(TInterface decorated)
        {
            if (decorated is null)
            {
                throw new ArgumentNullException(nameof(decorated));
            }

            TInterface proxiedInterface = DispatchProxy.Create<TInterface, DecoratorFactory<TInterface>>();

            ((IContainDecoratedObject<TInterface>)proxiedInterface).SetDecorated(decorated);

            return proxiedInterface;
        }

        public void SetDecorated(TInterface decorated)
        {
            if (decorated is null)
            {
                throw new ArgumentNullException(nameof(decorated));
            }

            Decorated = decorated;
        }

        protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
        {
            object? result = targetMethod?.Invoke(this.Decorated, args);

            return result;
        }
    }
}
