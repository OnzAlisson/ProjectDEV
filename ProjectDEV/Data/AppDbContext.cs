using Microsoft.EntityFrameworkCore;
using ProjectDEV.Models;

namespace ProjectDEV.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Unidade> Unidades { get; set; }
        public DbSet<Colaborador> Colaboradores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurações do usuário
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Login)
                .IsUnique();

            // Configurações da unidade
            modelBuilder.Entity<Unidade>()
                .HasIndex(u => u.Codigo)
                .IsUnique();

            // Configurações do colaborador
            modelBuilder.Entity<Colaborador>()
                .HasOne(c => c.Unidade)
                .WithMany(u => u.Colaboradores)
                .HasForeignKey(c => c.UnidadeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Colaborador>()
                .HasOne(c => c.Usuario)
                .WithOne(u => u.Colaborador)
                .HasForeignKey<Colaborador>(c => c.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
} 