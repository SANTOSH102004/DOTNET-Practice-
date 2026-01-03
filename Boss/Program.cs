using System;
using System.Collections.Generic;
using System.Linq;

class SteinerTreeDP
{
    // Normalize partition labels to canonical form
    static int[] Normalize(int[] comp)
    {
        Dictionary<int, int> map = new();
        int next = 0;
        int[] res = new int[comp.Length];

        for (int i = 0; i < comp.Length; i++)
        {
            if (comp[i] == -1)
            {
                res[i] = -1;
                continue;
            }

            if (!map.ContainsKey(comp[i]))
                map[comp[i]] = next++;

            res[i] = map[comp[i]];
        }
        return res;
    }

    static string Key(int mask, int[] comp)
    {
        return mask + "|" + string.Join(",", comp);
    }

    static int BitCount(int x)
    {
        int c = 0;
        while (x > 0) { c += x & 1; x >>= 1; }
        return c;
    }

    // FINAL BOSS DP (restricted but real)
    public static int Solve(
        List<int[]> bags,
        HashSet<int> terminals)
    {
        Dictionary<string, int> dp = new();
        dp["0|"] = 0;

        foreach (var bag in bags)
        {
            Dictionary<string, int> next = new();
            int m = bag.Length;
            int fullMask = 1 << m;

            foreach (var state in dp)
            {
                var parts = state.Key.Split('|');
                int oldCost = state.Value;

                for (int mask = 0; mask < fullMask; mask++)
                {
                    // All terminals in this bag must be selected
                    bool ok = true;
                    for (int i = 0; i < m; i++)
                        if (terminals.Contains(bag[i]) && ((mask & (1 << i)) == 0))
                            ok = false;

                    if (!ok) continue;

                    int[] comp = new int[m];
                    int cid = 0;

                    for (int i = 0; i < m; i++)
                        comp[i] = ((mask & (1 << i)) != 0) ? cid++ : -1;

                    comp = Normalize(comp);
                    string k = Key(mask, comp);
                    int cost = oldCost + BitCount(mask);

                    if (!next.ContainsKey(k) || next[k] > cost)
                        next[k] = cost;
                }
            }

            dp = next;
        }

        int ans = int.MaxValue;

        foreach (var s in dp)
        {
            var parts = s.Key.Split('|');
            if (parts.Length < 2) continue;

            int[] comp = parts[1].Split(',').Select(int.Parse).ToArray();
            var valid = comp.Where(x => x != -1).Distinct().Count();

            if (valid <= 1)
                ans = Math.Min(ans, s.Value);
        }

        return ans;
    }
}

class Program
{
    // 🚨 ENTRY POINT (THIS FIXES YOUR ERROR)
    static void Main(string[] args)
    {
        // Path decomposition (treewidth = 1)
        var bags = new List<int[]>
        {
            new[] {1, 2},
            new[] {2, 3},
            new[] {3, 4}
        };

        // Terminals to connect
        var terminals = new HashSet<int> { 1, 4 };

        int result = SteinerTreeDP.Solve(bags, terminals);
        Console.WriteLine("Minimum Steiner Tree cost = " + result);
    }
}
