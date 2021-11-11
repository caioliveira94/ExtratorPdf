using Data.Entities;
using Data.EntityConfiguration;
using Microsoft.EntityFrameworkCore;

namespace Infra.Context
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-ENEBUAP\\SQLEXPRESS;initial Catalog=SmartSAT;Integrated Security=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration(new ExtratoClienteConfiguration());

            modelBuilder.Entity<ExtratoCliente>()
            .Property(p => p.Id)
            .ValueGeneratedOnAdd();

            modelBuilder.Entity<Video>()
            .Property(p => p.Id)
            .ValueGeneratedOnAdd();

            modelBuilder.Entity<PosicaoCliente>()
            .Property(p => p.Id)
            .ValueGeneratedOnAdd();

            modelBuilder.Entity<MovimentacaoCliente>()
            .Property(p => p.Id)
            .ValueGeneratedOnAdd();
        }

        public DbSet<Video> Videos { get; set; }
        public DbSet<ExtratoCliente> ExtratoClientes { get; set; }
        public DbSet<PosicaoCliente> PosicaoClientes { get; set; }
        public DbSet<MovimentacaoCliente> MovimentacaoClientes { get; set; }
    }
}
