using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Algorithm : MonoBehaviour {

    private StopWatch timer;
	// Use this for initialization
	void Start () {
        //二分查找
        //int[] i = { 10, 20, 1, 2, 3, 4, 5, 6};
        //Debug.Log(Rank(3, i));

        //双栈算数表达式求值
        //string s = "(1+((2+3)*(4*5)))";
        //Debug.Log(Evaluate(s));

        //栈动态调整数组大小
        //ResizeStack<string> stack = new ResizeStack<string>();
        //stack.Push("n");
        //stack.Push("e");
        //stack.Push("u");
        //stack.Push("Y");
        //stack.Push("i");
        //stack.Push("S");
        //stack.Iterator();

        //栈的链表实现
        //ListStack<string> stack = new ListStack<string>();
        //stack.Push("n");
        //stack.Push("e");
        //stack.Push("u");
        //stack.Push("Y");
        //stack.Push("i");
        //stack.Push("S");
        //stack.Iterator();
        //Debug.Log(stack.Pop());

        //队列的链表实现
        //ListQueue<string> queue = new ListQueue<string>();
        //queue.Enqueue("S");
        //queue.Enqueue("i");
        //queue.Enqueue("Y");
        //queue.Enqueue("u");
        //queue.Enqueue("e");
        //queue.Enqueue("n");
        //queue.Iterator();

        //背包的链表实现
        //ListBag<string> bag = new ListBag<string>();
        //bag.Add("Yuen");
        //bag.Add("Si");
        //bag.Iterator();

        //队列的数组实现
        //ResizeQueue<string> queue = new ResizeQueue<string>();
        //queue.Enqueue("Si");
        //queue.Enqueue("Yu");
        //queue.Enqueue("en");
        //queue.Iterator();

        //timer = new StopWatch();
	}
	
	// Update is called once per frame
	void Update () {
	}

    /// <summary>
    /// 数组排序
    /// </summary>
    public static int[] Sort(int[] a)
    {
        for (int i = 0; i < a.Length - 1; i++)
        {
            for (int j = i + 1; j < a.Length; j++)
            {
                if (a[j] < a[i])
                {
                    int tem = a[i];
                    a[i] = a[j];
                    a[j] = tem;
                }
            }
        }
        return a;
    }

    /// <summary>
    /// 二分查找，有序数组查找
    /// </summary>
    public static int Rank(int key, int[] a)
    {
        a = Sort(a);
        int lo = 0;
        int hi = a.Length - 1;
        while (lo <= hi)
        {
            int mid = lo + (hi - lo) / 2;
            //print(mid);
            if (key < a[mid])
                hi = mid - 1;
            else if (key > a[mid])
                lo = mid + 1;
            else
                return mid;
        }
        return -1;
    }

    /// <summary>
    /// 利用栈求一个string形式算数表达式的值
    /// 一个stack存运算符，一个stack存值
    /// </summary>
    public static double Evaluate(string args)
    {
        Stack<char> ops = new Stack<char>();
        Stack<double> vals = new Stack<double>();
        int idx = 0;
        char[] s = args.ToCharArray();
        while (idx < args.Length)
        {
            char ss = s[idx];
            if (ss == '+')
                ops.Push(ss);
            else if (ss == '-')
                ops.Push(ss);
            else if (ss == '*')
                ops.Push(ss);
            else if (ss == '/')
                ops.Push(ss);
            else if (ss == ')')
            {
                //检测到右括号，一直出栈到右括号
                char sss = ops.Pop();
                double v = vals.Pop();
                if (sss == '+')
                    v = vals.Pop() + v;
                else if (sss == '-')
                    v = vals.Pop() - v;
                else if (sss == '*')
                    v = vals.Pop() * v;
                else if (sss == '/')
                    v = vals.Pop() / v;
                vals.Push(v);
            }
            else if(ss != '(')
                vals.Push(double.Parse(ss.ToString()));
            idx++;
        }
        return vals.Pop();
    }
}


   