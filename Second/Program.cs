using System;
using System.Runtime.CompilerServices;

class RemovingBoxes
{
    public static int RemovegBoxes(int[] boxes)
    {
        int n = boxes.Length;
        int[,,] dp = new int[n, n, n];
        return Helper(boxes, 0, n - 1, 0, dp);
    }    

    private static int Helper(int[] boxes, int l, int r, int k, int[,,] dp)
    {
        if (l > r) return 0;
        if (dp[l, r, k] != 0) return dp[l, r, k];

        while (r > l && boxes[r] == boxes[r - 1])
        {
            r--;
            k++;
        }

        dp[l, r, k] = Helper(boxes, l, r - 1, 0, dp) + (k + 1) * (k + 1);

        for (int i = l; i < r; i++)
        {
            if (boxes[i] == boxes[r])
            {
                dp[l, r, k] = Math.Max(dp[l, r, k],
                    Helper(boxes, l, i, k + 1, dp) + Helper(boxes, i + 1, r - 1, 0, dp));
            }
        }
        return dp[l, r, k];
    }
    static void Main()
    {
        int[] boxes = { 1,2,3,2,2,2,3,4,3,1 };
        Console.WriteLine("Maximum points: " + RemovegBoxes(boxes));
    }
}