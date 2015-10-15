using System.Collections.Generic;
using System.Linq;

namespace VendingMachine.Helpers
{
    public static class MoneyHelper
    {
        public static decimal Summary(this IDictionary<decimal, int> coins)
        {
            return coins.Count == 0 ? 0 : coins.Select(x => x.Key * x.Value).Aggregate((i, j) => i + j);
        }
    }
}