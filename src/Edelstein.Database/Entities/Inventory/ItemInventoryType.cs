namespace Edelstein.Database.Entities.Inventory
{
    public enum ItemInventoryType
    {
        Equip = 0x1,
        Consume = 0x2,
        Install = 0x3,
        Etc = 0x4,
        Cash = 0x5,

        Equipped = 0x6,
        EquippedCash = 0x7,
        EquippedDragon = 0x8,
        EquippedMechanic = 0x9
    }
}