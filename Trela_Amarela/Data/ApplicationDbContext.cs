using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Trela_Amarela.Models;

namespace Trela_Amarela.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // definir as tabelas da base de dados
        public DbSet<Clientes> Clientes { get; set; }
        public DbSet<Animais> Animais { get; set; }
        public DbSet<Boxs> Boxs { get; set; }
        public DbSet<Reservas> Reservas { get; set; }


    }
}