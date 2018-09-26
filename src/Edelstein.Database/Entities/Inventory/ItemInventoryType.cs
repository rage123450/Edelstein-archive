namespace Edelstein.Database.Entities.Inventory
{
    public enum ItemInventoryType
    {
        Equip = 0x0,
        Consume = 0x1,
        Install = 0x2,
        Etc = 0x3,
        Cash = 0x4,

        Equipped = 0x5,
        EquippedCash = 0x6,
        EquippedDragon = 0x7,
        EquippedMechanic = 0x8
    }
}