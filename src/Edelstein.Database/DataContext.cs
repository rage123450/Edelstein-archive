using System;
using System.Linq;
using Edelstein.Database.Entities;
using Edelstein.Database.Entities.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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

        public void InsertUpdateOrDeleteGraph(object entity)
        {
            if (entity is Character character)
            {
                var existing = Characters
                    .AsNoTracking()
                    .Include(c => c.FunctionKeys)
                    .FirstOrDefault(c => c.ID == character.ID);

                if (existing != null)
                {
                    foreach (var functionKey in existing.FunctionKeys)
                    {
                        if (character.FunctionKeys.All(f => f.ID == functionKey.ID))
                            Remove(functionKey);
                    }
                }
            }
        }

        public override EntityEntry<TEntity> Update<TEntity>(TEntity entity)
        {
            InsertUpdateOrDeleteGraph(entity);
            return base.Update(entity);
        }
    }
}