using System.Data.Entity;
using VendingMachine.BE;

namespace VendingMachine.BL
{
    public class VendingContext: DbContext
    {
        public DbSet<ItemToSale> ItemsToSale { get; set; }
    }
}
