using Microsoft.EntityFrameworkCore;
using netbullAPI.Entidade;
using netbullAPI.Security.Models;

namespace netbullAPI.Persistencia
{
    public class netbullDBContext : DbContext 
    {
        public netbullDBContext(DbContextOptions<netbullDBContext> opt) : base(opt)
        {
                
        }

        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<Telefone> Telefones { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<Produto> Produtos { get; set; }

        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Item> Itens { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
