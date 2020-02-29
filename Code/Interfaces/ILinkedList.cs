public interface ILinkedList<T>
{
    T First();

    T Last();

    void AddFirst(T element);

    void AddLast(T element);

    void Remove(T element);

    bool Contains(T element);

    T[] ToArray();

    void sort();
}
