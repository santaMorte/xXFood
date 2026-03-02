using Microsoft.AspNetCore.Mvc;

namespace xFood.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();
    }
}
