using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreQueueMain : MonoBehaviour {
    private int[] a = { 2, 8, 0, 6, 4, 3, 4, 2, 2, 4 };
	// Use this for initialization
	void Start () {
        //ArrayTest();
        //SrotArrayTest();
        HeapTest();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// 二叉堆
    /// </summary>
    public void HeapTest()
    {
        MaxPQWithHeap queue = new MaxPQWithHeap(a);
        //queue.DisPlay();
        int max = queue.DeleteMax();
        //queue.DisPlay();
        //queue.Insert(9);
        //queue.Insert(7);
        //queue.Insert(11);
        //queue.DisPlay();
        //int ma = queue.DeleteMax();
        //queue.DisPlay();
    }

    /// <summary>
    /// 无序数组实现队列
    /// </summary>
    public void ArrayTest()
    {
        MaxPQWithArray queue = new MaxPQWithArray(a);
        queue.DisPlay();
        int max = queue.DeleteMax();
        queue.DisPlay();
        queue.Insert(9);
        queue.Insert(7);
        queue.Insert(11);
        queue.DisPlay();
        int ma = queue.DeleteMax();
        queue.DisPlay();
    }

    /// <summary>
    /// 有序
    /// </summary>
    public void SrotArrayTest()
    {
        MaxPQWithSortArray queue = new MaxPQWithSortArray(a);
        queue.DisPlay();
        queue.Insert(9);
        queue.DisPlay();
    }
}

/// <summary>
/// 二叉堆实现优先队列
/// 假设k为当前节点，则k/2为父节点，2k、2k+1为子节点
/// 默认起点为1,最后一个节点则为length
/// </summary>
public class MaxPQWithHeap
{
    private int[] queue = new int[10];
    private int length = 0;
    public MaxPQWithHeap(int[] a)
    {
        queue[0] = -1;
        while (a.Length > queue.Length / 2)
            queue = new int[queue.Length * 2];
        for (int i = 1; i <= a.Length; i++)
            queue[i] = a[i - 1];
        length = a.Length;
        //FirstSort();
        Sort();
        Queue2Array();
    }

    /// <summary>
    /// 将无序的数组构建成二叉堆
    /// 从下至上构建
    /// </summary>
    public void Sort()
    {
        if (IsEmpty())
            return;
        int n = length;
        int i = n / 2;
        //先从最后一个子二叉堆开始下沉
        while (i > 1)
        {
            int k = --i;
            int tem = queue[k];
            queue[k] = queue[1];
            queue[1] = tem;
            Sink(i);
        }
    }

    /// <summary>
    /// 将一个二叉堆，转换成排序完毕的数组
    /// </summary>
    public void Queue2Array()
    {
        //存有序数组
        int[] array = new int[length];
        int idx = array.Length - 1;
        int[] temQueue = new int[length + 1];
        for (int i = 0; i < length; i++)
            temQueue[i] = queue[i];
        int n = length;
        while (n > 1)
        {
            //将头尾交换
            int tem = temQueue[1];
            temQueue[1] = temQueue[n];
            temQueue[n] = tem;
            //放置到最后
            array[idx--] = temQueue[n];
            //Debug.Log(temQueue[n]);
            //调整堆
            int[] temp = temQueue;
            temQueue = new int[n--];
            for (int i = 0; i < temQueue.Length; i++)
                temQueue[i] = temp[i];
            temQueue = Sink(temQueue, 1);
        }
        for (int i = 0; i < array.Length; i++)
            Debug.Log(array[i]);
    }

    public int[] Sink(int[] a, int k)
    {
        int n = a.Length;
        while (2 * k < n)
        {
            int j = k * 2;
            if (j < n && j + 1 < n && a[j] < a[j + 1])
                j++;
            if (j < n && a[k] >= a[j])
                break;
            int tem = a[k];
            a[k] = a[j];
            a[j] = tem;
            k = j;
        }
        return a;
    }

    /// <summary>
    /// 初始化用的排序
    /// </summary>
    public void FirstSort()
    {
        if (IsEmpty() || GetSize() == 1)
            return;
        for (int i = 2; i <= length; i++)
        {
            for (int j = i; j > 1; j--)
            {
                if (queue[j] > queue[j - 1])
                {
                    int tem = queue[j];
                    queue[j] = queue[j - 1];
                    queue[j - 1] = tem;
                }
            }
        }
    }

    public bool IsEmpty()
    {
        return length == 0;
    }

    public int GetSize()
    {
        return length;
    }

    /// <summary>
    /// 堆的上浮
    /// </summary>
    public void Swim(int k)
    {
        while (k > 1 && queue[k / 2] < queue[k])
        {
            int tem = queue[k];
            queue[k] = queue[k / 2];
            queue[k / 2] = tem;
        }
    }

    /// <summary>
    /// 堆的下沉
    /// </summary>
    public void Sink(int k)
    {
        while (2 * k <= length)
        {
            int j = k * 2;
            if (j < length && queue[j] < queue[j + 1])
                j++;
            if (queue[k] >= queue[j])
                break;
            int tem = queue[k];
            queue[k] = queue[j];
            queue[j] = tem;
            k = j;
        }
    }

    public void Insert(int a)
    {
        queue[length++] = a;
        if (length > queue.Length / 2)
        {
            int[] tem = queue;
            queue = new int[queue.Length * 2];
            for (int i = 1; i <= length; i++)
                queue[i] = tem[i];
        }
        Swim(length);
    }

    public int GetMax()
    {
        if (IsEmpty())
            return 0;
        return queue[1];
    }

    public int DeleteMax()
    {
        int max = GetMax();
        queue[1] = queue[length];
        length--;
        Sink(1);
        return max;
    }

    /// <summary>
    /// 显示当前队列
    /// </summary>
    public void DisPlay()
    {
        if (IsEmpty())
            return;
        string qStr = queue[1].ToString();
        for (int i = 2; i <= GetSize(); i++)
            qStr += "," + queue[i].ToString();
        Debug.Log(qStr);
    }

    public void DisPlay(int[] a)
    {
        if (a.Length == 0)
            return;
        string qStr = a[1].ToString();
        for (int i = 2; i < a.Length; i++)
            qStr += "," + a[i].ToString();
        Debug.Log(qStr);
    }
}

/// <summary>
/// 无序数组实现优先队列
/// </summary>
public class MaxPQWithArray
{
    private int[] queue = new int[10];
    private int length = 0;

    public MaxPQWithArray(int[] a)
    {
        while (a.Length > queue.Length / 2)
            queue = new int[queue.Length * 2];
        for (int i = 0; i < a.Length; i++)
            queue[i] = a[i];
        length = a.Length;
    }

    /// <summary>
    /// 无序所以直接添加
    /// </summary>
    public void Insert(int v)
    {
        int[] tem = queue;
        queue[length++] = v;
        if (length >= queue.Length / 2)
            queue = new int[queue.Length * 2];
        for (int i = 0; i < tem.Length; i++)
            queue[i] = tem[i];
    }

    /// <summary>
    /// 最大元素的索引
    /// </summary>
    private int MaxIndex()
    {
        if (length == 0)
            return -1;
        int max = 0;
        for (int i = 1; i < queue.Length; i++)
            if (queue[i] > queue[max])
                max = i;
        return max;
    }

    /// <summary>
    /// 返回最大元素,-1代表队列为空
    /// </summary>
    public int Max()
    {
        if (length == 0)
            return -1;
        return queue[MaxIndex()];
    }

    /// <summary>
    /// 删除最大元素并返回
    /// </summary>
    public int DeleteMax()
    {
        if (IsEmpty())
            return -1;
        int max = Max();
        
        length--;
        //元素前移
        for (int i = MaxIndex(); i < length; i++)
            queue[i] = queue[i + 1];
        //size适配
        if (length <= queue.Length / 3)
        {
            int[] tem = queue;
            queue = new int[queue.Length / 2];
            for (int i = 0; i < queue.Length; i++)
                queue[i] = tem[i];
        }
        return max;
    }

    /// <summary>
    /// 是否为空
    /// </summary>
    /// <returns></returns>
    public bool IsEmpty()
    {
        if (GetSize() == 0)
            return true;
        else
            return false;
    }

    /// <summary>
    /// 返回元素个数
    /// </summary>
    public int GetSize()
    {
        return length;
    }

    /// <summary>
    /// 显示当前队列
    /// </summary>
    public void DisPlay()
    {
        if (IsEmpty())
            return;
        string qStr = queue[0].ToString();
        for (int i = 1; i < GetSize(); i++)
            qStr += "," + queue[i].ToString();
        Debug.Log(qStr);
    }
}

/// <summary>
/// 有序数组实现优先队列
/// </summary>
public class MaxPQWithSortArray
{
    private int[] queue = new int[10];
    private int length;
    int g = 0;

    public MaxPQWithSortArray(int[] a)
    {
        while (a.Length > queue.Length / 2)
            queue = new int[queue.Length * 2];
        for (int i = 0; i < a.Length; i++)
            queue[i] = a[i];
        length = a.Length;
        FirstSort();
    }

    public bool IsEmpty()
    {
        return length == 0;
    }

    public int GetSize()
    {
        return length;
    }

    /// <summary>
    /// 初始化用的排序
    /// </summary>
    public void FirstSort()
    {
        if (IsEmpty() || GetSize() == 1) 
            return;
        for (int i = 1; i < length; i++)
        {
            for (int j = i; j > 0; j--)
            {
                if (queue[j] < queue[j - 1])
                {
                    int tem = queue[j];
                    queue[j] = queue[j - 1];
                    queue[j - 1] = tem;
                }
            }
        }
    }

    /// <summary>
    /// 插入新元素后的排序,返回idx
    /// 因为是有序的，所以用二分查找的插排
    /// </summary>
    public int InsertSort(int a, int min, int max)
    {
        g++;
        if (g == 10) return 0;
        int mid = (min + max) / 2;
        if (min > max)
            return min;
        else
        {
            if (a > queue[mid])
                return InsertSort(a, mid + 1, max);
            else if (a < queue[mid])
                return InsertSort(a, min - 1, mid);
            else
                return mid;
        }
    }

    /// <summary>
    /// 插入时进行排序
    /// </summary>
    public void Insert(int a)
    {
        int idx = InsertSort(a, 0, length-1);
        for (int i = idx + 1; i < length; i++)
            queue[i] = queue[i - 1];
        queue[idx] = a;
        length++;
    }

    public int GetMax()
    {
        return queue[length - 1];
    }

    public int DeleteMax(int a)
    {
        int max = GetMax();
        length--;
        return GetMax();
    }

    /// <summary>
    /// 显示当前队列
    /// </summary>
    public void DisPlay()
    {
        if (IsEmpty())
            return;
        string qStr = queue[0].ToString();
        for (int i = 1; i < GetSize(); i++)
            qStr += "," + queue[i].ToString();
        Debug.Log(qStr);
    }
}