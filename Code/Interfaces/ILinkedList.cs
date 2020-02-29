public interface ILinkedList<T>
{
    T[] ToArray();

    void sort();

    ILinkedList<T> sorted();
}
