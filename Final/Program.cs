using System;
using System.Collections.Generic;

enum BagType
{
    Leaf,
    Introduce,
    Forget,
    Join
}

class Bag
{
    public int Id;
    public BagType Type;
    public List<int> Vertices = new List<int>();
    public int IntroducedVertex;
    public int ForgottenVertex;
    public Bag? Left;
    public Bag? Right;
}

class TreeDecompositionDP
{
    private Dictionary<int, Dictionary<int, int>> dp = new();
    private Dictionary<int, HashSet<int>> graph;

    public TreeDecompositionDP(Dictionary<int, HashSet<int>> graph)
    {
        this.graph = graph;
    }

    public int Solve(Bag root)
    {
        DFS(root);
        int ans = 0;
        foreach (var v in dp[root.Id].Values)
            ans = Math.Max(ans, v);
        return ans;
    }

    private void DFS(Bag bag)
    {
        if (bag.Type == BagType.Leaf)
        {
            dp[bag.Id] = new Dictionary<int, int> { { 0, 0 } };
            return;
        }

        if (bag.Type == BagType.Introduce)
        {
            DFS(bag.Left!);
            dp[bag.Id] = new Dictionary<int, int>();

            int v = bag.IntroducedVertex;
            int pos = bag.Vertices.IndexOf(v);

            foreach (var e in dp[bag.Left!.Id])
            {
                int mask = e.Key;
                int val = e.Value;

                // v not chosen
                dp[bag.Id][mask] =
                    Math.Max(dp[bag.Id].GetValueOrDefault(mask, 0), val);

                // v chosen
                if (CanSelect(v, mask, bag.Vertices))
                {
                    int newMask = mask | (1 << pos);
                    dp[bag.Id][newMask] =
                        Math.Max(dp[bag.Id].GetValueOrDefault(newMask, 0), val + 1);
                }
            }
        }
        else if (bag.Type == BagType.Forget)
        {
            DFS(bag.Left!);
            dp[bag.Id] = new Dictionary<int, int>();

            int pos = bag.Left!.Vertices.IndexOf(bag.ForgottenVertex);

            foreach (var e in dp[bag.Left.Id])
            {
                int newMask = e.Key & ~(1 << pos);
                dp[bag.Id][newMask] =
                    Math.Max(dp[bag.Id].GetValueOrDefault(newMask, 0), e.Value);
            }
        }
        else if (bag.Type == BagType.Join)
        {
            DFS(bag.Left!);
            DFS(bag.Right!);
            dp[bag.Id] = new Dictionary<int, int>();

            foreach (var l in dp[bag.Left!.Id])
            {
                int mask = l.Key;
                if (!dp[bag.Right!.Id].ContainsKey(mask)) continue;

                dp[bag.Id][mask] =
                    l.Value + dp[bag.Right.Id][mask] - CountBits(mask);
            }
        }
    }

    private bool CanSelect(int v, int mask, List<int> bagVertices)
    {
        for (int i = 0; i < bagVertices.Count; i++)
            if ((mask & (1 << i)) != 0 && graph[v].Contains(bagVertices[i]))
                return false;
        return true;
    }

    private int CountBits(int x)
    {
        int c = 0;
        while (x > 0) { c += x & 1; x >>= 1; }
        return c;
    }
}

class Program
{
    static void Main()
    {
        // Graph: 1 -- 2 -- 3
        var graph = new Dictionary<int, HashSet<int>>
        {
            {1, new HashSet<int>{2}},
            {2, new HashSet<int>{1,3}},
            {3, new HashSet<int>{2}}
        };

        // Manual NICE tree decomposition
        var leaf = new Bag { Id = 0, Type = BagType.Leaf };

        var i1 = new Bag
        {
            Id = 1,
            Type = BagType.Introduce,
            IntroducedVertex = 1,
            Vertices = new List<int> { 1 },
            Left = leaf
        };

        var i2 = new Bag
        {
            Id = 2,
            Type = BagType.Introduce,
            IntroducedVertex = 2,
            Vertices = new List<int> { 1, 2 },
            Left = i1
        };

        var f1 = new Bag
        {
            Id = 3,
            Type = BagType.Forget,
            ForgottenVertex = 1,
            Vertices = new List<int> { 2 },
            Left = i2
        };

        var i3 = new Bag
        {
            Id = 4,
            Type = BagType.Introduce,
            IntroducedVertex = 3,
            Vertices = new List<int> { 2, 3 },
            Left = f1
        };

        var solver = new TreeDecompositionDP(graph);
        Console.WriteLine("Maximum Independent Set = " + solver.Solve(i3));
    }
}


