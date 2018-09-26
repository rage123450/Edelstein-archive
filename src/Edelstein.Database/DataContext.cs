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
        }

        public void InsertUpdateOrDeleteGraph(object entity)
        {
            if (entity is Character character)
            {
                var existing = Characters
                    .AsNoTracking()
                    .Include(c => c.FunctionKeys)
                    .Include(c => c.InventoryEquipped)
                    .ThenInclude(c => c.Items)
                    .Include(c => c.InventoryEquippedCash)
                    .ThenInclude(c => c.Items)
                    .Include(c => c.InventoryEquip)
                    .ThenInclude(c => c.Items)
                    .Include(c => c.InventoryConsume)
                    .ThenInclude(c => c.Items)
                    .Include(c => c.InventoryInstall)
                    .ThenInclude(c => c.Items)
                    .Include(c => c.InventoryEtc)
                    .ThenInclude(c => c.Items)
                    .Include(c => c.InventoryCash)
                    .ThenInclude(c => c.Items)
                    .FirstOrDefault(c => c.ID == character.ID);

                if (existing != null)
                {
                    foreach (var functionKey in existing.FunctionKeys)
                    {
                        if (character.FunctionKeys.All(f => f.ID == functionKey.ID))
                            Remove(functionKey);
                    }

                    var existingItems = new List<ItemSlot>();
                    existingItems.AddRange(existing.InventoryEquipped.Items);
                    existingItems.AddRange(existing.InventoryEquippedCash.Items);
                    existingItems.AddRange(existing.InventoryEquip.Items);
                    existingItems.AddRange(existing.InventoryConsume.Items);
                    existingItems.AddRange(existing.InventoryInstall.Items);
                    existingItems.AddRange(existing.InventoryEtc.Items);
                    existingItems.AddRange(existing.InventoryCash.Items);
                    var currentItems = new List<ItemSlot>();
                    currentItems.AddRange(character.InventoryEquipped.Items);
                    currentItems.AddRange(character.InventoryEquippedCash.Items);
                    currentItems.AddRange(character.InventoryEquip.Items);
                    currentItems.AddRange(character.InventoryConsume.Items);
                    currentItems.AddRange(character.InventoryInstall.Items);
                    currentItems.AddRange(character.InventoryEtc.Items);
                    currentItems.AddRange(character.InventoryCash.Items);

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