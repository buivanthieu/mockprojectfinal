using Microsoft.AspNetCore.Mvc;
namespace WebMVC.Controllers
{
    [Route("[controller]")]
    public class LookupController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

       
    }
}
