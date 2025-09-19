using EsameFinale.Models;
using Microsoft.EntityFrameworkCore;

namespace EsameFinale.Context
{
    public class LibreriaContext : DbContext
        {
            public LibreriaContext(DbContextOptions<LibreriaContext> options) : base(options) { }

            public DbSet<Playlist> Playlists => Set<Playlist>();
            public DbSet<Canzone> Canzoni => Set<Canzone>();
    }

}
