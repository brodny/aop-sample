using System.Reflection;

#nullable enable

namespace LBrodny.AOP
{
    public abstract class AOPAttribute
    {
        public abstract object Execute(
            MethodInfo method,
            object[] args,
            object[] annotations);
    }
}
