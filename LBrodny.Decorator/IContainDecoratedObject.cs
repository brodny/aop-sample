namespace LBrodny.Decorator
{
    internal interface IContainDecoratedObject<TDecorated>
    {
        TDecorated Decorated { get; }

        void SetDecorated(TDecorated decorated);
    }
}
