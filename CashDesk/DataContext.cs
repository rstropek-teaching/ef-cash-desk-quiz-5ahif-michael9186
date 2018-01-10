using CashDesk.Model;
using Microsoft.EntityFrameworkCore;

namespace CashDesk
{
    public class DataContext : DbContext
    {

        public DbSet<Member> Members { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<Deposit> Deposits { get; set; }
        //     public DbSet<DepositStatistics> DepositStatistics { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            optionBuilder.UseInMemoryDatabase("CashDesk");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Member>()
                .HasIndex(i => i.LastName)
                .IsUnique();
        }

    }
}