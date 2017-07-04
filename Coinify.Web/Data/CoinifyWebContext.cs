using Microsoft.EntityFrameworkCore;

namespace Coinify.Web.Models
{
    public class CoinifyWebContext : DbContext
    {
        public CoinifyWebContext (DbContextOptions<CoinifyWebContext> options)
            : base(options)
        {
        }

        public DbSet<Coinify.Web.Models.Coin> Coin { get; set; }

        public DbSet<Coinify.Web.Models.Note> Note { get; set; }

        public DbSet<Coinify.Web.Models.User> User { get; set; }

        public DbSet<Coinify.Web.Models.CoinSize> CoinSize { get; set; }

        public DbSet<Coinify.Web.Models.AutomatedTellerMachine> AutomatedTellerMachine { get; set; }
    }
}
