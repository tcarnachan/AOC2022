namespace AOC2022
{
    public class Day19
    {
        int[][] inp;

        Factory[] factories;

        long[] upperBounds = new long[33];
        long[] currMax;

        public Day19()
        {
            inp = InputHandler.GetInput(19).Split('\n').Map(InputHandler.GetInts);

            factories = new Factory[inp.Length];
            for (int i = 0; i < inp.Length; i++)
            {
                int[] blueprint = inp[i];
                factories[i] = new Factory(blueprint[0], new (int, int)[][]
                {
                    new (int, int)[] { (blueprint[1], 0) },
                    new (int, int)[] { (blueprint[2], 0) },
                    new (int, int)[] { (blueprint[3], 0), (blueprint[4], 1) },
                    new (int, int)[] { (blueprint[5], 0), (blueprint[6], 2) }
                });
            }

            // Geodes opened with t minutes left if one
            // geode robot is created every minute
            int sum = 1;
            for (int t = 1; t < 33; t++)
            {
                upperBounds[t] = sum;
                sum += t;
            }
        }

        List<int> emptyList = new List<int>();

        private long Search(Factory factory, int depth, int maxDepth, List<int> dontBuild)
        {
            // Depth == 0 or with infinite resources will still open fewer geodes than the current best
            if (depth == 0 || factory.numBots[3] * depth + upperBounds[depth] < currMax[depth])
                return 0;

            // Check what can be built
            List<int> canBuild = new List<int>();
            for (int resource = 0; resource < 4; resource++)
            {
                if (!dontBuild.Contains(resource) && factory.CanBuildBot(resource, depth))
                    canBuild.Add(resource);
            }

            // Collect resources
            factory.CollectResources();

            // Try building each bot
            long max = 0;
            foreach (int resource in canBuild)
            {
                Factory newFactory = new Factory(factory);
                newFactory.BuildBot(resource);
                max = Math.Max(max, Search(newFactory, depth - 1, maxDepth, emptyList));
            }
            // If you choose to not build something you can, then don't build it
            // until you have built something else
            canBuild.AddRange(dontBuild);
            max = Math.Max(max, Search(factory, depth - 1, maxDepth, canBuild));

            long res = factory.numBots[3] + max;
            if (res > currMax[depth])
                currMax[depth] = res;

            return res;
        }

        public long Part1()
        {
            long sum = 0;
            foreach (Factory factory in factories)
            {
                currMax = new long[25];
                sum += factory.id * Search(new Factory(factory), 24, 24, emptyList);
            }
            return sum;
        }

        public long Part2()
        {
            long prod = 1;
            for (int i = 0; i < 3; i++)
            {
                currMax = new long[33];
                prod *= Search(new Factory(factories[i]), 32, 32, emptyList);
            }
            return prod;
        }

        private class Factory
        {
            public int id;

            public int[] numBots = new int[] { 1, 0, 0, 0 };
            public (int, int)[][] botCosts;
            public int[] resources;

            private int[] maxCost;

            public Factory(int id, (int, int)[][] botCosts)
            {
                this.id = id;
                this.botCosts = botCosts;
                resources = new int[botCosts.Length - 1];

                // No robot needs more than maxCost[r] of resource r to be built
                maxCost = new int[numBots.Length - 1];
                for (int i = 0; i < maxCost.Length; i++)
                    maxCost[i] = botCosts.Max(b => b.Where(r => r.Item2 == i).Select(r => r.Item1).DefaultIfEmpty().Max());
            }

            public Factory(Factory fact)
            {
                id = fact.id;
                Array.Copy(fact.numBots, numBots, numBots.Length);
                resources = new int[numBots.Length - 1];
                Array.Copy(fact.resources, resources, resources.Length);

                // Shouldn't change so can use same copy
                botCosts = fact.botCosts;
                maxCost = fact.maxCost;
            }

            public void CollectResources()
            {
                for (int r = 0; r < resources.Length; r++)
                    resources[r] += numBots[r];
            }

            public bool CanBuildBot(int botType, int timeLeft)
            {
                // No limit on number of geode bots built
                if (botType < 3)
                {
                    // Will produce enough of this resource even
                    // if maximum amount is used each minute
                    if (numBots[botType] * timeLeft + resources[botType] >= timeLeft * maxCost[botType])
                        return false;
                }

                (int, int)[] cost = botCosts[botType];

                foreach ((int n, int r) in cost)
                {
                    if (resources[r] < n)
                        return false;
                }

                return true;
            }

            public void BuildBot(int botType)
            {
                (int, int)[] cost = botCosts[botType];

                foreach ((int n, int r) in cost)
                    resources[r] -= n;

                numBots[botType]++;
            }
        }
    }
}