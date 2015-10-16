using System.Collections.Generic;
using VendingMachine.BE;

namespace VendingMachine.BL
{
    public interface ISaleDataManager
    {
        List<ItemToSale> GetProducts();
        ItemToSale GetProduct(int productId);
        void Reset();
        void AddProduct(ItemToSale item);
        void AddToCache(decimal coin, int count);
        void AddToBank(decimal coin, int count);
        List<Bank> GetBankCoins();
        List<MoneyCache> GetCacheCoins();
        List<UserWallet> GetUserCoins();
        void AddToUserWallet(decimal coin, int count);
        bool Buy(ItemToSale product, out string errorMessage);
        void InsertCoin(decimal nominal);
        void MoneyBack();
    }
}