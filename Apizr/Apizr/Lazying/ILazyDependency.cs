namespace Apizr.Lazying
{
    public interface ILazyDependency<T>
    {
        T Value { get; }
    }
}
