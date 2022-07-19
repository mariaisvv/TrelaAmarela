using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Trela_Amarela.Data;
using Trela_Amarela.Models;

namespace Trela_Amarela.Controllers
{
    [Authorize] // esta 'anotação' garante que só as pessoas autenticadas têm acesso aos recursos
    public class AnimaisController : Controller
    {
        private readonly ApplicationDbContext _context;

        // <summary>
        /// esta variável recolhe os dados da pessoa que se autenticou
        /// </summary>
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webhost;

        public AnimaisController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment webhost)
        {
            _context = context;
            _userManager = userManager;
            _webhost = webhost;
        }

        // GET: Animais, apresentar todos os cães do cliente
        public async Task<IActionResult> Index()
        {
            // var. auxiliar
            string idDaPessoaAutenticada = _userManager.GetUserId(User);
            var animais = await (from v in _context.Animais.Include(v => v.Cliente)
                                  join c in _context.Clientes on v.IdCliente equals c.IdCliente
                                  join u in _context.Users on c.Email equals u.Email
                                  where u.Id == idDaPessoaAutenticada
                                  select v)
                                  .ToListAsync();

            return View(animais);
        }

        // GET: Animais/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Animais == null)
            {
                return NotFound();
            }

            var animais = await _context.Animais
                .Include(a => a.Cliente)
                .FirstOrDefaultAsync(m => m.IdAnimal == id);
            if (animais == null)
            {
                return NotFound();
            }

            return View(animais);
        }

        // GET: Animais/Create
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> CreateAsync()
        {
            int idClienteAutenticado = (await _context.Clientes.Where(c => c.UserName == _userManager.GetUserId(User)).FirstOrDefaultAsync()).IdCliente;

            ViewData["IdAnimal"] = new SelectList(_context.Animais.Include(v => v.Cliente).Where(v => v.IdCliente == idClienteAutenticado), "idAnimal", "Nome");
            return View();
        }

        // POST: Animais/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdAnimal,Nome,DataNasc,Porte,Raca,Vacinacao,Desparasitacao,N_Especiais,Nr_registo,Nr_chip,Foto")] Animais animal, IFormFile imgFile)
        {
            animal.Foto = imgFile.FileName;

            //_webhost.WebRootPath vai ter o path para a pasta wwwroot
            var saveimg = Path.Combine(_webhost.WebRootPath, "fotos", imgFile.FileName);

            var imgext = Path.GetExtension(imgFile.FileName);

            if (imgext == ".jpg" || imgext == ".png")
            {
                using (FileStream uploadimg = new FileStream(saveimg, FileMode.Create))
                {
                    await imgFile.CopyToAsync(uploadimg);

                }
            }




            if (ModelState.IsValid)
            {
                // obter os dados da pessoa autenticada
                Clientes cliente = _context.Clientes.Where(c => c.UserName == _userManager.GetUserId(User)).FirstOrDefault();
                // adicionar o cliente ao animal
                animal.Cliente = cliente;


                _context.Add(animal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
              return View(animal);
        }

        // GET: Animais/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Animais == null)
            {
                return NotFound();
            }

            // e, o ID fornecido pertence a um animal que pertence ao Utilizador que está a usar o sistema?
            int idClienteAutenticado = (await _context.Clientes.Where(c => c.UserName == _userManager.GetUserId(User)).FirstOrDefaultAsync()).IdCliente;

            var animal = await _context.Animais.Where(v => v.IdAnimal == id && v.IdCliente == idClienteAutenticado).FirstOrDefaultAsync();
            if (animal == null)
            {
                return NotFound();
            }
             return View(animal);
        }

        // POST: Animais/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdAnimal,Nome,DataNasc,Porte,Raca,Vacinacao,Desparasitacao,N_Especiais,Nr_registo,Nr_chip,Foto,IdCliente")] Animais animal)
        {
            if (id != animal.IdAnimal)
            {
                return NotFound();
            }

            // recuperar o ID do utilizador (cliente) que está autenticado
            // e reassociar esse ID ao animal
            int idClienteAutenticado = (await _context.Clientes.Where(c => c.UserName == _userManager.GetUserId(User)).FirstOrDefaultAsync()).IdCliente;
            animal.IdCliente = idClienteAutenticado;


            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(animal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnimaisExists(animal.IdAnimal))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "IdCliente", "CodPostal", animal.IdCliente);
            return View(animal);
        }

        // GET: Animais/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Animais == null)
            {
                return NotFound();
            }

            var animal = await _context.Animais
                .Include(a => a.Cliente)
                .FirstOrDefaultAsync(m => m.IdAnimal == id);
            if (animal == null)
            {
                return NotFound();
            }

            return View(animal);
        }

        // POST: Animais/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Animais == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Animais'  is null.");
            }
            var animal = await _context.Animais.FindAsync(id);
            if (animal != null)
            {
                _context.Animais.Remove(animal);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnimaisExists(int id)
        {
          return _context.Animais.Any(e => e.IdAnimal == id);
        }
    }
}
