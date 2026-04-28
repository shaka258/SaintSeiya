using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace SaintSeiya.Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager Instance { get; private set; }

        [SerializeField] private int _maxSlots = 30;
        [SerializeField] private List<ItemData> _itemDatabase = new();

        private Dictionary<string, int> _inventory = new();

        public event System.Action<ItemData, int> OnItemAdded;
        public event System.Action<ItemData, int> OnItemRemoved;
        public event System.Action<ItemData>      OnItemUsed;
        public event System.Action                OnInventoryChanged;

        public int MaxSlots  => _maxSlots;
        public int UsedSlots => _inventory.Count;

        void Awake() { if (Instance != null) { Destroy(gameObject); return; } Instance = this; }

        public bool AddItem(ItemData data, int amount = 1)
        {
            if (data == null || amount <= 0) return false;
            if (!_inventory.ContainsKey(data.itemId) && UsedSlots >= _maxSlots) return false;
            _inventory[data.itemId] = Mathf.Min((_inventory.TryGetValue(data.itemId, out int cur) ? cur : 0) + amount, data.maxStack);
            OnItemAdded?.Invoke(data, amount); OnInventoryChanged?.Invoke(); return true;
        }

        public bool AddItem(string itemId, int amount = 1) { var d = FindItem(itemId); return d != null && AddItem(d, amount); }

        public bool RemoveItem(string itemId, int amount = 1)
        {
            if (!_inventory.TryGetValue(itemId, out int cur) || cur < amount) return false;
            var data = FindItem(itemId);
            _inventory[itemId] -= amount;
            if (_inventory[itemId] <= 0) _inventory.Remove(itemId);
            if (data != null) OnItemRemoved?.Invoke(data, amount);
            OnInventoryChanged?.Invoke(); return true;
        }

        public bool UseItem(string itemId, Characters.CharacterStats target = null)
        {
            var data = FindItem(itemId);
            if (data == null || !HasItem(itemId) || data.itemType != ItemType.Consumable) return false;
            if (target != null)
            {
                if (data.hpRestore > 0) target.Heal(data.hpRestore);
                if (data.hpRestoreRatio > 0) target.Heal(target.MaxHP * data.hpRestoreRatio);
                if (data.cosmosRestore > 0) target.cosmos?.GainCosmos(data.cosmosRestore);
            }
            RemoveItem(itemId); OnItemUsed?.Invoke(data); return true;
        }

        public bool HasItem(string itemId, int amount = 1) => _inventory.TryGetValue(itemId, out int c) && c >= amount;
        public int  GetItemCount(string itemId) => _inventory.TryGetValue(itemId, out int c) ? c : 0;

        public List<(ItemData data, int count)> GetAllItems()
            => _inventory.Select(kv => (FindItem(kv.Key), kv.Value)).Where(x => x.Item1 != null).ToList();

        public List<(ItemData data, int count)> GetItemsByType(ItemType type)
            => GetAllItems().Where(x => x.data.itemType == type).ToList();

        public string Serialize() => JsonConvert.SerializeObject(_inventory);
        public void Deserialize(string json) { if (!string.IsNullOrEmpty(json)) { _inventory = JsonConvert.DeserializeObject<Dictionary<string, int>>(json) ?? new(); OnInventoryChanged?.Invoke(); } }

        public void RegisterItem(ItemData data) { if (!_itemDatabase.Any(d => d.itemId == data.itemId)) _itemDatabase.Add(data); }
        private ItemData FindItem(string id) => _itemDatabase.FirstOrDefault(d => d.itemId == id);
    }
}
