using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 链表实现队列
/// </summary>
public class ListQueue<T> {
    private class Node<T>
    {
        public T t;
        public Node<T> next;
    }
    private Node<T> first;
    private Node<T> last;
    private int n;

    public bool IsEmpty() { return n == 0; }
    public int Size() { return n; }
    public void Enqueue(T t)
    {
        Node<T> oldlast = last;
        last = new Node<T>();
        last.t = t;
        last.next = null;
        if (IsEmpty())
            first = last;
        else
            oldlast.next = last;
        n++;
    }

    public T Dequeue()
    {
        if (IsEmpty())
            return default(T);
        Node<T> oldfirst = first;
        first = first.next;
        if (IsEmpty())
            last = null;
        n--;
        return oldfirst.t;
    }

    public void Iterator()
    {
        Node<T> realfirst = first;
        while (first != null)
        {
            Debug.Log(first.t);
            first = first.next;
        }
        first = realfirst;
    }
}

/// <summary>
/// 数组实现队列
/// </summary>
public class ResizeQueue<T>
{
    private T[] a = new T[1];
    private int n = 0;

    public bool IsEmpty() { return n == 0; }
    public void Resize(int length)
    {
        T[] t = new T[length];
        for (int i = 0; i < n; i++)
            t[i] = a[i];
        a = t;

    }
    public void Enqueue(T t)
    {
        if (n == a.Length)
            Resize(a.Length * 2);
        a[n++] = t;
    }
    public T Dequeue()
    {
        if (IsEmpty())
            return default(T);
        T t = a[0];
        for (int i = 0; i < n - 1; i++)
        {
            a[i] = a[i + 1];
        }
        n--;
        if (n == a.Length / 4 && n > 0)
            Resize(a.Length / 2);
        return t;
    }
    public void Iterator()
    {
        for (int i = 0; i < n; i++)
        {
            Debug.Log(a[i]);
        }
    }
}

/// <summary>
/// 链表实现背包
/// </summary>
public class ListBag<T>
{ 
    public BagNode<T> first;
    private int n;

    public bool IsEmpty() { return first == null; }
    public int Size() { return n; }
    public void Add(T t)
    {
        BagNode<T> oldFirst = first;
        first = new BagNode<T>();
        first.t = t;
        first.next = oldFirst;
        n++;
    }

    public void Iterator()
    {
        BagNode<T> realfirst = first;
        while (first != null)
        {
            BagNode<T> oldfirst = first;
            first = first.next;
            Debug.Log(oldfirst.t);
        }
        first = realfirst;
    }
}

public class BagNode<T>
{
    public T t;
    public BagNode<T> next;
}
