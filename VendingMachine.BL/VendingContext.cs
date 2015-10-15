using System.Data.Entity;
using VendingMachine.BE;

namespace VendingMachine.BL
{
    public class VendingContext : DbContext
    {
        public DbSet<ItemToSale> ItemsToSale { get; set; }
        public DbSet<MoneyCache> MoneyСache { get; set; }
        public DbSet<Bank> Bank { get; set; }
        public DbSet<UserWallet> UserWallet { get; set; }
    }
}
