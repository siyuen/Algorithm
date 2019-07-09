using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{


}

public class DoubleNode<T>
{
    public T t;
    public DoubleNode<T> last;
    public DoubleNode<T> next;

    public void InsertHead(DoubleNode<T> head)
    {
        last = head;
        head.next = this;
    }
    public void InsertEnd(DoubleNode<T> end)
    {
        next = end;
        end.last = this;
    }
    public void DeleteHead()
    {
        if (last == null)
            return;
        last.next = null;
        last = null;
    }
    public void DeleteEnd()
    {
        if (next == null)
            return;
        next.last = null;
        next = null;
    }
}
