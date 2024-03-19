using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Web.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Cart()
        {
            return View();
        }
    }
}
