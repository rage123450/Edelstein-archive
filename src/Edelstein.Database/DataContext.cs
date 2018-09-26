using System;
using System.Collections.Generic;
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

            modelBuilder.Entity<Account>()
                .HasMany(a => a.Characters)
                .WithOne(c => c.Account)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Account>()
                .HasMany<AccountData>(a => a.Data)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Character>()
                .HasMany(c => c.FunctionKeys)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Character>()
                .HasMany(c => c.Inventories)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ItemInventory>()
                .HasMany(i => i.Items)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        }

        public void InsertUpdateOrDeleteGraph(object entity)
        {
            if (entity is Character character)
            {
                var existing = Characters
                    .AsNoTracking()
                    .Include(c => c.FunctionKeys)
                    .Include(c => c.Inventories)
                    .ThenInclude(c => c.Items)
                    .FirstOrDefault(c => c.ID == character.ID);

                if (existing != null)
                {
                    foreach (var functionKey in existing.FunctionKeys)
                    {
                        if (character.FunctionKeys.All(f => f.ID == functionKey.ID))
                            Remove(functionKey);
                    }

                    var existingItems = existing.Inventories.SelectMany(i => i.Items);
                    var currentItems = character.Inventories.SelectMany(i => i.Items);

                    foreach (var existingItem in existingItems)
                    {
                        if (currentItems.All(f => f.ID == existingItem.ID))
                            Remove(existingItem);
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