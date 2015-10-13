using System.Collections.Generic;
using VendingMachine.Models;

namespace VendingMachine.Managers
{
    public interface IVendingManager
    {
        List<ItemToSaleVM> GetSaleList();
        void Reset();
    }
}