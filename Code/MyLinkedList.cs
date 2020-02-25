using System;
using System.Collections;
using System.Collections.Generic;

public class MyLinkedList<T> : ILinkedList<T>, IEnumerable<T>
{
    public IEnumerator<T> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    public void sort()
    {
        throw new NotImplementedException();
    }

    public ILinkedList<T> sorted()
    {
        throw new NotImplementedException();
    }

    public T[] ToArray()
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new NotImplementedException();
    }
}
