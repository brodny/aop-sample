using System;
using System.Linq;
using System.Reflection;

#nullable enable

namespace LBrodny.AOP
{
    public class ProxyFactory<TDecorated> : DispatchProxy
        where TDecorated: class
    {
        private TDecorated? _decorated;

        public static TDecorated Create(TDecorated decorated)
        {
            if (decorated is null)
            {
                throw new ArgumentNullException(nameof(decorated));
            }

            object proxy = Create<TDecorated, ProxyFactory<TDecorated>>();

            if (proxy == null)
            {
                throw new InvalidOperationException();
            }

            ((ProxyFactory<TDecorated>)proxy).SetParameters(decorated);

            return (TDecorated)proxy;
        }

        protected override object? Invoke(MethodInfo targetMethod, object?[]? args)
        {
            if (targetMethod is null)
            {
                throw new ArgumentNullException(nameof(targetMethod));
            }

            args ??= new object?[0];

            var aopAttributes = targetMethod.GetCustomAttributes<AOPAttribute>(true);

            object? result;

            if (!aopAttributes.Any())
            {
                result = targetMethod.Invoke(_decorated, args);
                return result;
            }

            var attribute = aopAttributes.First();

            result = attribute.Execute(_decorated, targetMethod, args);
            return result;
        }

        private void SetParameters(TDecorated decorated)
        {
            _decorated = decorated ?? throw new ArgumentNullException(nameof(decorated));
        }
    }
}
