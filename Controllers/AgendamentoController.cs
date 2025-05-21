using Microsoft.AspNetCore.Mvc;
using AgendamentoTransporte.Models;
using AgendamentoTransporte.Data;
using System.Linq;
using ClosedXML.Excel;
using System.IO;

namespace AgendamentoTransporte.Controllers
{
    public class AgendamentoController : Controller
    {
        private readonly AppDbContext _context;

        public AgendamentoController(AppDbContext context)
        {
            _context = context;
        }

        private void PreencherViewBags()
        {
            ViewBag.LocaisConsulta = new List<string>
            {
                "AME Atibaia", "Atibaia", "Amparo", "AME Amparo", "Americana", "Barretos",
                "Bragança Paulista", "Husf Bragança Paulista", "Campinas", "AME Campinas", "Unicamp Campinas",
                "Jundiaí", "Hospital Regional Jundiaí", "Pedra Bela", "Pinhalzinho", "Santa Bárbara", "AME Santa Barbara",
                "São Paulo", "Socorro", "Outros"
            };

            ViewBag.LocaisBusca = new List<string>
            {
                "Araras", "Bairro Maciel", "Bairro do Ribeiro", "Boca da Mata", "Campanha",
                "Campo", "Córrego Raso", "Estiva", "Limas",
                "Paiol das Telhas", "Pedra Bela", "Pitangueiras de baixo", "Pitangueiras de cima",
                "Pitangueiras do meio", "Sertãozinho", "Vargem", "Vargem do Monjolo", "Tuncuns", "Outros"
            };
        }

        public IActionResult Index()
        {
            PreencherViewBags();
            return View();
        }

        [HttpPost]
        public IActionResult Agendar(Agendamento agendamento)
        {
            if (ModelState.IsValid)
            {
                // Captura automaticamente o nome do usuário logado (Administrador ou Agendador)
                agendamento.UsuarioResponsavel = TempData["UsuarioLogado"]?.ToString();
               

                TempData.Keep(); // mantém o dado para as próximas requisições

                _context.Agendamentos.Add(agendamento);
                _context.SaveChanges();

                return RedirectToAction("Confirmar", new { id = agendamento.Id });
            }

            PreencherViewBags();
            return View("Index", agendamento);
        }

        public IActionResult Confirmar(int id)
        {
            var agendamento = _context.Agendamentos.FirstOrDefault(a => a.Id == id);
            if (agendamento == null)
                return RedirectToAction("Index");

            return View(agendamento);
        }

        public IActionResult Visualizar(DateTime? data, TimeSpan? hora, string? local)
        {
            var agendamentos = _context.Agendamentos.AsQueryable();

            if (data.HasValue)
                agendamentos = agendamentos.Where(a => a.Data.Date == data.Value.Date);
            if (hora.HasValue)
                agendamentos = agendamentos.Where(a => a.Hora == hora.Value);
            if (!string.IsNullOrEmpty(local))
                agendamentos = agendamentos.Where(a => a.LocalConsulta == local || a.LocalConsultaOutro == local);


            ViewData["Data"] = data?.ToString("yyyy-MM-dd");
            ViewData["Hora"] = hora?.ToString(@"hh\:mm");
            ViewData["Local"] = local;
            ViewBag.TipoUsuario = TempData["TipoUsuario"];
            TempData.Keep(); // mantém o dado para o próximo request



            return View(agendamentos.ToList().OrderBy(a => a.Data).ThenBy(a => a.Hora));

        }

        public IActionResult ExportarExcel()
        {
            var agendamentos = _context.Agendamentos.ToList();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Agendamentos");

            worksheet.Cell(1, 1).Value = "Nome";
            worksheet.Cell(1, 2).Value = "Telefone";
            worksheet.Cell(1, 3).Value = "Data";
            worksheet.Cell(1, 4).Value = "Hora";
            worksheet.Cell(1, 5).Value = "Local";
            worksheet.Cell(1, 6).Value = "Acompanhante";

            for (int i = 0; i < agendamentos.Count; i++)
            {
                var a = agendamentos[i];
                worksheet.Cell(i + 2, 1).Value = a.NomePaciente;
                worksheet.Cell(i + 2, 2).Value = a.Telefone;
                worksheet.Cell(i + 2, 3).Value = a.Data.ToShortDateString();
                worksheet.Cell(i + 2, 4).Value = a.Hora.ToString();
                worksheet.Cell(i + 2, 5).Value = a.LocalConsulta;
                worksheet.Cell(i + 2, 6).Value = a.PrecisaAcompanhante;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            return File(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Agendamentos.xlsx");
        }

        // GET: Agendamento/Cancelar
        public IActionResult Cancelar(string? local, DateTime? data, TimeSpan? hora)
        {
            if (TempData["TipoUsuario"]?.ToString() != "Administrador")
            {
                TempData["Erro"] = "Apenas administradores podem cancelar agendamentos.";
                return RedirectToAction("Visualizar");
            }

            TempData.Keep();

            var agendamentos = _context.Agendamentos.AsQueryable();

            if (!string.IsNullOrEmpty(local))
                agendamentos = agendamentos.Where(a => a.LocalConsulta == local);
            if (data.HasValue)
                agendamentos = agendamentos.Where(a => a.Data.Date == data.Value.Date);
            if (hora.HasValue)
                agendamentos = agendamentos.Where(a => a.Hora == hora.Value);


            var listaAgendamentos = agendamentos.ToList()
                                    .OrderBy(a => a.Data)
                                    .ThenBy(a => a.Hora)
                                    .ToList();

            return View(listaAgendamentos);

        }
        // GET: Agendamento/Editar/5
        public IActionResult Editar(int id)
        {
            if (TempData["TipoUsuario"]?.ToString() != "Administrador")
            {
                TempData["Erro"] = "Apenas administradores podem editar agendamentos.";
                return RedirectToAction("Visualizar");
            }

            TempData.Keep();

            var agendamento = _context.Agendamentos.FirstOrDefault(a => a.Id == id);
            if (agendamento == null)
            {
                return NotFound();
            }

            return View(agendamento);
        }

        // POST: Agendamento/Editar
        [HttpPost]
        public IActionResult Editar(Agendamento agendamento)
        {
            if (TempData["TipoUsuario"]?.ToString() != "Administrador")
            {
                TempData["Erro"] = "Apenas administradores podem editar agendamentos.";
                return RedirectToAction("Visualizar");
            }

            if (!ModelState.IsValid)
            {
                return View(agendamento);
            }

            var agendamentoExistente = _context.Agendamentos.FirstOrDefault(a => a.Id == agendamento.Id);
            if (agendamentoExistente == null)
            {
                return NotFound();
            }

            // Atualiza os campos
            agendamentoExistente.NomePaciente = agendamento.NomePaciente;
            agendamentoExistente.Telefone = agendamento.Telefone;
            agendamentoExistente.Data = agendamento.Data;
            agendamentoExistente.Hora = agendamento.Hora;
            agendamentoExistente.LocalConsulta = agendamento.LocalConsulta;
            agendamentoExistente.LocalConsultaOutro = agendamento.LocalConsultaOutro;
            agendamentoExistente.PrecisaAcompanhante = agendamento.PrecisaAcompanhante;
            agendamentoExistente.LocalBusca = agendamento.LocalBusca;
            agendamentoExistente.LocalBuscaOutro = agendamento.LocalBuscaOutro;
            agendamentoExistente.PontoReferencia = agendamento.PontoReferencia;
            agendamentoExistente.MotivoViagem = agendamento.MotivoViagem;
            agendamentoExistente.Observacao = agendamento.Observacao;
            agendamentoExistente.UsuarioResponsavel = agendamento.UsuarioResponsavel;

            _context.SaveChanges();

            TempData["Mensagem"] = "Agendamento atualizado com sucesso!";
            return RedirectToAction("Visualizar");
        }


        [HttpPost]
        public IActionResult CancelarConfirmado(int id)
        {
            if (TempData["TipoUsuario"]?.ToString() != "Administrador")
            {
                TempData["Erro"] = "Apenas administradores podem cancelar agendamentos.";
                return RedirectToAction("Visualizar");
            }

            var agendamento = _context.Agendamentos.Find(id);
            if (agendamento != null)
            {
                _context.Agendamentos.Remove(agendamento);
                _context.SaveChanges();
            }

            return RedirectToAction("Visualizar");
        }
    }
}
