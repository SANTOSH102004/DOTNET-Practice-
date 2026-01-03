using System;

class DP
{
    static bool IsPalindrome(string s, int start, int end)
    {
        while (start < end)
        {
            if (s[start] != s[end])
                return false;
            start++;
            end--;
        }
        return true;
    }

    static int MinCut(string s)
    {
        int n = s.Length;
        int[] dp = new int[n];

        for (int i = 0; i < n; i++)
        {
            if (IsPalindrome(s, 0, i))
            {
                dp[i] = 0;
            }
            else
            {
                dp[i] = int.MaxValue;
                for (int j = 0; j < i; j++)
                {
                    if(IsPalindrome(s, j + 1, i))
                    {
                        dp[i] = Math.Min(dp[i], dp[j] + 1);
                    }
                }
            }
        }
        return dp[n - 1];
    }

    static void Main()
    {
        string s = "aab";
        Console.WriteLine( "Minimum cuts needed:" + MinCut(s));
    }
}