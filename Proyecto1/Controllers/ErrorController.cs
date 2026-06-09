using Microsoft.AspNetCore.Mvc;

namespace Proyecto1.Controllers
{
    public class ErrorController : Controller
{
    public IActionResult Error()
    {
        return View();
    }
}
}
