using UnityEngine;

namespace SaintSeiya.Inventory
{
    public enum ItemType
    {
        Consumable,     // 회복 아이템
        Equipment,      // 장비 (성의 강화)
        KeyItem,        // 중요 아이템 (퀘스트)
        Material        // 재료
    }

    public enum ItemRarity
    {
        Common,         // 일반
        Uncommon,       // 고급
        Rare,           // 희귀
        Legendary       // 전설
    }

    [CreateAssetMenu(fileName = "Item_New", menuName = "SaintSeiya/Item")]
    public class ItemData : ScriptableObject
    {
        [Header("Info")]
        public string itemId;
        public string itemName;
        [TextArea] public string description;
        public ItemType itemType;
        public ItemRarity rarity;

        [Header("Stack")]
        public bool isStackable = true;
        public int maxStack = 99;

        [Header("Effect (Consumable)")]
        public float hpRestore      = 0f;   // HP 회복량
        public float cosmosRestore  = 0f;   // 코스모 회복량
        public float hpRestoreRatio = 0f;   // HP 최대치 % 회복

        [Header("Visual")]
        public Sprite icon;

        [Header("Value")]
        public int buyPrice  = 100;
        public int sellPrice = 30;
    }
}
