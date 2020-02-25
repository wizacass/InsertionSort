using System;

public interface ILinkedList<T>
{
    public T[] ToArray();

    public void sort();

    public ILinkedList<T> sorted();
}
