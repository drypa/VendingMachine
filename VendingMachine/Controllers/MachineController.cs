using System.Collections.Generic;
using System.Web.Mvc;
using VendingMachine.Managers;
using VendingMachine.Models;

namespace VendingMachine.Controllers
{
    public class MachineController : Controller
    {
        public ActionResult Index()
        {
            var model = GetManager().GetSaleList();
            return View(model);
        }

        public ActionResult Reset()
        {
            GetManager().Reset();
            return RedirectToAction("Index");
        }

        public ActionResult Init()
        {
            var manager = GetManager();
            manager.Reset();
            List<ItemToSaleVM> items = new List<ItemToSaleVM>
            {
                new ItemToSaleVM{Name = "Чай",Price = 13,AvailableCount = 10},
                new ItemToSaleVM{Name = "Кофе",Price = 18,AvailableCount = 20},
                new ItemToSaleVM{Name = "Кофе с молоком",Price = 21,AvailableCount = 20},
                new ItemToSaleVM{Name = "Сок",Price = 35,AvailableCount = 15},
            };
            foreach (var item in items)
            {
                manager.Add(item);
            }
            return RedirectToAction("Index");
        }

        private VendingManager GetManager()
        {
            return new VendingManager();
        }
    }
}
