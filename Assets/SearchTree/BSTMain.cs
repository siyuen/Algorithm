using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BSTMain : MonoBehaviour
{
    string[] k = { "b", "f", "a", "e", "d", "c", "d", "b", "b", "d" };
    int[] v = { 2, 8, 0, 6, 4, 3, 4, 2, 2, 4 };

    void Start()
    {
        //SearchBSTTest();
        //RedBlackBSTTest();
        SeparateHashSTTest();
    }

    void SeparateHashSTTest()
    {
    }

    void RedBlackBSTTest()
    {
        RedBlackBST<string, int> bst = new RedBlackBST<string, int>();
        for (int i = 0; i < k.Length; i++)
        {
            bst.Put(k[i], v[i]);
        }
        bst.Display();
        //Debug.Log("删除最小键");
        //bst.DelMin();
        //bst.Display();
        //Debug.Log("删除最大键");
        //bst.DelMax();
        //bst.Display();
        Debug.Log("删除d,4");
        bst.DelNode("d");
        bst.Display();
    }

    void SearchBSTTest()
    {
        SearchBST<string, int> bst = new SearchBST<string, int>();
        for (int i = 0; i < k.Length; i++)
        {
            bst.Put(k[i], v[i]);
        }
        //bst.Display();
        Debug.Log(bst.Root.val);
        Debug.Log(bst.Root.left.val + "," + bst.Root.right.val);
        Debug.Log("长度:" + bst.Size());
        Debug.Log("获取节点:" + bst.Get("c"));
        Debug.Log("最小key:" + bst.Min());
        Debug.Log("最大key:" + bst.Max());
        Debug.Log("向下取整:" + bst.Floor("g"));
        Debug.Log("向上取整:" + bst.Ceiling("b"));
        Debug.Log("select排名:" + bst.Select(4));
        Debug.Log("rank排名:" + bst.Rank("e"));
        bst.DeleteMin();
        Debug.Log("删除最小键");
        bst.Display();
        bst.DeleteMax();
        Debug.Log("删除最大键");
        bst.Display();
        bst.Delete("d");
        Debug.Log("删除指定键");
        bst.Display(bst.Root);
    }
}

/// <summary>
/// 二叉查找树
/// 每个节点存储键值对，键按照左子树根右子树排序,递归查找
/// </summary>
public class SearchBST<Key, Value> where Key : IComparable
{
    public class Node
    {
        public Key key;
        public Value val;
        public Node left;
        public Node right;
        public int n;

        public Node(Key key, Value val, int n)
        {
            this.key = key;
            this.val = val;
            this.n = n;
        }
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
                Debug.Log(tree.val);
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

    private Node root;

    public Node Root
    {
        get {return root;}
    }

    public int Size()
    {
        return size(root);
    }

    /// <summary>
    /// 每个节点挂载着计数器，每次添加的时候会更新相关节点的计数器
    /// </summary>
    private int size(Node x)
    {
        if (x == null)
            return 0;
        return x.n;
    }

    public Value Get(Key key)
    {
        return GetValue(key, root);
    }

    /// <summary>
    /// 相当于切分查找
    /// </summary>
    private Value GetValue(Key key, Node x)
    {
        if (x == null)
            return default(Value);
        int cmp = key.CompareTo(x.key);
        if (cmp > 0)
            return GetValue(key, x.right);
        else if (cmp < 0)
            return GetValue(key, x.left);
        else
            return x.val;
    }

    public void Put(Key key, Value val)
    {
        root = put(root, key, val);
    }

    private Node put(Node x, Key key, Value val)
    {
        if (x == null)
            return new Node(key, val, 1);
        int cmp = key.CompareTo(x.key);
        if (cmp < 0)
            x.left = put(x.left, key, val);
        else if (cmp > 0)
            x.right = put(x.right, key, val);
        else
            x.val = val;
        //reset每个节点上的计数器
        x.n = size(x.left) + size(x.right) + 1;
        return x;
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

    public Key Max()
    {
        return Max(root).key;
    }

    /// <summary>
    /// 如果存在右子树则递归找到最右，不然就返回当前根
    /// </summary>
    private Node Max(Node x)
    {
        if(x.right != null)
            return Max(x.right);
        else
            return x;
    }

    /// <summary>
    /// 向下取整
    /// </summary>
    public Key Floor(Key key)
    {
        Node x = Floor(root, key);
        if (x == null)
            return default(Key);
        return x.key;
    }

    /// <summary>
    /// 如果小于当前跟，则递归左子树寻找，大于当前根则递归右子树寻找
    /// </summary>
    private Node Floor(Node x, Key key)
    {
        if (x == null)
            return null;
        int cmp = key.CompareTo(x.key);
        if (cmp == 0)
            return x;
        else if (cmp < 0)
            return Floor(x.left, key);
        else
        {
            Node t = Floor(x.right, key);
            if (t == null)
                return x;
            else
                return t;
        }
    }

    /// <summary>
    /// 向上取整
    /// </summary>
    public Key Ceiling(Key key)
    {
        Node x = Ceiling(root, key);
        if (x == null)
            return default(Key);
        else
            return x.key;
    }

    /// <summary>
    /// 如果大于当前根，则递归右子树继续找，小于当前根则递归左子树继续寻找
    /// </summary>
    private Node Ceiling(Node x, Key key)
    {
        if (x == null)
            return null;
        int cmp = key.CompareTo(x.key);
        if (cmp == 0)
            return x;
        else if (cmp > 0)
            return Ceiling(x.right, key);
        else
        {
            Node t = Ceiling(x.left, key);
            if (t == null)
                return x;
            else
                return t;
        }
    }

    /// <summary>
    /// 找到排名为k的节点,有k个小于它的键
    /// </summary>
    public Key Select(int k)
    {
        return Select(root, k).key;
    }

    /// <summary>
    /// 判断左子树的长度
    /// </summary>
    private Node Select(Node x, int k)
    {
        if (x == null)
            return null;
        int t = size(x.left);
        if (t > k)
            return Select(x.left, k);
        else if (t == k)
            return x;
        else
            return Select(x.right, k - t - 1);
    }

    /// <summary>
    /// 查找key的排名
    /// </summary>
    public int Rank(Key key)
    {
        return Rank(root, key);
    }

    private int Rank(Node x, Key key)
    {
        if (x == null)
            return 0;
        int cmp = key.CompareTo(x.key);
        if (cmp == 0)
            return size(x.left);
        else if (cmp < 0)
            return Rank(x.left, key);
        else
            return size(x.left) + 1 + Rank(x.right, key);
    }

    public void DeleteMin()
    {
        root = DeleteMin(root);
    }

    /// <summary>
    /// 找到最左子数进行删除
    /// 情况一：无子树，不需要处理
    /// 情况二：右子树，需要将右子树指向上一个节点
    /// </summary>
    private Node DeleteMin(Node x)
    {
        if (x == null)
            return null;
        if (x.left == null)
            return x.right;
        x.left = DeleteMin(x.left);
        x.n = size(x.left) + size(x.right) + 1;
        return x;
    }

    public void DeleteMax()
    {
        root = DeleteMax(root);
    }

    private Node DeleteMax(Node x)
    {
        if (x == null)
            return null;
        if (x.right == null)
            return x.left;
        x.right = DeleteMax(x.right);
        x.n = size(x.left) + size(x.right) + 1;
        return x;
    }

    public void Delete(Key key)
    {
        root = Delete(root, key);
    }

    private Node Delete(Node x, Key key)
    {
        if (x == null)
            return null;
        int cmp = key.CompareTo(x.key);
        if (cmp < 0)
            x.left = Delete(x.left, key);
        else if (cmp > 0)
            x.right = Delete(x.right, key);
        else
        {
            if (x.right == null)
                return x.left;
            if (x.left == null)
                return x.right;
            Node t = x;
            x = Min(t.right);
            //右子树等于删除最小节点后的右子树，因为最小的称为新的根节点
            x.right = DeleteMin(t.right);
            x.left = t.left;
        }
        x.n = size(x.left) + size(x.right) + 1;
        return x;
    }

    /// <summary>
    /// 中序遍历
    /// </summary>
    public void Display(Node x)
    {
        if (x == null)
            return;
        Display(x.left);
        Debug.Log(x.val);
        Display(x.right);
    }
}
