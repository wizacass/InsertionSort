using System;
using System.Collections;
using System.Collections.Generic;

public class MyLinkedList<T> : ILinkedList<T>, IEnumerable<T>
{
    public int Size { get; private set; }
    public bool IsEmpty => Size == 0;

    private Node _head;
    private Node _tail;

    public MyLinkedList()
    {
        Size = 0;
    }

    public void AddFirst(T element)
    {
        var node = new Node(element);
        if (IsEmpty)
        {
            _head = node;
            _tail = node;
        }
        else
        {
            node.Next = _head;
            _head.Previous = node;
            _head = node;
        }
        Size++;
    }

    public void AddLast(T element)
    {
        var node = new Node(element);
        if (IsEmpty)
        {
            _head = node;
            _tail = node;
        }
        else
        {
            node.Previous = _tail;
            _tail.Next = node;
            _tail = node;
        }
        Size++;
    }

    public bool Contains(T element)
    {
        if (IsEmpty) { return false; }
        foreach (var obj in this)
        {
            if (element.Equals(obj))
            {
                return true;
            }
        }
        return false;
    }

    public T First()
    {
        return _head.Data;
    }

    public T Last()
    {
        return _tail.Data;
    }

    public void Remove(T element)
    {
        if (IsEmpty) { return; }
        for (var node = _head; node != null; node = node.Next)
        {
            if (element.Equals(node.Data))
            {
                node.Previous.Next = node.Next;
                node.Next.Previous = node.Previous;
                node = null;
                Size--;
                return;
            }
        }
    }

    public void sort()
    {
        if (IsEmpty) { return; }
        throw new NotImplementedException();
    }

    public T[] ToArray()
    {
        if (IsEmpty) { return null; }
        var array = new T[Size];
        int i = 0;
        foreach (var obj in this)
        {
            array[i++] = obj;
        }
        return array;
    }

    public IEnumerator<T> GetEnumerator()
    {
        for (var node = _head; node != null; node = node.Next)
        {
            yield return node.Data;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private sealed class Node
    {
        public T Data { get; }
        public Node Previous { get; set; }
        public Node Next { get; set; }

        public Node(T data)
        {
            Data = data;
            Previous = null;
            Next = null;
        }
    }
}
