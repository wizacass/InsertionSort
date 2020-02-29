public class DataGenerator<T>
{
    private readonly IDataManager<T> _dataManager;
    private readonly IDataFactory<T> _factory;

    public DataGenerator(IDataManager<T> manager, IDataFactory<T> factory)
    {
        _dataManager = manager;
        _factory = factory;
    }

    public void Generate(int count, string filename)
    {
        var entries = _factory.GenerateEntries(count);
        _dataManager.Write(entries, filename);
    }
}
