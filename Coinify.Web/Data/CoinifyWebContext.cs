using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Coinify.Web.Models;

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
    }
}
