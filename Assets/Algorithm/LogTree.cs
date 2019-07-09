using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogTree : MonoBehaviour {

    private Button btn;
    private TreeNode tree;
	// Use this for initialization
	void Start () {
        btn = GameObject.Find("Button").GetComponent<Button>();
        btn.onClick.AddListener(PreDebug);
        btn.onClick.AddListener(MidDebug);
        btn.onClick.AddListener(PostDebug);
	}

    void InitTree()
    {
        tree = new TreeNode();
        tree.value = 1;
        tree.rightNode = new TreeNode();
        tree.rightNode.value = 2;
        tree.leftNode = new TreeNode();
        tree.leftNode.value = 3;
    }

    void PreDebug()
    {
        InitTree();
        Debug.Log("前序遍历");
        Stack<TreeNode> stack = new Stack<TreeNode>();
        while (tree != null || stack.Count != 0)
        {
            while (tree != null)
            {
                Debug.Log(tree.value);
                stack.Push(tree);
                tree = tree.leftNode;
            }
            if (stack.Count != 0)
            {
                tree = stack.Pop();
                tree = tree.rightNode;
            }
        }
    }

    void MidDebug()
    {
        InitTree();
        Debug.Log("中序遍历");
        Stack<TreeNode> stack = new Stack<TreeNode>();
        while (tree != null || stack.Count != 0)
        {
            while (tree != null)
            {
                stack.Push(tree);
                tree = tree.leftNode;
            }
            if (stack.Count != 0)
            {
                tree = stack.Pop();
                Debug.Log(tree.value);
                tree = tree.rightNode;
            }
        }
    }

    void PostDebug()
    {
        InitTree();
        Debug.Log("后序遍历");
        Stack<TreeNode> stack = new Stack<TreeNode>();
        TreeNode parent = new TreeNode();
        stack.Push(tree);
        while (stack.Count != 0)
        {
            tree = stack.Peek();
            if ((tree.leftNode == null && tree.rightNode == null) || (parent != null && (parent == tree.leftNode || parent == tree.rightNode)))
            {
                Debug.Log(tree.value);
                parent = tree;
                tree = stack.Pop();
            }
            else
            {
                if (tree.rightNode != null)
                    stack.Push(tree.rightNode);
                if (tree.leftNode != null)
                    stack.Push(tree.leftNode); 
            }
        }
    }
}

public class TreeNode
{
    public int value;
    public TreeNode rightNode;
    public TreeNode leftNode;
}
