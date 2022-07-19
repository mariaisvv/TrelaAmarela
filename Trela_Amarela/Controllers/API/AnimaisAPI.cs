using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trela_Amarela.Data;
using Trela_Amarela.Models;

namespace Trela_Amarela.Controllers.API
{
    [Route("API/[controller]")]
    [ApiController]
    public class AnimaisAPI : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        private readonly IWebHostEnvironment _caminho;

        public AnimaisAPI(ApplicationDbContext context, IWebHostEnvironment caminho)
        {
            _context = context;
            _caminho = caminho;
        }

        // GET: api/AnimaisAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AnimaisAPIviewModel>>> GetAnimais()
        {
            var listaAnimais = await _context.Animais
                .Select(a => new AnimaisAPIviewModel
                {
                    IdAnimal = a.IdAnimal,
                    Raca = a.Raca,
                    Porte = a.Porte,
                    DataNasc = a.DataNasc,
                    Vacinacao = a.Vacinacao,
                    Desparasitacao = a.Desparasitacao,
                    N_Especiais = a.N_Especiais,
                    Nr_registo = a.Nr_registo,
                    Nr_chip = a.Nr_chip,
                    Foto = a.Foto

                })
                .OrderBy(f => f.IdAnimal)
                .ToListAsync();
            return listaAnimais;
        }

        // GET: API/AnimaisAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Animais>> GetAnimais(int id)
        {
            var animais = await _context.Animais.FindAsync(id);

            if (animais == null)
            {
                return NotFound();
            }

            return animais;
        }

        // PUT: API/AnimaisAPI/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAnimais(int id, Animais animal)
        {
            if (id != animal.IdAnimal)
            {
                return BadRequest();
            }

            _context.Entry(animal).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnimaisExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: API/AnimaisAPI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Animais>> PostVeiculos([FromForm] Animais animal)
        {

            animal.IdCliente = 3;
            _context.Animais.Add(animal);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAnimais", new { id = animal.IdAnimal }, animal);
        }

        // DELETE: API/AnimaisAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnimais(int id)
        {
            var animal = await _context.Animais.FindAsync(id);
            if (animal == null)
            {
                return NotFound();
            }

            _context.Animais.Remove(animal);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AnimaisExists(int id)
        {
            return _context.Animais.Any(v => v.IdAnimal == id);
        }
    }
}