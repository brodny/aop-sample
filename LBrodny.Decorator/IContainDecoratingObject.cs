namespace LBrodny.Decorator
{
    public interface IContainDecoratingObject
    {
        IDecorateObject DecoratingObject { get; }

        void SetDecoratingObject(IDecorateObject decoratingObject);
    }
}
