using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace SaintSeiya.Inventory
{
    /// <summary>
    /// 플레이어 인벤토리 — 아이템 추가/제거/사용/저장
    /// </summary>
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager Instance { get; private set; }

        [Header("Settings")]
        [SerializeField] private int _maxSlots = 30;

        [Header("Item Database")]
        [SerializeField] private List<ItemData> _itemDatabase = new();

        // 인벤토리 슬롯: itemId → 수량
        private Dictionary<string, int> _inventory = new();

        // 이벤트
        public event System.Action<ItemData, int> OnItemAdded;     // (아이템, 수량)
        public event System.Action<ItemData, int> OnItemRemoved;   // (아이템, 수량)
        public event System.Action<ItemData>      OnItemUsed;      // (아이템)
        public event System.Action                OnInventoryChanged;

        public int MaxSlots => _maxSlots;
        public int UsedSlots => _inventory.Count;

        void Awake()
        {
            if (Instance != null) { Destroy(gameObject); return; }
            Instance = this;
        }

        // ─── 아이템 추가 ────────────────────────────────────────

        public bool AddItem(string itemId, int amount = 1)
        {
            var data = FindItem(itemId);
            if (data == null)
            {
                Debug.LogWarning($"[Inventory] 아이템 없음: {itemId}");
                return false;
            }
            return AddItem(data, amount);
        }

        public bool AddItem(ItemData data, int amount = 1)
        {
            if (data == null || amount <= 0) return false;

            // 슬롯 체크 (새 아이템인 경우)
            if (!_inventory.ContainsKey(data.itemId) && UsedSlots >= _maxSlots)
            {
                Debug.Log("[Inventory] 인벤토리 가득 참!");
                return false;
            }

            if (_inventory.ContainsKey(data.itemId))
            {
                int newAmount = _inventory[data.itemId] + amount;
                _inventory[data.itemId] = Mathf.Min(newAmount, data.maxStack);
            }
            else
            {
                _inventory[data.itemId] = Mathf.Min(amount, data.maxStack);
            }

            OnItemAdded?.Invoke(data, amount);
            OnInventoryChanged?.Invoke();
            Debug.Log($"[Inventory] 추가: {data.itemName} x{amount}");
            return true;
        }

        // ─── 아이템 제거 ────────────────────────────────────────

        public bool RemoveItem(string itemId, int amount = 1)
        {
            if (!_inventory.ContainsKey(itemId)) return false;
            if (_inventory[itemId] < amount) return false;

            var data = FindItem(itemId);
            _inventory[itemId] -= amount;

            if (_inventory[itemId] <= 0)
                _inventory.Remove(itemId);

            if (data != null) OnItemRemoved?.Invoke(data, amount);
            OnInventoryChanged?.Invoke();
            return true;
        }

        // ─── 아이템 사용 ────────────────────────────────────────

        public bool UseItem(string itemId, Characters.CharacterStats target = null)
        {
            var data = FindItem(itemId);
            if (data == null || !HasItem(itemId)) return false;

            if (data.itemType != ItemType.Consumable)
            {
                Debug.Log($"[Inventory] {data.itemName}은 소비 아이템이 아닙니다.");
                return false;
            }

            // 효과 적용
            if (target != null)
            {
                if (data.hpRestore > 0)
                    target.Heal(data.hpRestore);

                if (data.hpRestoreRatio > 0)
                    target.Heal(target.MaxHP * data.hpRestoreRatio);

                if (data.cosmosRestore > 0)
                    target.cosmos?.GainCosmos(data.cosmosRestore);
            }

            RemoveItem(itemId);
            OnItemUsed?.Invoke(data);
            Debug.Log($"[Inventory] 사용: {data.itemName}");
            return true;
        }

        // ─── 조회 ───────────────────────────────────────────────

        public bool HasItem(string itemId, int amount = 1)
            => _inventory.TryGetValue(itemId, out int count) && count >= amount;

        public int GetItemCount(string itemId)
            => _inventory.TryGetValue(itemId, out int count) ? count : 0;

        public List<(ItemData data, int count)> GetAllItems()
        {
            var result = new List<(ItemData, int)>();
            foreach (var kv in _inventory)
            {
                var data = FindItem(kv.Key);
                if (data != null) result.Add((data, kv.Value));
            }
            return result;
        }

        public List<(ItemData data, int count)> GetItemsByType(ItemType type)
            => GetAllItems().Where(x => x.data.itemType == type).ToList();

        // ─── 저장/불러오기 ──────────────────────────────────────

        public string Serialize()
            => JsonConvert.SerializeObject(_inventory);

        public void Deserialize(string json)
        {
            if (string.IsNullOrEmpty(json)) return;
            _inventory = JsonConvert.DeserializeObject<Dictionary<string, int>>(json)
                         ?? new Dictionary<string, int>();
            OnInventoryChanged?.Invoke();
        }

        // ─── 유틸리티 ───────────────────────────────────────────

        private ItemData FindItem(string itemId)
            => _itemDatabase.FirstOrDefault(d => d.itemId == itemId);

        public void RegisterItem(ItemData data)
        {
            if (!_itemDatabase.Any(d => d.itemId == data.itemId))
                _itemDatabase.Add(data);
        }
    }
}
