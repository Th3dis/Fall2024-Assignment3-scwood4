using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Fall2024_Assignment3_scwood4.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Models.Movie> Movies { get; set; }
        public DbSet<Models.Actor> Actors { get; set; }
        public DbSet<Models.ActorMovie> ActorMovies { get; set; }
    }
}
