using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using VendingMachine.BE;

namespace VendingMachine.BL
{
    public class SaleDataManager : ISaleDataManager
    {
        #region workaround to fix error "No Entity Framework provider found for the ADO.NET provider"
        // ReSharper disable once NotAccessedField.Local
        private volatile Type _dependency;

        public SaleDataManager()
        {
            _dependency = typeof(System.Data.Entity.SqlServer.SqlProviderServices);
        }
        #endregion workaround to fix error "No Entity Framework provider found for the ADO.NET provider"

        #region products
        public List<ItemToSale> GetProducts()
        {
            using (var context = new VendingContext())
            {
                return context.ItemsToSale.ToList();
            }
        }

        public ItemToSale GetProduct(int productId)
        {
            using (var context = new VendingContext())
            {
                return context.ItemsToSale.FirstOrDefault(x => x.Id == productId);
            }
        }

        public void AddProduct(ItemToSale item)
        {
            using (var context = new VendingContext())
            {
                context.ItemsToSale.Add(item);
                context.SaveChanges();
            }
        }
        #endregion products

        #region bank
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

        public List<Bank> GetBankCoins()
        {
            using (var context = new VendingContext())
            {
                return context.Bank.ToList();
            }
        }
        #endregion bank

        #region cache
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
                    context.MoneyСache.Add(new MoneyCache { Nominal = coin, Count = count });
                }

                context.SaveChanges();
            }
        }

        public List<MoneyCache> GetCacheCoins()
        {
            using (var context = new VendingContext())
            {
                return context.MoneyСache.ToList();
            }
        }

        private bool EnoughMoneyToBuy(ItemToSale product)
        {
            var coins = GetCacheCoins();
            return coins.Count != 0 && coins.Select(x => x.Count * x.Nominal).Aggregate((n, m) => n + m) >= product.Price;
        }

        #endregion cache

        #region UserWallet
        public List<UserWallet> GetUserCoins()
        {
            using (var context = new VendingContext())
            {
                return context.UserWallet.ToList();
            }
        }

        public void AddToUserWallet(decimal coin, int count)
        {
            using (var context = new VendingContext())
            {
                var coins = context.UserWallet.FirstOrDefault(x => x.Nominal == coin);
                if (coins != null)
                {
                    coins.Count += count;
                }
                else
                {
                    context.UserWallet.Add(new UserWallet { Nominal = coin, Count = count });
                }

                context.SaveChanges();
            }
        }

        public bool Buy(ItemToSale product, out string errorMessage)
        {
            errorMessage = null;
            if (!EnoughMoneyToBuy(product))
            {
                errorMessage = "Недостаточно внесено денег для покупки";
                return false;
            }

            using (var context = new VendingContext())
            {
                //DecreaseCache(context.MoneyСache, product.Price,context.Bank);
            }
            return false;
        }

        private void DecreaseCache(DbSet<MoneyCache> moneyСache,decimal delta,DbSet<Bank> bank)
        {
            var ordered = moneyСache.OrderByDescending(x => x.Nominal);
            foreach (var coin in ordered)
            {
                if(coin.Nominal > delta || coin.Count==0) continue;
                coin.Count -= 1;
                delta -= coin.Nominal;
            }
        }

        #endregion UserWallet



        public void Reset()
        {
            using (var context = new VendingContext())
            {
                context.Database.ExecuteSqlCommand("TRUNCATE TABLE [wares]");
                context.Database.ExecuteSqlCommand("TRUNCATE TABLE [bank]");
                context.Database.ExecuteSqlCommand("TRUNCATE TABLE [money_cache]");
                context.Database.ExecuteSqlCommand("TRUNCATE TABLE [user_wallet]");
            }
        }
       
    }
}
