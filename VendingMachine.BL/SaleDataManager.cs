﻿using System;
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
                AddToBank(coin, count, context);
                context.SaveChanges();
            }
        }
        private void AddToBank(decimal coin, int count, VendingContext context)
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
                AddToCache(coin, count, context);
                context.SaveChanges();
            }
        }
        private void AddToCache(decimal coin, int count, VendingContext context)
        {
            var coins = context.MoneyCache.FirstOrDefault(x => x.Nominal == coin);
            if (coins != null)
            {
                coins.Count += count;
            }
            else
            {
                context.MoneyCache.Add(new MoneyCache { Nominal = coin, Count = count });
            }
        }

        public List<MoneyCache> GetCacheCoins()
        {
            using (var context = new VendingContext())
            {
                return context.MoneyCache.ToList();
            }
        }

        private bool EnoughMoneyToBuy(ItemToSale product)
        {
            var coins = GetCacheCoins();
            return coins.Count != 0 && coins.Select(x => x.Count * x.Nominal).Aggregate((n, m) => n + m) >= product.Price;
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
                var coinsInserted = context.MoneyCache.Select(x => x.Count * x.Nominal).ToList();
                var moneyInCacheCount = coinsInserted.Aggregate((arg1, arg2) => arg1 + arg2);
                var needReturn = moneyInCacheCount - product.Price;
                foreach (var coins in context.MoneyCache)
                {
                    AddToBank(coins.Nominal, coins.Count, context);
                }
                foreach (MoneyCache cache in context.MoneyCache)
                {
                    cache.Count = 0;
                }

                ReturnToCache(needReturn, context);
                var productToBuy = context.ItemsToSale.First(x => x.Id == product.Id);
                productToBuy.AvailableCount -= 1;
                context.SaveChanges();
            }
            return true;
        }

        private void ReturnToCache(decimal needReturn, VendingContext context)
        {
            var ordered = context.Bank.OrderByDescending(x => x.Nominal);
            foreach (var coins in ordered)
            {
                while (coins.Nominal <= needReturn && coins.Count != 0)
                {
                    coins.Count -= 1;
                    needReturn -= coins.Nominal;
                    AddToCache(coins.Nominal, 1, context);
                }
                if (needReturn <= 0) return;
            }
        }

        #endregion cache

        #region UserWallet
        public List<UserWallet> GetUserCoins()
        {
            using (var context = new VendingContext())
            {
                return context.UserWallet.Where(x => x.Count > 0).ToList();
            }
        }

        public void AddToUserWallet(decimal coin, int count)
        {
            using (var context = new VendingContext())
            {
                AddToUserWallet(coin, count, context);
                context.SaveChanges();
            }
        }
        private void AddToUserWallet(decimal coin, int count, VendingContext context)
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

        }

        public void InsertCoin(decimal nominal)
        {
            using (var context = new VendingContext())
            {
                var coins = context.UserWallet.FirstOrDefault(x => x.Nominal == nominal);
                if (coins != null && coins.Count >= 1)
                {
                    coins.Count -= 1;
                    AddToCache(nominal, 1);
                    context.SaveChanges();
                }

            }
        }

        public void MoneyBack()
        {
            using (var context = new VendingContext())
            {
                foreach (var coins in context.MoneyCache)
                {
                    AddToUserWallet(coins.Nominal, coins.Count, context);
                }
                foreach (MoneyCache cache in context.MoneyCache)
                {
                    cache.Count = 0;
                }
                context.SaveChanges();
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
