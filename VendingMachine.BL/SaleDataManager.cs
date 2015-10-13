using System;
using System.Collections.Generic;
using System.Linq;
using VendingMachine.BE;

namespace VendingMachine.BL
{
    public class SaleDataManager : ISaleDataManager
    {
        #region workaroun to fix error "No Entity Framework provider found for the ADO.NET provider"
        private volatile Type _dependency;

        public SaleDataManager()
        {
            _dependency = typeof(System.Data.Entity.SqlServer.SqlProviderServices);
        }
        #endregion workaroun to fix error "No Entity Framework provider found for the ADO.NET provider"

        public List<ItemToSale> GetSaleList()
        {
            using (var context = new VendingContext())
            {
                return context.ItemsToSale.ToList();
            }
        }

        public void Reset()
        {
            using (var context = new VendingContext())
            {
                context.Database.ExecuteSqlCommand("TRUNCATE TABLE [wares]");
            }
        }

        public void Add(ItemToSale item)
        {
            using (var context = new VendingContext())
            {
                context.ItemsToSale.Add(item);
                context.SaveChanges();
            }
        }
    }
}
