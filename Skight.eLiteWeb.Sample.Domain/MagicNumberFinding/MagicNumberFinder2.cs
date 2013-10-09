using System;

namespace Skight.HelpCenter.Domain
{
    public class MagicNumberFinder2
    {
        private int[] used = new int[10];

        public void find()
        {
            used[5] = 1;
            dfs(0, 1);
        }

        public void dfs(int pre, int pos)
        {
            if (pos > 9)
            {
                Console.WriteLine(pre);
                return;
            }
            int tmp = pre*10;
            if (pos == 5)
            {
                dfs(tmp + 5, pos + 1);
            }
            else
            {

                for (int i = 1; i < 10; i++)
                {
                    if (used[i] == 0)
                    {
                        int cur = tmp + i;
                        if (cur%pos == 0)
                        {
                            used[i] = 1;
                            dfs(cur, pos + 1);
                            used[i] = 0;
                        }
                    }

                }

            }
        }
    }
}