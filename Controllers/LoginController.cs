using Microsoft.AspNetCore.Mvc;

namespace AgendamentoTransporte.Controllers
{
    public class LoginController : Controller
    {
        // Armazenamento simplificado em memória
        private static Dictionary<string, int> tentativas = new();
        private static Dictionary<string, DateTime> bloqueios = new();

        private const int MAX_TENTATIVAS = 5;
        private const int BLOQUEIO_MINUTOS = 15;

        // GET: Login
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string usuario, string senha)
        {
            if (bloqueios.ContainsKey(usuario) && bloqueios[usuario] > DateTime.Now)
            {
                ViewBag.Erro = $"Usuário bloqueado até {bloqueios[usuario]:HH:mm:ss}";
                return View();
            }

            bool autenticado = false;

            if (usuario == "Renato" && senha == "12345678")
            {
                TempData["TipoUsuario"] = "Administrador";
                TempData["UsuarioLogado"] = "Renato";
                autenticado = true;
            }
            else if (usuario == "Mônica" && senha == "123")
            {
                TempData["TipoUsuario"] = "Agendador";
                TempData["UsuarioLogado"] = "Mônica";
                autenticado = true;
            }

            if (autenticado)
            {
                // Zera tentativas e bloqueios ao logar com sucesso
                tentativas.Remove(usuario);
                bloqueios.Remove(usuario);

                return RedirectToAction("Index", "Menu");
            }
            else
            {
                if (!tentativas.ContainsKey(usuario))
                    tentativas[usuario] = 0;

                tentativas[usuario]++;

                if (tentativas[usuario] >= MAX_TENTATIVAS)
                {
                    bloqueios[usuario] = DateTime.Now.AddMinutes(BLOQUEIO_MINUTOS);
                    tentativas.Remove(usuario);
                    ViewBag.Erro = $"Usuário bloqueado por {BLOQUEIO_MINUTOS} minutos.";
                }
                else
                {
                    int restantes = MAX_TENTATIVAS - tentativas[usuario];
                    ViewBag.Erro = $"Usuário ou senha inválidos. Tentativas restantes: {restantes}";
                }

                return View();
            }
        }
    }
}
