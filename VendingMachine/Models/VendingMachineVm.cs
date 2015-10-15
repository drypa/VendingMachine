using System.Collections.Generic;

namespace VendingMachine.Models
{
    public class VendingMachineVm
    {
        public List<ItemToSaleVM> ItemsToSale { get; set; }
        public Dictionary<decimal, int> Bank { get; set; }
        public Dictionary<decimal, int> Cache { get; set; }
        public Dictionary<decimal, int> Wallet { get; set; }
    }
}