using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphMain : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //DFSGraphTest();
        //BFSTest();
        //CCTest();
        //DigraphTest();
        MinTreeTest();
	}

    void MinTreeTest()
    {
        EdgeWeightedGraph g = new EdgeWeightedGraph(5);
        g.AddEdge(new Edge(0, 1, 1));
        g.AddEdge(new Edge(1, 2, 2));
        g.AddEdge(new Edge(1, 3, 6));
        g.AddEdge(new Edge(2, 4, 4));
        g.AddEdge(new Edge(3, 4, 5));

        LazyPrimMST mst = new LazyPrimMST(g);
        //Debug.Log("延迟:" + mst.Weight());

        PrimMST primMst = new PrimMST(g);
        //Debug.Log("即时:" + primMst.Weight());

        KruskalMST kruskalMst = new KruskalMST(g);
        Debug.Log(kruskalMst.Weight());
    }

    void DigraphTest()
    {
        Digraph r = new Digraph(7);
        r.AddEdge(0, 1);
        r.AddEdge(1, 2);
        r.AddEdge(3, 1);
        r.AddEdge(1, 4);
        r.AddEdge(4, 5);
        r.AddEdge(4, 6);
        r.AddEdge(6, 0);

        //r.Display();
        Digraph reverse = r.Reverse();
        //reverse.Display();

        DirectedDFS d = new DirectedDFS(r, 1);
        //d.Display();

        DirectedCycle c = new DirectedCycle(r);
        //Debug.Log(c.HasCycle());
        //c.Cycle();

        //DepthFirstOrder o = new DepthFirstOrder(r);
        //o.Pre();

        KosarajuSCC scc = new KosarajuSCC(r);
        Debug.Log(scc.Connected(0, 4));
    }

    void CCTest()
    {
        Graph g = new Graph(13);
        g.AddEdge(0, 1);
        g.AddEdge(0, 2);
        g.AddEdge(0, 5);
        g.AddEdge(0, 6);
        g.AddEdge(1, 0);
        g.AddEdge(2, 0);
        g.AddEdge(3, 5);
        g.AddEdge(3, 4);
        g.AddEdge(4, 3);
        g.AddEdge(4, 5);
        g.AddEdge(4, 6);
        g.AddEdge(5, 0);
        g.AddEdge(5, 3);
        g.AddEdge(5, 4);
        g.AddEdge(6, 0);
        g.AddEdge(6, 4);
        g.AddEdge(7, 8);
        g.AddEdge(8, 7);
        g.AddEdge(9, 10);
        g.AddEdge(9, 11);
        g.AddEdge(9, 12);
        g.AddEdge(10, 9);
        g.AddEdge(11, 9);
        g.AddEdge(11, 12);
        g.AddEdge(12, 11);
        g.AddEdge(12, 9);
        //g.Display();

        CC c = new CC(g);
        c.Display();
    }

    void BFSTest()
    {
        Graph g = new Graph(8);
        g.AddEdge(0, 1);
        g.AddEdge(1, 0);
        g.AddEdge(1, 2);
        g.AddEdge(1, 4);
        g.AddEdge(2, 1);
        g.AddEdge(2, 3);
        g.AddEdge(2, 5);
        g.AddEdge(5, 2);
        g.AddEdge(5, 4);
        g.AddEdge(5, 6);
        g.AddEdge(3, 2);
        g.AddEdge(3, 6);
        g.AddEdge(6, 5);
        g.AddEdge(6, 3);
        g.AddEdge(4, 1);
        g.AddEdge(4, 7);
        g.AddEdge(4, 5);
        g.AddEdge(7, 4);
        //g.Display();

        BreadthFirstSearsh bfs = new BreadthFirstSearsh(g, 1);
        bfs.Display();

        bfs.PathTo(6);
    }

    void DFSGraphTest()
    {
        Graph g = new Graph(8);
        g.AddEdge(0, 1);
        //g.AddEdge(1, 0);
        g.AddEdge(1, 2);
        g.AddEdge(1, 4);
       // g.AddEdge(2, 1);
        g.AddEdge(2, 3);
        //g.AddEdge(2, 5);
        g.AddEdge(5, 2);
        //g.AddEdge(3, 2);
        g.AddEdge(3, 6);
        g.AddEdge(4, 5);
        g.AddEdge(3, 5);
        //g.AddEdge(6, 3);
        //g.AddEdge(4, 1);
        g.AddEdge(4, 7);
        //g.AddEdge(7, 4);
        //g.Display();
        //Debug.Log("边:" + g.E() + ",顶点:" + g.V());

        DepthFirstSearsh dfs = new DepthFirstSearsh(g, 1);
        //dfs.Display();

        //dfs.PathTo(3);

        //环测试
        //Debug.Log(dfs.HasCycle());

        //二分图测试
        Debug.Log(dfs.IsBipartite());
    }
}

/// <summary>
/// keuakLul最小生成树
/// 每次添加一个权值最小的边且不会形成环
/// </summary>
public class KruskalMST
{
    private List<Edge> mst;
    private List<Edge> pq;

    public KruskalMST(EdgeWeightedGraph g)
    {
        mst = new List<Edge>();
        pq = new List<Edge>();
        
        ListBag<Edge>[] list = g.Adj();

        for (int i = 0; i < list.Length; i++)
        {
            ListBag<Edge> e = list[i];
            BagNode<Edge> node = e.first;

            while (node != null)
            {
                //Debug.Log(node.t.Weight());
                pq.Add(node.t);
                node = node.next;
            }
        }

        //每次添加一条权值最小的边
        //多花一个数组空间检测是否形成环
        while (pq.Count != 0 && mst.Count < g.V() - 1)
        {
            int min = GetMinWeight();
           
            Edge e = pq[min];
            pq.RemoveAt(min);

            if (!Connected(e))
            {
                int v = e.Either();
                int w = e.Other(v);

                //Debug.Log(v + "," + w);

                mst.Add(e);
            }
        }
    }

    public double Weight()
    {
        double weight = 0;
        for (int i = 0; i < mst.Count; i++)
            weight += mst[i].Weight();
        return weight;
    }

    public bool Connected(Edge e)
    {
        for (int i = 0; i < mst.Count; i++)
        {
            int v1 = mst[i].Either();
            int w1 = mst[i].Other(v1);
            int v2 = e.Either();
            int w2 = e.Other(v2);

            if (v1 == v2 && w1 == w2)
                return true;
        }
        return false;
    }

    private int GetMinWeight()
    {
        int minIdx = 0;
        double min = 100;

        for (int i = 0; i < pq.Count; i++)
        {
            if (pq[i].Weight() < min)
            {
                minIdx = i;
                min = pq[i].Weight();
            }
        }

        return minIdx;
    }
}

/// <summary>
/// 即时最小生成树
/// 每次添加新的边时，更新list，只保存权值最小的边
/// </summary>
public class PrimMST
{
    private Edge[] edgeTo;
    private double[] distTo;
    private bool[] marked;
    private List<int> pq;
    private int length;
    private bool same;

    public PrimMST(EdgeWeightedGraph g)
    {
        edgeTo = new Edge[g.V()];
        distTo = new double[g.V()];
        marked = new bool[g.V()];

        for (int i = 0; i < g.V(); i++)
        {
            distTo[i] = 100;
        }

        pq = new List<int>();

        distTo[0] = 0;
        pq.Add(0);
        length = 1;

        while (pq.Count != 0)
        {
            same = false;
            Visit(g, pq[0]);
            pq.RemoveAt(0);
            length--;
        }
    }

    private void Visit(EdgeWeightedGraph g, int v)
    {
        marked[v] = true;

        ListBag<Edge> list = g.Adj()[v];
        BagNode<Edge> node = list.first;

        while (node != null)
        {
            int w = node.t.Other(v);

            if (!marked[w])
            {
                //更新权值最小的边
                if (node.t.Weight() < distTo[w])
                {
                    edgeTo[w] = node.t;
                    distTo[w] = node.t.Weight();

                    //记录最近的顶点
                    if (same)
                    {
                        pq[pq.Count - 1] = w;
                    }
                    else
                    {
                        same = true;
                        pq.Add(w);
                    }
                }
            }
            node = node.next;
        }
    }

    public double Weight()
    {
        double weight = 0;

        for (int i = 0; i < distTo.Length; i++)
            weight += distTo[i];

        return weight;
    }
}

/// <summary>
/// 延时最小生成树
/// 保存了所有的边，每次遍历取权值最小的边
/// </summary>
public class LazyPrimMST
{
    private bool[] marked;
    private List<Edge> mst;
    private List<Edge> pq;

    public LazyPrimMST(EdgeWeightedGraph g)
    {
        pq = new List<Edge>();
        marked = new bool[g.V()];
        mst = new List<Edge>();

        Visit(g, 0);

        while (pq.Count != 0)
        {
            //获取权重最小的边，添加未被访问过的节点
            Edge e = pq[GetMinWeight()];

            int v = e.Either();
            int w = e.Other(v);

            pq.RemoveAt(GetMinWeight());

            if (!marked[v])
            {
                Visit(g, v);
                mst.Add(e);
            }

            if (!marked[w])
            {
                Visit(g, w);
                mst.Add(e);
            }
            
        }
    }

    private int GetMinWeight()
    {
        int minIdx = 0;
        double min = 100;

        for (int i = 0; i < pq.Count; i++)
        {
            if (pq[i].Weight() < min)
            {
                minIdx = i;
                min = pq[i].Weight();
            }
        }

        return minIdx;
    }

    private void Visit(EdgeWeightedGraph g, int v)
    {
        marked[v] = true;

        ListBag<Edge> list = g.Adj()[v];
        BagNode<Edge> node = list.first;

        while (node != null)
        {
            if (!marked[node.t.Other(v)])
                pq.Add(node.t);
            node = node.next;
        }
    }

    public double Weight()
    {
        double weight = 0;

        for (int i = 0; i < mst.Count; i++)
            weight += mst[i].Weight();

        return weight;
    }

    public void Display()
    {

    }
}

/// <summary>
/// 带权无向图
/// </summary>
public class EdgeWeightedGraph
{
    private int v;
    private int e;
    private ListBag<Edge>[] adj;

    public EdgeWeightedGraph(int v)
    {
        this.v = v;
        this.e = 0;
        adj = new ListBag<Edge>[v];
        for (int i = 0; i < v; i++)
            adj[i] = new ListBag<Edge>();
    }

    /// <summary>
    /// list顶点表中记录所连接的边，所以会出现两次
    /// </summary>
    public void AddEdge(Edge e)
    {
        int v = e.Either();
        int w = e.Other(v);

        adj[v].Add(e);
        adj[w].Add(e);
        this.e++;
    }

    public int V()
    {
        return v;
    }

    public ListBag<Edge>[] Adj()
    {
        return adj;
    }
}

/// <summary>
/// 带权边
/// </summary>
public class Edge
{
    private int v;
    private int w;
    private double weight;

    public Edge(int v, int w, double weight)
    {
        this.v = v;
        this.w = w;
        this.weight = weight;
    }

    public double Weight()
    {
        return weight;
    }

    public int Either()
    {
        return v;
    }

    public int Other(int ver)
    {
        if (ver == v)
            return w;
        else
            return v;
    }

    public int CompareTo(Edge that)
    {
        if (this.Weight() < that.Weight())
            return -1;
        else if (this.Weight() > that.Weight())
            return 1;
        else
            return 0;
    }
}

/// <summary>
/// 有向图强连通分量算法
/// 逆序遍历跟顺序遍历如果都连通就是强连通
/// </summary>
public class KosarajuSCC
{
    private bool[] marked;
    private int[] id;
    private int count;

    public KosarajuSCC(Digraph g)
    {
        marked = new bool[g.V()];
        id = new int[g.V()];

        Digraph reverse = g.Reverse();

        //逆后续
        DepthFirstOrder order = new DepthFirstOrder(reverse);
        Stack<int> post = order.ReverstPost();
        
        while (post.Count != 0)
        {
            //逆序深度搜索
            int v = post.Pop();
            Debug.Log(v);
            if (!marked[v])
            {
                DFS(g, v);
                count++;
            }
        }
    }

    public void DFS(Digraph g, int v)
    {
        marked[v] = true;
        id[v] = count;

        ListBag<int> list = g.Adj(v);
        BagNode<int> node = list.first;

        while (node != null)
        {
            if (!marked[node.t])
                DFS(g, node.t);
            node = node.next;
        }
    }

    public bool Connected(int v, int w)
    {
        return id[v] == id[w];
    }
}

/// <summary>
/// 拓扑排序
/// 先判断有向环是否存在 再获得递归顺序的顶点排序
/// </summary>
public class Topological
{
    private List<int> order;

    public Topological(Digraph g)
    {
        DirectedCycle cycle = new DirectedCycle(g);

        if (!cycle.HasCycle())
        {
            DepthFirstOrder dfs = new DepthFirstOrder(g);

            order = dfs.Pre();
        }
    }

    public List<int> Order()
    {
        for (int i = 0; i < order.Count; i++)
            Debug.Log(order[i]);
        return order;
    }

    public bool IsDAG()
    {
        return order != null;
    }
}

/// <summary>
/// 有向图深度优先顶点排序
/// </summary>
public class DepthFirstOrder
{
    private bool[] marked;
    private List<int> pre;
    private List<int> post;
    //逆后续排列
    private Stack<int> reversePost;

    public DepthFirstOrder(Digraph g)
    {
        pre = new List<int>();
        post = new List<int>();
        reversePost = new Stack<int>();
        marked = new bool[g.V()];

        for (int i = 0; i < g.V(); i++)
            if (!marked[i])
                DFS(g, i);
    }

    private void DFS(Digraph g, int v)
    {
        pre.Add(v);
        marked[v] = true;

        ListBag<int> list = g.Adj(v);
        BagNode<int> node = list.first;

        while (node != null)
        {
            if (!marked[node.t])
                DFS(g, node.t);
            node = node.next;
        }

        post.Add(v);
        reversePost.Push(v);
    }

    public List<int> Pre()
    {
        for (int i = 0; i < pre.Count; i++)
            Debug.Log(pre[i]);

        return pre;
    }

    public List<int> Post()
    {
        for (int i = 0; i < post.Count; i++)
            Debug.Log(post[i]);

        return post;
    }

    public Stack<int> ReverstPost()
    {
        return reversePost;
    }
}

/// <summary>
/// 有向环检测
/// </summary>
public class DirectedCycle
{
    private bool[] marked;
    private int[] edgeTo;
    private List<int> cycle;
    private bool[] onStack;

    public DirectedCycle(Digraph g)
    {
        onStack = new bool[g.V()];
        edgeTo = new int[g.V()];
        marked = new bool[g.V()];
        for (int i = 0; i < g.V(); i++)
            if (!marked[i])
                DFS(g, i);
    }

    private void DFS(Digraph g, int v)
    {
        onStack[v] = true;
        marked[v] = true;

        ListBag<int> list = g.Adj(v);
        BagNode<int> node = list.first;

        //遍历v的有向路径
        while (node != null)
        {
            if (HasCycle())
                return;
            else if(!marked[node.t])
            {
                edgeTo[node.t] = v;
                DFS(g, node.t);
            }
            else if (onStack[node.t])
            {
                //onStack记录了当前这条路径中已经遍历过的顶点
                //所以重复的顶点肯定形成了有向环
                cycle = new List<int>();
                for (int i = v; i != node.t; i = edgeTo[i])
                    cycle.Add(i);

                cycle.Add(node.t);
                cycle.Add(v);
            }
        }
        onStack[v] = false;
    }

    public bool HasCycle()
    {
        return cycle != null;
    }

    public void Cycle()
    {
        for (int i = 0; i < cycle.Count; i++)
            Debug.Log(cycle[i]);
    }
}

/// <summary>
/// 深度搜索有向图
/// 解决有向图中顶点可达性问题
/// </summary>
public class DirectedDFS
{
    private bool[] marked;

    public DirectedDFS(Digraph g, int s)
    {
        marked = new bool[g.V()];
        DFS(g, s);
    }

    public DirectedDFS(Digraph g, ListBag<int> sources)
    {
        marked = new bool[g.V()];
        
    }

    private void DFS(Digraph g, int v)
    {
        marked[v] = true;

        ListBag<int> list = g.Adj(v);
        BagNode<int> node = list.first;

        while (node != null)
        {
            if (!marked[node.t])
                DFS(g, node.t);
            node = node.next;
        }
    }

    public bool Marked(int v)
    {
        return marked[v];
    }

    public void Display()
    {
        for (int i = 0; i < marked.Length; i++)
            Debug.Log(i + ":" + marked[i]);
    }
}

/// <summary>
/// 有向图
/// </summary>
public class Digraph
{
    private int v;
    private int e;

    private ListBag<int>[] adj;

    public Digraph(int v)
    {
        this.v = v;
        this.e = 0;

        adj = new ListBag<int>[v];
        for(int i=0;i<v;i++)
            adj[i] = new ListBag<int>();
    }

    public int V()
    {
        return v;
    }

    public int E()
    {
        return e;
    }

    /// <summary>
    /// 有向图只添加一个
    /// </summary>
    public void AddEdge(int v, int w)
    {
        adj[v].Add(w);
        e++;
    }

    public ListBag<int> Adj(int v)
    {
        return adj[v];
    }

    /// <summary>
    /// 有向图取反
    /// adj可以获取每个顶点所连向的点
    /// 取反后可以获取每个顶点被谁指向
    /// </summary>
    public Digraph Reverse()
    {
        Digraph r = new Digraph(v);
        for (int i = 0; i < v; i++)
        {
            ListBag<int> list = adj[i];
            BagNode<int> node = list.first;

            while (node != null)
            {
                //把值作为key添加进新的图里
                r.AddEdge(node.t, i);
                node = node.next;
            }
        }

        return r;
    }

    public void Display()
    {
        for (int i = 0; i < adj.Length; i++)
        {
            Debug.Log("索引:" + i);
            adj[i].Iterator();
        }
    }
}

/// <summary>
/// 基于领接表
/// 每条边会出现两次
/// </summary>
public class Graph
{
    //顶点数
    private int v;
    //边数
    private int e;

    /// <summary>
    /// 领接表
    /// 每个顶点维护一个保存了与其相邻的所有顶点的链表
    /// </summary>
    public ListBag<int>[] adj;

    public Graph(int v)
    {
        this.v = v;
        this.e = 0;

        adj = new ListBag<int>[v];
        for (int i = 0; i < v; i++)
            adj[i] = new ListBag<int>();
    }

    public int V()
    {
        return v;
    }

    public int E()
    {
        return e;
    }

    /// <summary>
    /// 添加边，两个顶点的链表中相互添加
    /// 会重复,链表也不好检测是否重复
    /// </summary>
    public void AddEdge(int v1, int v2)
    {
        adj[v1].Add(v2);
        adj[v2].Add(v1);
        e++;
    }

    public void Display()
    {
        for (int i = 0; i < adj.Length; i++)
        {
            Debug.Log("索引:" + i);
            adj[i].Iterator();
        }
    }
}

/// <summary>
/// 广度优先搜索
/// </summary>
public class BreadthFirstSearsh
{
    private bool[] marked;
    private int count;
    private int[] edgeTo;
    private int s;

    public BreadthFirstSearsh(Graph g, int v)
    {
        marked = new bool[g.V()];
        edgeTo = new int[g.V()];
        this.s = v;
        BFS(g, v);
    }

    private void BFS(Graph g, int v)
    {
        Queue<int> queue = new Queue<int>();
        marked[v] = true;
        queue.Enqueue(v);

        ListBag<int> adj = g.adj[v];
        BagNode<int> node = adj.first;

        while (queue.Count != 0)
        {
            int s = queue.Dequeue();
            Debug.Log("出队:" + s);
            //遍历周围的点
            while (node.next != null)
            {
                node = node.next;
                if (!marked[node.t])
                {
                    marked[node.t] = true;
                    edgeTo[node.t] = v;
                    BFS(g, node.t);
                    //queue.Enqueue(node.t);
                } 
            }
        }
    }

    public void PathTo(int v)
    {
        if (!Marked(v))
            return;

        Stack<int> path = new Stack<int>();
        for (int i = v; i != s; i = edgeTo[i])
            path.Push(i);
        path.Push(s);  //起点塞到第一个

        while (path.Count != 0)
        {
            Debug.Log("路径:" + path.Pop());
        }
    }

    public bool Marked(int v)
    {
        return marked[v];
    }

    public void Display()
    {
        for (int i = 0; i < marked.Length; i++)
            Debug.Log(i + ":" + marked[i]);
    }
}

/// <summary>
/// 深度优先搜索
/// </summary>
public class DepthFirstSearsh
{
    private bool[] marked;
    private int count;
    private int[] edgeTo;
    private int s;
    private bool hasCycle;

    //二分图
    private bool isTwoColor;
    private bool[] colors;

    public DepthFirstSearsh(Graph g, int v)
    {
        marked = new bool[g.V()];
        edgeTo = new int[g.V()];
        this.s = v;
        hasCycle = true;
        colors = new bool[g.V()];

        DFS(g, v, v);
    }

    /// <summary>
    /// 遍历该顶点的领接表
    /// 相邻的用marked=true
    /// </summary>
    private void DFS(Graph g, int v, int w)
    {
        marked[v] = true;
        count++;

        ListBag<int> adj = g.adj[v];
        BagNode<int> node = adj.first;

        if (node == null)
            return;

        
        while (node != null)
        {
            Debug.Log("下一个点:" + node.t);
            if (!marked[node.t])
            {
                //marked[node.t] = true;
                //count++;
                //一个点不会赋值两次 
                edgeTo[node.t] = v;

                //相邻两点颜色值不同
                colors[node.t] = !colors[v];

                DFS(g, node.t, v);
            }
            else if (node.t != w)
            {
                hasCycle = true;
            }

            if(colors[node.t] == colors[v])
            {
                isTwoColor = false;
            }
            node = node.next;
        }
    }

    public void Display()
    {
        for (int i = 0; i < marked.Length; i++)
            Debug.Log(i + ":" + marked[i]);
    }

    /// <summary>
    /// 深度优先搜索
    /// </summary>
    public void PathTo(int v)
    {
        if (!Marked(v))
            return;

        Stack<int> path = new Stack<int>();
        for (int i = v; i != s; i = edgeTo[i])
            path.Push(i);
        path.Push(s);  //起点塞到第一个

        while (path.Count != 0)
        {
            Debug.Log("路径:" + path.Pop());
        }
    }

    public bool Marked(int v)
    {
        return marked[v];
    }

    public int Count()
    {
        return count;
    }

    /// <summary>
    /// 是否是无环图
    /// </summary>
    public bool HasCycle()
    {
        return hasCycle;
    }

    /// <summary>
    /// 是否是二分图
    /// </summary>
    public bool IsBipartite()
    {
        return isTwoColor;
    }
}

/// <summary>
/// 连通分量（连通子图）
/// </summary>
public class CC
{
    //记录顶点
    private bool[] marked;
    //连通标识符
    private int[] id;
    //连通图idx
    private int count;

    public CC(Graph G)
    {
        marked = new bool[G.V()];
        id = new int[G.V()];

        for (int i = 0; i < G.V(); i++)
        {
            if (!marked[i])
            {
                //深度优先搜索 遍历所有连通子图
                DFS(G, i);
                count++;
            }
        }
    }

    private void DFS(Graph g, int v)
    {
        marked[v] = true;
        id[v] = count;

        ListBag<int> adj = g.adj[v];
        BagNode<int> node = adj.first;

        if (node == null)
            return;

        while (node.next != null)
        {
            node = node.next;
            if (!marked[node.t])
            {
                DFS(g, node.t);
            }
        }
    }

    public bool Connected(int v, int w)
    {
        return id[v] == id[w];
    }

    public int ID(int v)
    {
        return id[v];
    }

    public int Count()
    {
        return count;
    }

    public void Display()
    {
        for (int i = 0; i < id.Length; i++)
        {
            Debug.Log(i + ":" + id[i]);
        }
    }
}