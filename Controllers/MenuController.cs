using Microsoft.AspNetCore.Mvc;

namespace AgendamentoTransporte.Controllers
{
    public class MenuController : Controller
    {
        public IActionResult Index()
        {
            if (TempData["TipoUsuario"] == null)
            {
                // Tentativa de recuperar da Session se perder o TempData (opcional)
                return RedirectToAction("Index", "Login");
            }

            ViewBag.TipoUsuario = TempData["TipoUsuario"];
            TempData.Keep("TipoUsuario"); // Mantém o TempData ativo na próxima requisição

            return View();
        }
    }
}

