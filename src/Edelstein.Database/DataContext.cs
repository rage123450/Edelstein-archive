using Edelstein.Database.Entities;
using Edelstein.Database.Entities.Inventory;
using Microsoft.EntityFrameworkCore;

namespace Edelstein.Database
{
    public class DataContext : DbContext
    {
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Character> Characters { get; set; }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ItemSlotEquip>().HasBaseType<ItemSlot>();
            modelBuilder.Entity<ItemSlotBundle>().HasBaseType<ItemSlot>();
        }
    }
}