using System.Collections.Generic;
using System.Linq;
using VendingMachine.BE;
using VendingMachine.BL;
using VendingMachine.Models;

namespace VendingMachine.Managers
{
    public class VendingManager : IVendingManager
    {
        public List<ItemToSaleVM> GetSaleList()
        {
            return GetDataManager().GetSaleList().Select(x => new ItemToSaleVM { Name = x.Name, Price = x.Price, AvailableCount = x.AvailableCount, Id = x.Id }).ToList();
        }

        private ISaleDataManager GetDataManager()
        {
            return new SaleDataManager();
        }

        public void Reset()
        {
            GetDataManager().Reset();
        }

        public void Add(ItemToSaleVM item)
        {
            GetDataManager().Add(new ItemToSale { Name = item.Name, Price = item.Price, AvailableCount = item.AvailableCount, Id = item.Id });
        }

        public void AddMoneyToBank(decimal coin, int count)
        {
            GetDataManager().AddToBank(coin, count);
        }
        public void AddMoneyToCache(decimal coin, int count)
        {
            GetDataManager().AddToCache(coin, count);
        }
    }
}