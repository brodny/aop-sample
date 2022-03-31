using System;
using System.Reflection;

namespace LBrodny.Decorator
{
    public class DecoratorFactory<TInterface> : DispatchProxy,
        IContainDecoratedObject<TInterface>,
        IContainDecoratingObject
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

        public IDecorateObject DecoratingObject { get; private set; } = default!;

        public static TInterface Create(
            TInterface decorated,
            IDecorateObject decoratingObject)
        {
            if (decorated is null)
            {
                throw new ArgumentNullException(nameof(decorated));
            }

            if (decoratingObject is null)
            {
                throw new ArgumentNullException(nameof(decoratingObject));
            }

            TInterface proxiedInterface = DispatchProxy.Create<TInterface, DecoratorFactory<TInterface>>();

            ((IContainDecoratedObject<TInterface>)proxiedInterface).SetDecorated(decorated);
            ((IContainDecoratingObject)proxiedInterface).SetDecoratingObject(decoratingObject);

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

        public void SetDecoratingObject(IDecorateObject decoratingObject)
        {
            if (decoratingObject is null)
            {
                throw new ArgumentNullException(nameof(decoratingObject));
            }

            DecoratingObject = decoratingObject;
        }

        protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
        {
            object? result = targetMethod?.Invoke(this.Decorated, args);

            return result;
        }
    }
}
