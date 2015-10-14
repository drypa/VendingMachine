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
                context.Database.ExecuteSqlCommand("TRUNCATE TABLE [bank]");
                context.Database.ExecuteSqlCommand("TRUNCATE TABLE [money_cache]");
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

        public void AddToCache(decimal coin, int count)
        {
            using (var context = new VendingContext())
            {
                var coins = context.MoneyСache.FirstOrDefault(x => x.Nominal == coin);
                if (coins != null)
                {
                    coins.Count += count;
                }
                else
                {
                    context.MoneyСache.Add(new MoneyCache { Nominal = coin ,Count = count});
                }
                
                context.SaveChanges();
            }
        }

        public void AddToBank(decimal coin, int count)
        {
            using (var context = new VendingContext())
            {
                var coins = context.Bank.FirstOrDefault(x => x.Nominal == coin);
                if (coins != null)
                {
                    coins.Count += count;
                }
                else
                {
                    context.Bank.Add(new Bank { Nominal = coin, Count = count });
                }

                context.SaveChanges();
            }
        }
    }
}
