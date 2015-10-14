using System.Collections.Generic;
using VendingMachine.BE;

namespace VendingMachine.BL
{
    public interface ISaleDataManager
    {
        List<ItemToSale> GetSaleList();
        void Reset();
        void Add(ItemToSale item);
        void AddToCache(decimal coin, int count);
        void AddToBank(decimal coin, int count);
    }
}