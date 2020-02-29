public interface IDataFactory<T>
{
    T GenerateEntry();

    T[] GenerateEntries(int count);
}
