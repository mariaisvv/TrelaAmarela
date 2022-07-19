using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Trela_Amarela.Models;

namespace Trela_Amarela.Data
{
    public class ApplicationUser : IdentityUser
    {

        /// <summary>
        /// recolhe a data de registo de um utilizador
        /// </summary>
        public DateTime DataRegisto { get; set; }

        // /// <summary>
        // /// se fizerem isto, estão a adicionar todos os atributos do 'Cliente'
        // /// à tabela de autenticação
        // /// </summary>
        // public Clientes Cliente { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // sedd tabela Box
            modelBuilder.Entity<Boxs>().HasData(
                new Boxs { IdBox = 1, Nome = "Pequena", Dim_Box = "2x1" },
                new Boxs { IdBox = 2, Nome = "Média", Dim_Box = "3x3" },
                new Boxs { IdBox = 3, Nome = "Grande", Dim_Box = "5x5" }

                );
        }

        // definir as tabelas da base de dados
        public DbSet<Clientes> Clientes { get; set; }
        public DbSet<Animais> Animais { get; set; }
        public DbSet<Boxs> Boxs { get; set; }
        public DbSet<Reservas> Reservas { get; set; }


    }
}