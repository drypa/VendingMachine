using System.Collections.Generic;
using System.Linq;
using VendingMachine.BE;
using VendingMachine.BL;
using VendingMachine.Models;

namespace VendingMachine.Managers
{
    public class VendingManager : IVendingManager
    {
        public VendingMachineVm GetModel()
        {
            return new VendingMachineVm
            {
                ItemsToSale = GetSaleList(),
                Bank = GetBankCoins(),
                Cache = GetCacheCoins(),
                Wallet = GetUserCoins()
            };
        }

        private Dictionary<decimal, int> GetUserCoins()
        {
            var coins = GetDataManager().GetUserCoins();
            var result = new Dictionary<decimal, int>(coins.Count);
            foreach (var coin in coins)
            {
                result.Add(coin.Nominal, coin.Count);
            }
            return result;
        }

        private List<ItemToSaleVM> GetSaleList()
        {
            return GetDataManager().GetProducts().Select(x => new ItemToSaleVM { Name = x.Name, Price = x.Price, AvailableCount = x.AvailableCount, Id = x.Id }).ToList();
        }

        private Dictionary<decimal, int> GetBankCoins()
        {
            var coins = GetDataManager().GetBankCoins();
            var result = new Dictionary<decimal, int>(coins.Count);
            foreach (var coin in coins)
            {
                result.Add(coin.Nominal, coin.Count);
            }
            return result;
        }

        private ISaleDataManager _manager;
        private ISaleDataManager GetDataManager()
        {
            return _manager ?? (_manager = new SaleDataManager());
        }

        public void Reset()
        {
            GetDataManager().Reset();
        }

        public void Add(ItemToSaleVM item)
        {
            GetDataManager().AddProduct(new ItemToSale { Name = item.Name, Price = item.Price, AvailableCount = item.AvailableCount, Id = item.Id });
        }

        public void AddMoneyToBank(decimal coin, int count)
        {
            GetDataManager().AddToBank(coin, count);
        }
        public void AddMoneyToCache(decimal coin, int count)
        {
            GetDataManager().AddToCache(coin, count);
        }

        public void AddMoneyToWallet(decimal coin, int count)
        {
            GetDataManager().AddToUserWallet(coin, count);
        }

        private Dictionary<decimal, int> GetCacheCoins()
        {
            var coins = GetDataManager().GetCacheCoins();
            var result = new Dictionary<decimal, int>(coins.Count);
            foreach (var coin in coins)
            {
                result.Add(coin.Nominal, coin.Count);
            }
            return result;
        }
    }
}