using MathGameApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace MathGameApplication.DbContexts
{
    public class MathGameContext : DbContext
    {
        public MathGameContext(DbContextOptions<MathGameContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Round> Rounds { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

                 modelBuilder.Entity<Round>().HasData(
                new Round
                {
                    Id = 1,
                    Expression = "10*3=20",
                },
                new Round
                {
                    Id = 2,
                    Expression = "2-9=-7",
                },
                new Round
                {
                    Id = 3,
                    Expression = "4+6=12",
                },
                new Round
                {
                    Id = 4,
                    Expression = "10/5=3",
                },
                 new Round
                 {
                     Id = 5,
                     Expression = "2*2=5",
                 },
                 new Round
                 {
                     Id = 6,
                     Expression = "4*5=?",
                 }
            );
        }

    }
}