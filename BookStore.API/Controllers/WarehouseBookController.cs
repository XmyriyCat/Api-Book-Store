using Microsoft.AspNetCore.Mvc;

namespace ApiBookStore.Controllers
{
    public class WarehouseBookController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
