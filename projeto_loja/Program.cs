using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace ProjetoLoja
{
    public class Usuario
    {
        public int UsuarioId { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public ICollection<Compra> Compras { get; set; }
    }

    public class Compra
    {
        public int CompraId { get; set; }
        public DateTime DataCompra { get; set; }
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        public ICollection<Itens> Itens { get; set; }
    }

    public class Itens
    {
        public int ItensId { get; set; }
        public int Quantidade { get; set; }
        public int CompraId { get; set; }
        public Compra Compra { get; set; }
        public int ProdutoId { get; set; }
        public Produto Produto { get; set; }
    }

    public class Produto
    {
        public int ProdutoId { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }
        public ICollection<Itens> Itens { get; set; }
        public ICollection<ProdutoCategoria> ProdutoCategorias { get; set; }
    }

    public class Categoria
    {
        public int CategoriaId { get; set; }
        public string Descricao { get; set; }
        public ICollection<ProdutoCategoria> ProdutoCategorias { get; set; }
    }

    public class ProdutoCategoria
    {
        public int IdProdutoCategoria { get; set; }
        public int ProdutoId { get; set; }
        public Produto Produto { get; set; }
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }
    }

    public class LojaContext : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Compra> Compras { get; set; }
        public DbSet<Itens> Itens { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<ProdutoCategoria> ProdutoCategorias { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("YourConnectionStringHere");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProdutoCategoria>()
                .HasKey(pc => new { pc.ProdutoId, pc.CategoriaId });

            modelBuilder.Entity<ProdutoCategoria>()
                .HasOne(pc => pc.Produto)
                .WithMany(p => p.ProdutoCategorias)
                .HasForeignKey(pc => pc.ProdutoId);

            modelBuilder.Entity<ProdutoCategoria>()
                .HasOne(pc => pc.Categoria)
                .WithMany(c => c.ProdutoCategorias)
                .HasForeignKey(pc => pc.CategoriaId);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new LojaContext())
            {
                context.Database.EnsureCreated();
            }
        }
    }
}

