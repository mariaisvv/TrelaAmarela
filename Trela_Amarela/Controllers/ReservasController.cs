using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Trela_Amarela.Data;
using Trela_Amarela.Models;

namespace Trela_Amarela.Controllers
{
    [Authorize] // esta 'anotação' garante que só as pessoas autenticadas têm acesso aos recursos
    public class ReservasController : Controller
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// esta variável recolhe os dados da pessoa q se autenticou
        /// </summary>
        private readonly UserManager<ApplicationUser> _userManager;

        public ReservasController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Reservas
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Admin"))
            {
                var reservas = await _context.Reservas
                                            .Include(r => r.ListaAnimais)
                                            .Include(r => r.Box)
                                            .ToListAsync();

                return View(reservas);
            }
            else
            {

                // var. auxiliar
                string idDaPessoaAutenticada = _userManager.GetUserId(User);
                // quais as reservas que pertencem à pessoa que está autenticada?
                var reservas = await (from r in _context.Reservas.Include(r => r.ListaAnimais).Include(r => r.Box)
                                      join c in _context.Clientes on r.IdCliente equals c.IdCliente
                                      join u in _context.Users on c.Email equals u.Email
                                      where u.Id == idDaPessoaAutenticada
                                      select r)
                                 .ToListAsync();
                return View(reservas);
            }
        }

        // GET: Reservas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Reservas == null)
            {
                return NotFound();
            }

            var reservas = await _context.Reservas
                .Include(r => r.Box)
                .Include(r => r.Cliente)
                .Include(l => l.ListaAnimais)
                .FirstOrDefaultAsync(m => m.IdReserva == id);
            if (reservas == null)
            {
                return NotFound();
            }

            return View(reservas);
        }

        // GET: Reservas/Create
        public async Task<IActionResult> Create()
        {
            // e, o ID fornecido pertence a um animal que pertence ao Utilizador que está a usar o sistema?
            int idClienteAutenticado = (await _context.Clientes.Where(c => c.UserName == _userManager.GetUserId(User)).FirstOrDefaultAsync()).IdCliente;
            string idAutenticado = _userManager.GetUserId(User);
            ViewData["IdBox"] = new SelectList(_context.Boxs, "IdBox", "Nome");
            ViewBag.ListaAnimais = await (from a in _context.Animais.Include(a => a.Cliente)
                                          join c in _context.Clientes on a.IdCliente equals c.IdCliente
                                          join u in _context.Users on c.Email equals u.Email
                                          where u.Id == idAutenticado
                                          select a)
                                  .ToListAsync();
            ViewData["IdAnimal"] = new SelectList(_context.Animais.Include(a => a.Cliente).Where(a => a.IdCliente == idClienteAutenticado), "IdAnimal", "Nome");
            return View();
        }

        // POST: Reservas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdReserva,D_Entrada,D_Saida,Nr_animais,Nr_registo,IdCliente,IdBox")] Reservas reserva, int[] AnimalEscolhido)
        {

            // avalia se o array com a lista de animais escolhidos associados à reserva  está vazio ou não
            if (AnimalEscolhido.Length == 0)
            {
                //É gerada uma mensagem de erro
                ModelState.AddModelError("", "É necessário selecionar pelo menos um animal.");
                string idAutenticado = _userManager.GetUserId(User);
                // gerar a lista serviços que podem ser associados à oficina
                ViewBag.ListaAnimais = await (from a in _context.Animais.Include(a => a.Cliente)
                                              join c in _context.Clientes on a.IdCliente equals c.IdCliente
                                              join u in _context.Users on c.Email equals u.Email
                                              where u.Id == idAutenticado
                                              select a)
                      .ToListAsync();
                ViewData["IdBox"] = new SelectList(_context.Boxs, "IdBox", "Nome");

                // devolver controlo à View
                return View(reserva);
            }
            // criar uma lista com os objetos escolhidos dos animais
            List<Animais> listaDeAnimaisEscolhidos = new List<Animais>();
            // Para cada objeto escolhido..
            foreach (int item in AnimalEscolhido)
            {
                //procurar o animais
                Animais animais = _context.Animais.Find(item);
                // adicionar o animal à lista
                listaDeAnimaisEscolhidos.Add(animais);
            }

            // adicionar a lista ao objeto de "Animais"
            reserva.ListaAnimais = listaDeAnimaisEscolhidos;

            reserva.Nr_animais = AnimalEscolhido.Length;


            if (ModelState.IsValid)
            {
                // obter os dados da pessoa autenticada
                Clientes cliente = _context.Clientes.Where(c => c.UserName == _userManager.GetUserId(User)).FirstOrDefault();
                // adicionar o cliente à reserva
                reserva.Cliente = cliente;

                _context.Add(reserva);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdBox"] = new SelectList(_context.Boxs, "IdBox", "IdBox", reserva.IdBox);
            return View(reserva);
        }

        // GET: Reservas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Reservas == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reservas
                 .Where(o => o.IdReserva == id)
                 .Include(o => o.ListaAnimais)
                 .FirstOrDefaultAsync();

            if (reserva == null)
            {
                return NotFound();
            }

            // lista de todos os animais existentes do utilizador autenticado
            string idAutenticado = _userManager.GetUserId(User);
            ViewBag.ListaDeAnimais = await (from a in _context.Animais.Include(a => a.Cliente)
                                          join c in _context.Clientes on a.IdCliente equals c.IdCliente
                                          join u in _context.Users on c.Email equals u.Email
                                          where u.Id == idAutenticado
                                          select a)
                                  .ToListAsync();


            ViewData["IdBox"] = new SelectList(_context.Boxs, "IdBox", "Nome", reserva.IdBox);
            return View(reserva);
        }

        // POST: Reservas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdReserva,D_Entrada,D_Saida,Nr_animais,Nr_registo,IdCliente,IdBox")] Reservas newReserva, int[] AnimalEscolhido )
        {

            // avalia se o array com a lista de animais escolhidos associados à reserva  está vazio ou não
            if (AnimalEscolhido.Length == 0)
            {
                //É gerada uma mensagem de erro
                ModelState.AddModelError("", "É necessário selecionar pelo menos um animal.");
                // obtem o nome da box associado ao animal
                ViewData["IdBox"] = new SelectList(_context.Boxs, "IdBox", "Nome", newReserva.IdBox);
                string idAutenticado = _userManager.GetUserId(User);
                // gerar a lista serviços que podem ser associados à oficina
                ViewBag.ListaDeAnimais = await (from a in _context.Animais.Include(a => a.Cliente)
                                              join c in _context.Clientes on a.IdCliente equals c.IdCliente
                                              join u in _context.Users on c.Email equals u.Email
                                              where u.Id == idAutenticado
                                              select a)
                      .ToListAsync();
                // devolver controlo à View
                return View(newReserva);
            }

            if (id != newReserva.IdReserva)
            {
                return NotFound();
            }
           // ********************************************************************************************
            // dados anteriormente guardados da Reserva
            var reserva = await _context.Reservas
                                       .Where(o => o.IdReserva == id)
                                       .Include(o => o.ListaAnimais)
                                       .FirstOrDefaultAsync();

            // obter a lista dos IDs dos animais associadas à reserva, antes da edição
            var oldListaAnimais = reserva.ListaAnimais
                                           .Select(s => s.IdAnimal)
                                           .ToList();

            // avaliar se o utilizador alterou algum animal associada à reserva
            // adicionados -> lista de animais adicionados
            // retirados   -> lista de animais retirados
            var adicionados = AnimalEscolhido.Except(oldListaAnimais);
            var retirados = oldListaAnimais.Except(AnimalEscolhido.ToList());

            // se algum animal foi adicionado ou retirado
            // é necessário alterar a lista de animais 
            // associada à reserva
            if (adicionados.Any() || retirados.Any())
            {

                if (retirados.Any())
                {
                    // retirar o animal 
                    foreach (int oldAnimal in retirados)
                    {
                        var animalToRemove = reserva.ListaAnimais.FirstOrDefault(c => c.IdAnimal == oldAnimal);
                        reserva.ListaAnimais.Remove(animalToRemove);
                    }
                }
                if (adicionados.Any())
                {
                    // adicionar o animal 
                    foreach (int newAnimal in adicionados)
                    {
                        var animalToAdd = await _context.Animais.FirstOrDefaultAsync(s => s.IdAnimal == newAnimal);
                        reserva.ListaAnimais.Add(animalToAdd);
                    }
                }
            }

            //********************************************************************************************************************


            if (ModelState.IsValid)
            {
                try
                {
                    reserva.D_Entrada = newReserva.D_Entrada;
                    reserva.D_Saida = newReserva.D_Saida;
                    reserva.Nr_animais = AnimalEscolhido.Length;
                    reserva.Nr_registo = newReserva.Nr_registo;
                    reserva.Box = newReserva.Box; 



                    _context.Update(reserva);
                    await _context.SaveChangesAsync();
                }
                catch (Exception)


                {
                    if (!ReservasExists(newReserva.IdReserva))
                    {
                        // id da reserva não encontrado
                        ModelState.AddModelError("", "Não foi possivel guardar o registo na base de dados. Id não encontrado.");
                        return View(newReserva);
                    }
                    else
                    {
                        // id da reserva encontrado
                        ModelState.AddModelError("", "Não foi possivel guardar o registo na base de dados");

                       // throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdBox"] = new SelectList(_context.Boxs, "IdBox", "IdBox", newReserva.IdBox);
            return View(newReserva);
        }

        // GET: Reservas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Reservas == null)
            {
                return NotFound();
            }

            var reservas = await _context.Reservas
                .Include(r => r.Box)
                .Include(r => r.ListaAnimais)
                .FirstOrDefaultAsync(m => m.IdReserva == id);
            if (reservas == null)
            {
                return NotFound();
            }

            return View(reservas);
        }

        // POST: Reservas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Reservas == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Reservas'  is null.");
            }
            var reservas = await _context.Reservas.FindAsync(id);
            if (reservas != null)
            {
                _context.Reservas.Remove(reservas);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservasExists(int id)
        {
          return _context.Reservas.Any(e => e.IdReserva == id);
        }
    }
}
