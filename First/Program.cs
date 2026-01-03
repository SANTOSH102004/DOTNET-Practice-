using System;

class BurstBalloons
{
    public static int MaxCoins(int[] nums)
    {
        int n = nums.Length;
        int[] arr = new int[n + 2];
        arr[0] = arr[n + 1] = 1;

        for (int i = 0; i < n; i++)
            arr[i + 1] = nums[i];

        int[,] dp = new int[n + 2, n + 2];

        for (int length = 1; length <= n; length++)
        {
            for (int left = 1; left <= n - length + 1; left++)
            {
                int right = left + length - 1;

                for (int k = left; k <= right; k++)
                {
                    int coins = arr[left - 1] * arr[k] * arr[right + 1];
                    coins += dp[left, k - 1] + dp[k + 1, right];
                    dp[left, right] = Math.Max(dp[left, right], coins);
                }
            }
        }

        return dp[1, n];
    }

    static void Main()
    {
        int[] nums = { 3, 1, 5, 8 };
        Console.WriteLine("Maximum coins: " + MaxCoins(nums));
    }
}
