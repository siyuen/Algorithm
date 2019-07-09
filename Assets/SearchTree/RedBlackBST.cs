using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RedBlackBST<Key, Value> where Key : IComparable
{

    private static bool RED = true;
    private static bool BLACK = false;
    private Node root;

    public Node Root { get { return root; } }

    public class Node
    {
        public Key key;
        public Value val;
        public Node left;
        public Node right;
        public int n;
        public bool color;

        public Node(Key key, Value val, int n, bool color)
        {
            this.key = key;
            this.val = val;
            this.n = n;
            this.color = color;
        }
    }

    public int Size()
    {
        return size(root);
    }

    private int size(Node x)
    {
        if (x == null)
            return 0;
        return x.n;
    }

    public bool IsRed(Node x)
    {
        if (x == null)
            return false;
        return x.color == RED;
    }

    /// <summary>
    /// 将红色右链接左旋
    /// </summary>
    /// <param name="h">根节点</param>
    /// <returns>包含同一组键的子树和红色左连接的根节点</returns>
    public Node RotateLeft(Node h)
    {
        Node x = h.right;
        h.right = x.left;
        x.left = h;
        x.color = h.color;
        h.color = RED;
        x.n = h.n;
        h.n = 1 + size(h.left) + size(h.right);

        return x;
    }

    public Node RotateRight(Node h)
    {
        Node x = h.left;
        h.left = x.right;
        x.right = h;
        x.color = h.color;
        h.color = RED;
        x.n = h.n;
        h.n = 1 + size(h.left) + size(h.right);

        return x;
    }

    /// <summary>
    /// 颜色转换，将两个子链接变为普通，根节点变为红
    /// </summary>
    public void FlipColors(Node h)
    {
        h.color = RED;
        h.right.color = BLACK;
        h.left.color = BLACK;
    }

    public void Put(Key key, Value val)
    {
        root = Put(root, key, val);
        root.color = BLACK;
    }

    private Node Put(Node h, Key key, Value val)
    {
        if (h == null)
            return new Node(key, val, 1, RED);

        int tmp = key.CompareTo(h.key);
        if (tmp < 0)
            h.left = Put(h.left, key, val);
        else if (tmp > 0)
            h.right = Put(h.right, key, val);
        else
            h.val = val;

        //右链接为红链接则左旋
        if (IsRed(h.right) && !IsRed(h.left))
            h = RotateLeft(h);

        //连续两条左红连接
        if (IsRed(h.left) && IsRed(h.left.left))
            h = RotateRight(h);

        //左右都为红则转换颜色
        if (IsRed(h.left) && IsRed(h.right))
            FlipColors(h);
        
        h.n = 1 + size(h.left) + size(h.right);
        return h;
    }

    public void Display()
    {
        if (root == null)
            return;
        Node tree = root;
        Stack<Node> stack = new Stack<Node>();
        while (tree != null || stack.Count != 0)
        {
            while (tree != null)
            {
                Debug.Log(tree.val + "," + tree.color);
                stack.Push(tree);
                tree = tree.left;
            }
            if (stack.Count != 0)
            {
                tree = stack.Pop();
                tree = tree.right;
            }
        }
    }

    private Node MoveRedLeft(Node h){//这个函数是用来处理2节点的
        if (h.left == null || h.right == null)
            return h;

        FlipColors(h);//把上面的节点"拉下来"，形成一个大节点
        if (IsRed(h.right.left)){//
            h.right = RotateRight(h.right);
            h = RotateLeft(h); //左旋将左连接变红,其实右链接不一定为红
            FlipColors(h);//注意！！！《算法4》书中这一章的习题中的代码缺少这一行，这一行代表借了一个节点之后，再还一个给父节点。否则我们就连着兄弟节点一起变成一个大节点了。
        }
        return h;
    }

    public void DelMin(){
        if (!IsRed(root.left) && !IsRed(root.right))
            root.color = RED;

        root = DelMin(root);
        if (root != null) 
            root.color = BLACK;
    }

    private Node DelMin(Node h)
    {
        if (h.left == null) 
            return null;

        if (!IsRed(h.left) && !IsRed(h.left.left))//意味着h的左子节点为一个2节点
            h = MoveRedLeft(h);
        h.left = DelMin(h.left);

        return Balance(h);
    }

    private Node Balance(Node h)
    {
        if (IsRed(h.right)) 
            h = RotateLeft(h);
        if (IsRed(h.right) && !IsRed(h.left)) 
            h = RotateLeft(h);
        if (IsRed(h.left) && IsRed(h.left.left))
            h = RotateLeft(h);
        if (IsRed(h.left) && IsRed(h.right)) 
            FlipColors(h);
        h.n = size(h.left) + size(h.right) + 1;
        return h;
    }

    private Node MoveRedRight(Node h)
    {
        if (h.left == null || h.right == null)
            return h;

        FlipColors(h);
        //做邻3-节点借
        if (IsRed(h.left.left))
        {
            h = RotateRight(h);
            FlipColors(h);
        }

        return h;
    }

    public void DelMax()
    {
        if (!IsRed(root.right) && !IsRed(root.left))
            root.color = RED;

        root = DelMax(root);
        if (root != null)
            root.color = BLACK;
    }

    private Node DelMax(Node h)
    {
        //左连接为红则右旋
        if (IsRed(h.left))
            h = RotateRight(h);//保证方向的一致性
        if (h.right == null)
            return null;//找到最右的节点就删除。

        if (!IsRed(h.right) && !IsRed(h.right.left))
            h = MoveRedRight(h);//右子节点为2节点的话就运行这个函数将其变成至少3节点
        //Debug.Log(h.val + "," + h.left.val + "," + h.left.color + "," + h.right.val);
        h.right = DelMax(h.right);
        return Balance(h);
    }

    public void DelNode(Key key)
    {
        if (!IsRed(root.right) && !IsRed(root.left))
            root.color = RED;
        root = DelNode(root, key);
        if (root != null) 
            root.color = BLACK;

    }

    private Node DelNode(Node h, Key key)
    {
        int tmp = key.CompareTo(h.key);

        if (tmp >= 0)
        {
            if (IsRed(h.left))
                h = RotateRight(h);

            if (key.CompareTo(h.key) == 0 && (h.right == null))
                return null;

            if (!IsRed(h.right) && !IsRed(h.right.left))
                h = MoveRedRight(h);

            if (key.CompareTo(h.key) == 0)
            {
                //获取右链接中的最小值赋给当前h，并删除那个最小的
                Node x = Min(h.right);
                h.key = x.key;
                h.val = x.val;
                h.right = DelMin(h.right);
            }
            else 
                h.right = DelNode(h.right, key);

        }
        else if (tmp < 0)
        {
            if (!IsRed(h.left) && !IsRed(h.left.left))
                h = MoveRedLeft(h);
            h.left = DelNode(h.left, key);
        }
        
        return Balance(h);
    }

    public Key Min()
    {
        return Min(root).key;
    }

    /// <summary>
    /// 如果左子树为空，则根节点为最小key，否则就递归调用左子树
    /// </summary>
    private Node Min(Node x)
    {
        if (x.left == null)
            return x;
        else
            return Min(x.left);
    }
}
