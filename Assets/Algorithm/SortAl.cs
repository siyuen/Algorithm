using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SortAl : MonoBehaviour {
	// Use this for initialization
    public Button btn;

	void Start () {
        int[] a = { 2, 8, 0, 6, 4, 3, 4 };
        //Selection.Sort(a);
        //Insertion.Sort(a);
        //Shell.Sort(a);
        //Merge.Sort(a);
        //Merge.Down2UpSort(a);
        Quick3way.Sort(a);
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void Test()
    {
        Debug.Log(Time.time);
        float t1 = SortCompare.TimeRandomInput("Selection", 10, 20);
        float t2 = SortCompare.TimeRandomInput("Insertion", 10, 20);
        Debug.Log(Time.time);
    }

}

/// <summary>
/// 算法比较
/// </summary>
public class SortCompare
{
    public static float TimeCount(string alg, int[] a)
    {
        float startTime = Time.time;
        if (alg.Equals("Insertion"))
            Insertion.Sort(a);
        else if (alg.Equals("Selection"))
            Selection.Sort(a);
        return Time.time - startTime;
    }

    public static float TimeRandomInput(string alg, int n, int T)
    {
        float total = 0;
        int[] a = new int[n];
        for (int t = 0; t < T; t++)
        {
            for (int i = 0; i < n; i++)
                a[i] = Random.Range(0, 1000);
            total += TimeCount(alg, a);
        }
        return total;
    }
}

public class Example
{
    /// <summary>
    /// b是否小于a
    /// </summary>
    public static bool Less(int a, int b)
    {
        return a.CompareTo(b) < 0;
    }

    public static void Exch(int[] a, int i, int j)
    {
        int t = a[i];
        a[i] = a[j];
        a[j] = t;
    }

    public static void Show(int[] a)
    {
        for (int i = 0; i < a.Length; i++)
            Debug.Log(a[i]);
    }

    public static bool IsSorted(int[] a)
    {
        for (int i = 1; i < a.Length; i++)
            if (Less(a[i], a[i - 1])) return false;
        return true;
    }
}

/// <summary>
/// 选择排序，N平方/2次比较和N次交换。数据移动少但是运行时间和输入无关
/// 找出最小的进行交换
/// </summary>
public class Selection
{
    public static void Sort(int[] a)
    {
        for (int i = 0; i < a.Length; i++)
        {
            int min = i;
            for (int j = i + 1; j < a.Length; j++)
                if (Example.Less(a[j], a[min])) min = j;
            Example.Exch(a, i, min);
        }
        //Example.Show(a);
    }
}

/// <summary>
/// 插入排序,如果数组相邻经常是无序的影响很大
/// 向右逐步排序
/// </summary>
public class Insertion
{
    public static void Sort(int[] a)
    {
        for (int i = 1; i < a.Length; i++)
        {
            //保证a数组在0~j这部分是有序的
            for (int j = i; j > 0 && Example.Less(a[j], a[j - 1]); j--)
                Example.Exch(a, j, j - 1);
        }
        //Example.Show(a);
    }
}

/// <summary>
/// 高级插排
/// 希尔排序,将array进行分组插排(0,h,2h这样进行分组,最后进行传统插排)
/// 其实就是先变成部分有序
/// </summary>
public class Shell
{
    public static void Sort(int[] a)
    {
        int h=1;
        while (h < a.Length / 3)
            h = 3 * h + 1;
        while (h >= 1)
        {
            for (int i = h; i < a.Length; i++)
            {
                for (int j = i; j >= h && Example.Less(a[j], a[j - h]); j -= h)
                    Example.Exch(a, j, j - h);
            }
            h = h / 3;
        }
        Example.Show(a);
    }
}

/// <summary>
/// 归并排序,将数组一分再分到极限再合并
/// 最后就是两个有序的数组进行合并
/// </summary>
public class Merge
{
    private static int[] aux;

    public static void Sort(int[] a)
    {
        aux = new int[a.Length];
        Sort(a, 0, a.Length - 1);
        Example.Show(a);
    }

    /// <summary>
    /// 自顶向下,两对小的合并后，在合并两个大的，再两个小的合并
    /// </summary>
    private static void Sort(int[] a, int lo, int hi)
    {
        if (hi <= lo) return;
        int mid = lo + (hi - lo) / 2;
        Sort(a, lo, mid);
        Sort(a, mid + 1, hi);
        ArrayMerge(a, lo, mid, hi);
    }

    /// <summary>
    /// 自低向上，两两合并，两两合并....再合并大的
    /// </summary>
    public static void Down2UpSort(int[] a)
    {
        int n = a.Length;
        aux = new int[a.Length];
        //sz理解成间隔，先合并间隔为1的，合并一轮后合并间隔为2的
        //或者理解成合并个数，第一轮11合并，第二轮两两合并
        for (int sz = 1; sz < n; sz = sz + sz)
        {
            for (int lo = 0; lo < n - sz; lo += sz + sz)
            {
                ArrayMerge(a, lo, lo + sz - 1, Mathf.Min(lo + sz + sz - 1, n - 1));
            }
        }
        Example.Show(a);
    }

    /// <summary>
    /// 合并,使用另外的空间存储原数据
    /// </summary>
    public static void ArrayMerge(int[] a, int lo, int mid, int hi)
    {
        int i = lo;
        int j = mid + 1;

        for (int k = lo; k <= hi; k++)
            aux[k] = a[k];

        for (int k = lo; k <= hi; k++)
        {
            if (i > mid)
                a[k] = aux[j++];
            else if (j > hi)
                a[k] = aux[i++];
            else if (aux[j] < aux[i])
                a[k] = aux[j++];
            else
                a[k] = aux[i++];
        }   
    }
}

/// <summary>
/// 快速排序，分成两部分分别排序再合并
/// 在元素相同时也会进行处理
/// </summary>
public class Quick
{
    public static void Sort(int[] a)
    {
        Sort(a, 0, a.Length - 1);
        Example.Show(a);
    }

    private static void Sort(int[] a, int lo, int hi)
    {
        if (hi <= lo)
            return;
        int j = Partition(a, lo, hi);
        //找到一轮后，j确定了位置（也就是之前的v）,根据j分为两部分继续sort
        Sort(a, lo, j - 1);
        Sort(a, j + 1, hi);
    }

    private static int Partition(int[] a, int lo, int hi)
    {
        int i = lo;
        int j = hi;
        //切分
        int v = a[lo];
        while (true)
        {
            //两端扫描
            //寻找大于v的
            //while (Example.Less(a[++i], v))
            //{
            //    //处理边界
            //    if (i == hi) 
            //        break;
            //}
            while (i < hi)
            {
                i++;
                if(Example.Less(v, a[i]))
                    break;
            }
            //寻找小于v的
            //while (Example.Less(v, a[--j]))
            //{
            //    if (j == lo) break;
            //}
            while (j > lo)
            {
                if (Example.Less(a[j], v))
                    break;
                else
                    j--;
            }
            if (i >= j) break;
            //Debug.Log("内交换:" + a[i] + "," + a[j]);
            int t = a[i];
            a[i] = a[j];
            a[j] = t;
        }
        //Debug.Log("切分:" + a[lo] + "," + a[j]);
        int tem = a[lo];
        a[lo] = a[j];
        a[j] = tem;
        return j;
    }
}

/// <summary>
/// 快速排序三切分
/// 处理了相同元素的情况，就不需要再进行递归
/// 
/// </summary>
public class Quick3way
{
    public static void Sort(int[] a)
    {
        Sort(a, 0, a.Length - 1);
        Example.Show(a);
    }

    private static void Sort(int[] a, int lo, int hi)
    {
        if (hi <= lo)
            return;
        int lt = lo, i = lo + 1, gt = hi;
        int v = a[lo];
        //区分三种情况进行切分
        //第一轮时区分为小于v，等于v和大于v的
        //结束时,[lo,lt-1]为小于v的，[lt,i-1]为等于v的，[gt+1/i, hi]为大于v的，gt记录边界
        while (i <= gt)
        {
            int cmp = a[i].CompareTo(v);
            //cmp<0是a[i]<v
            //小于则与lt交换
            if (cmp < 0)
                Example.Exch(a, lt++, i++);
            //大于则与最后一位进行交换
            else if (cmp > 0)
                Example.Exch(a, i, gt--);
            //等于则直接下一位
            else
                i++;
        }
        //排左边的序,也就是小于v的
        Sort(a, lo, lt - 1);
        //排右边的序，也就是大于v的
        //gt+1应该会等于i
        Sort(a, i, hi);
    }
}