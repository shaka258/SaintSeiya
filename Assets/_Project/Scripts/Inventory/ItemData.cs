using UnityEngine;

namespace SaintSeiya.Inventory
{
    public enum ItemType { Consumable, Equipment, KeyItem, Material }
    public enum ItemRarity { Common, Uncommon, Rare, Legendary }

    [CreateAssetMenu(fileName = "Item_New", menuName = "SaintSeiya/Item")]
    public class ItemData : ScriptableObject
    {
        public string itemId;
        public string itemName;
        [TextArea] public string description;
        public ItemType itemType;
        public ItemRarity rarity;
        public bool isStackable = true;
        public int maxStack = 99;
        public float hpRestore = 0f;
        public float cosmosRestore = 0f;
        public float hpRestoreRatio = 0f;
        public Sprite icon;
        public int buyPrice = 100;
        public int sellPrice = 30;
    }
}
