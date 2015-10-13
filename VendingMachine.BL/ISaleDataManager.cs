using System.Collections.Generic;
using VendingMachine.BE;

namespace VendingMachine.BL
{
    public interface ISaleDataManager
    {
        List<ItemToSale> GetSaleList();
        void Reset();
        void Add(ItemToSale item);
    }
}