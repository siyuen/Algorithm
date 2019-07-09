using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 拉链法散列表
/// 碰撞处理:相同hash值得存在同一组的链表中
/// </summary>
public class SeparaterHashST<Key, Value>{

    //键值对数
    private int n;
    
    //散列表的大小
    private int m;

    //链表数组
    private SequentialSearchST<Key, Value>[] st;

    public SeparaterHashST()
    {
        //this(997);
    }

    public SeparaterHashST(int M)
    {
        //创建m条链表
        this.m = M;
        st = new SequentialSearchST<Key, Value>[M];
        for (int i = 0; i < m; i++)
            st[i] = new SequentialSearchST<Key, Value>();
    }

    private int Hash(Key key)
    {
        return (key.GetHashCode() & 0x7fffffff % m);
    }

    public Value Get(Key key)
    {
        return (Value)st[Hash(key)].Get(key);
    }

    public void Put(Key key, Value val)
    {
        st[Hash(key)].Put(key, val);
    }

    public void Display()
    {

    }
}


/// <summary>
/// 线性探测散列表
/// 碰撞处理:相同键则+1
/// </summary>
public class LinearHashST<Key, Value>
{
    private int n;
    private int m = 16;
    private Key[] keys;
    private Value[] vals;

    public LinearHashST()
    {
        keys = new Key[m];
        vals = new Value[m];
    }

    public LinearHashST(int cap)
    {
        keys = new Key[cap];
        vals = new Value[cap];
        m = cap;
    }

    private int Hash(Key key)
    {
        return (key.GetHashCode() & 0x7fffffff % m);
    }

    public void Put(Key key, Value val)
    {
        if (n > m / 2)
            Resize(2 * m);

        int i;
        for (i = Hash(key); keys[i] != null; i = (i + 1) % m)
        {
            if (keys[i].Equals(key))
            {
                vals[i] = val;
                return;
            }
        }

        keys[i] = key;
        vals[i] = val;
        n++;
    }

    public Value Get(Key key)
    {
        for (int i = Hash(key); keys[i] != null; i = (i + 1) % m)
        {
            if (keys[i].Equals(key))
            {
               return vals[i];
            }
        }

        return default(Value);
    }

    /// <summary>
    /// 删除键,单单置空会导致找不到之后的键簇
    /// 把删除键之后的键簇置空重新put
    /// </summary>
    public void Delete(Key key)
    {
        int i = Hash(key);
        while (!key.Equals(keys[i]))
            i = (i + 1) % m;
        keys[i] = default(Key);
        vals[i] = default(Value);

        i = (i + 1) % m;
        while (keys[i] != null)
        {
            Key k = keys[i];
            Value v = vals[i];
            keys[i] = default(Key);
            vals[i] = default(Value);

            n--;
            Put(k, v);
            i = (i + 1) % m;
        }
        n--;
        if (n > 0 && n < m / 8)
            Resize(m / 2);
    }


    private void Resize(int cap)
    {
        LinearHashST<Key, Value> t = new LinearHashST<Key, Value>(cap);

        for (int i = 0; i < m; i++)
            if (keys[i] != null)
                t.Put(keys[i], vals[i]);
        keys = t.keys;
        vals = t.vals;
        m = t.m;
    }
}