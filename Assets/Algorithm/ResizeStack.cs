using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 用数组控制大小的栈
/// </summary>
public class ResizeStack<T> {

    private T[] a = new T[1];
    private int n = 0;

    public bool IsEmpty() { return n == 0; }

    private void Resize(int max)
    {
        T[] temp = new T[max];
        for (int i = 0; i < n; i++)
            temp[i] = a[i];
        a = temp;
    }

    public void Push(T t)
    {
        if (n == a.Length) 
            Resize(2 * a.Length);
        a[n++] = t;
    }

    public T Pop()
    {
        T t = a[--n];
        a[n] = default(T);
        if (n > 0 && n == a.Length / 4)
            Resize(a.Length / 2);
        return t;
    }

    public void Iterator()
    {
        for (int i = n - 1; i >= 0; i--)
            Debug.Log(a[i]);
    }
}

/// <summary>
/// 链表实现栈
/// </summary>
public class ListStack<T>
{
    //栈顶
    private Node<T> first;
    //元素数量
    private int n;
    //节点
    private class Node<T>
    {
        public T t;
        public Node<T> next;
    }

    public bool IsEmpty() { return n == 0; }
    public int Size() { return n; }
    public void Push(T t)
    {
        Node<T> oldFirst = first;
        first = new Node<T>();
        first.t = t;
        first.next = oldFirst;
        n++;
    }
    public T Pop()
    {
        T t = first.t;
        first = first.next;
        n--;
        return t;
    }
    public T Peek()
    {
        T t = first.t;
        return t;
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