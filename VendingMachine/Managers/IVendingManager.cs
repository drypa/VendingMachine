using VendingMachine.Models;

namespace VendingMachine.Managers
{
    public interface IVendingManager
    {
        VendingMachineVm GetModel();
        void Reset();
        void AddMoneyToBank(decimal coin, int count);
        void AddMoneyToWallet(decimal coin, int count);
        void Add(ItemToSaleVM item);
    }
}