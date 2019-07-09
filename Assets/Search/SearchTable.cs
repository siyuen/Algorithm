using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SearchTable : MonoBehaviour {
    string[] k = { "b", "f", "a", "e", "d", "c", "d", "b", "b", "d" };
    int[] v = { 2, 8, 0, 6, 4, 3, 4, 2, 2, 4 };

	// Use this for initialization
	void Start () {
        //SequentialTest();
        //BinaryTest();
        ArrayTest();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ArrayTest()
    {
        ArrayST<string, int> st = new ArrayST<string, int>(k, v);
        st.Display();
    }

    public void BinaryTest()
    {
        BinarySearchST<string, int> st = new BinarySearchST<string, int>(k, v);
        st.DisPlay();
    }

    public void SequentialTest()
    {
        SequentialSearchST<string, int> st = new SequentialSearchST<string, int>();
        for (int i = 0; i < v.Length; i++)
            st.Put(k[i], v[i]);
        st.DisPlay();
        st.Delete("b");
        st.DisPlay();
    }
}

/// <summary>
/// 无序数组实现符号表
/// 存一个结构体
/// </summary>
public class ArrayST<Key, Value>
{
    private class Node
    {
        public Key key;
        public Value value;

        public Node(Key key, Value val)
        {
            this.key = key;
            this.value = val;
        }
    }

    private Node[] stArray;
    private int length;

    public ArrayST(Key[] keys, Value[] vals)
    {
        stArray = new Node[keys.Length * 2];
        length = 0;
        for (int i = 0; i < keys.Length; i++)
            Put(keys[i], vals[i]);
    }

    public void Display()
    {
        string key = "";
        string val = "";
        for (int i = 0; i < length; i++)
        {
            key += stArray[i].key.ToString() + ",";
            val += stArray[i].value.ToString() + ",";
        }
        Debug.Log(key);
        Debug.Log(val);
    }

    public void Put(Key key, Value val)
    {
        if (!Get(key).Equals(default(Value)) || Get(key).Equals(val))
            return;
        
        Node node = new Node(key, val);
        stArray[length++] = node;
    }


    public Value Get(Key key)
    {
        for (int i = 0; i < length; i++)
            if (stArray[i].key.Equals(key))
                return stArray[i].value;
        return default(Value);
    }

    public bool Delete(Key key)
    {
        int del = -1;
        for (int i = 0; i < length; i++)
            if (stArray[i].key.Equals(key))
                del = i;
        if (del == -1)
            return false;
        for (int i = del; i < length - 1; i++)
            stArray[i] = stArray[i + 1];
        return true;
    }
}


/// <summary>
/// 有序数组中的二分查找
/// </summary>
public class BinarySearchST<Key, Value> where Key :IComparable
{
    //使用两个平行数组存储键值对
    private Key[] keys = new Key[10];
    private Value[] vals = new Value[10];
    private int n;

    public BinarySearchST(Key[] keys, Value[] vals)
    {
        while (keys.Length > this.keys.Length / 2)
            this.keys = new Key[this.keys.Length * 2];
        while (vals.Length > this.vals.Length / 2)
            this.vals = new Value[this.vals.Length * 2];
        for (int i = 0; i < vals.Length; i++)
            Put(keys[i], vals[i]);
    }

    public void DisPlay()
    {
        if (Size() == 0)
            return;
        string value = "";
        string key = "";
        for (int i = 0; i < n; i++)
        {
            key += keys[i].ToString() + ",";
            value += vals[i].ToString() + ",";
        }
        Debug.Log(key);
        Debug.Log(value);
        Debug.Log("Size:" + Size());
    }

    public int Size()
    {
        return n;
    }

    public Value Get(Key key)
    {
        if (Size() == 0)
            return default(Value);
        int i = Rank(key);
        if (i < n && keys[i].Equals(key))
            return vals[i];
        else
            return default(Value);
    }

    /// <summary>
    /// 返回小于给定键的数量，实则是排名
    /// 利用二分查找
    /// </summary>
    public int Rank(Key key)
    {
        int head = 0;
        int tail = n - 1;
        while (head <= tail)
        {
            int mid = (head + tail) / 2;
            int cmp = key.CompareTo(keys[mid]);
            if (cmp < 0)
                tail = mid - 1;
            else if (cmp > 0)
                head = mid + 1;
            else
                return mid;
        }
        return head;
    }

    public void Put(Key key, Value val)
    {
        int i = Rank(key);
        if (i < n && keys[i].Equals(key))
        {
            vals[i] = val;
            return;
        }   
        //创建新的元素，将大的元素后移
        for (int j = n; j > i; j--)
        {
            keys[j] = keys[j - 1];
            vals[j] = vals[j - 1];
        }
        keys[i] = key;
        vals[i] = val;
        n++;
    }

    public Value Delete(Key key)
    {
        if (Size() == 0)
            return default(Value);
        int i = Rank(key);
        if (i >= n)
            return default(Value);
        Value v = vals[i];
        if (i < n && keys[i].Equals(key))
        {
            for (int j = i; j < n; j++)
            {
                keys[j] = keys[j + 1];
                vals[j] = vals[j + 1];
            }
        }
        n--;
        return v;
    }
}


/// <summary>
/// 无序链表实现符号表，按照顺序查找
/// 插入跟删除都需要n次比较
/// </summary>
public class SequentialSearchST<Key, Value>
{
    private Node first;

    private class Node
    {
        public Key key;
        public Value value;
        public Node next;

        public Node(Key key, Value val, Node next)
        {
            this.key = key;
            this.value = val;
            this.next = next;
        }
    }

    public void DisPlay()
    {
        if (Size() == 0)
            return;
        string value = "";
        string key = "";
        for (Node x = first; x != null; x = x.next)
        {
            key += x.key.ToString() + ",";
            value += x.value.ToString() + ",";
        }
        Debug.Log(key);
        Debug.Log(value);
        Debug.Log("Size:" + Size());
    }

    public Value Get(Key key)
    {
        for (Node x = first; x != null; x = x.next)
            if (key.Equals(key))
                return x.value;
        return default(Value);
    }

    public void Put(Key key, Value value)
    {
        for(Node x = first;x != null;x = x.next)
            if (key.Equals(x.key))
            { x.value = value; return; }
        first = new Node(key, value, first);
    }

    public Value Delete(Key key)
    {
        if (Size() == 0)
            return default(Value);
        Node last = first;
        //处理第一个
        if (key.Equals(first.key))
        {
            first = first.next;
            return last.value;
        }

        for (Node x = first; x != null; x = x.next)
        {
            if(key.Equals(x.key))
            {
                Value val = x.value;
                if (x.next != null)
                    last.next = x.next;
                else
                    last.next = null;
                return val;
            }
            last = x;
        }
        return default(Value);
    }

    /// <summary>
    /// 键值对数量
    /// </summary>
    public int Size()
    {
        if (first == null)
            return 0;
        int count = 0;
        for (Node x = first; x != null; x = x.next)
            if (x.value != null)
                count++;
        return count;
    }

    /// <summary>
    /// 范围内键值对数量
    /// </summary>
    public int Size(Key head, Key tail)
    {
        if (first == null)
            return 0;
        int count = 0;
        bool start = false;
        for (Node x = first; x != null; x = x.next)
        {
            if (head.Equals(x.key))
                start = true;
            if (start)
                count++;
            if (tail.Equals(x.key))
                break;
        }
        return count;
    }
}