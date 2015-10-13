namespace VendingMachine.Models
{
// ReSharper disable once InconsistentNaming
    public class ItemToSaleVM
    {
        public double Price { get; set; }
        public string Name { get; set; }
        public int AvailableCount { get; set; }
        public int Id { get; set; }
    }
}