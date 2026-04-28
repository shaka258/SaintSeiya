using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

namespace SaintSeiya.Inventory
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private KeyCode _toggleKey = KeyCode.I;
        [SerializeField] private Transform _itemGrid;
        [SerializeField] private GameObject _itemSlotPrefab;
        [SerializeField] private GameObject _detailPanel;
        [SerializeField] private Image _detailIcon;
        [SerializeField] private TextMeshProUGUI _detailName;
        [SerializeField] private TextMeshProUGUI _detailDescription;
        [SerializeField] private TextMeshProUGUI _detailCount;
        [SerializeField] private Button _useButton;
        [SerializeField] private Button _dropButton;
        [SerializeField] private TextMeshProUGUI _slotCountText;

        private ItemData _selectedItem;

        void Start()
        {
            _panel?.SetActive(false); _detailPanel?.SetActive(false);
            _useButton?.onClick.AddListener(OnUseClicked);
            _dropButton?.onClick.AddListener(OnDropClicked);
            if (InventoryManager.Instance != null) InventoryManager.Instance.OnInventoryChanged += RefreshGrid;
        }

        void OnDestroy() { if (InventoryManager.Instance != null) InventoryManager.Instance.OnInventoryChanged -= RefreshGrid; }
        void Update() { if (Input.GetKeyDown(_toggleKey)) Toggle(); }

        public void Toggle()
        {
            bool next = !_panel.activeSelf; _panel?.SetActive(next);
            if (next) { RefreshGrid(); Core.GameManager.Instance?.PauseGame(); }
            else Core.GameManager.Instance?.ResumeGame();
        }

        private void RefreshGrid()
        {
            if (_itemGrid == null || InventoryManager.Instance == null) return;
            foreach (Transform child in _itemGrid) Destroy(child.gameObject);
            foreach (var (data, count) in InventoryManager.Instance.GetAllItems())
            {
                var slot = Instantiate(_itemSlotPrefab, _itemGrid);
                slot.GetComponent<ItemSlotUI>()?.Setup(data, count, OnSlotClicked);
            }
            if (_slotCountText != null) _slotCountText.SetText($"{InventoryManager.Instance.UsedSlots} / {InventoryManager.Instance.MaxSlots}");
        }

        private void OnSlotClicked(ItemData data)
        {
            _selectedItem = data; _detailPanel?.SetActive(true);
            if (_detailIcon != null && data.icon != null) _detailIcon.sprite = data.icon;
            _detailName?.SetText(data.itemName); _detailDescription?.SetText(data.description);
            _detailCount?.SetText($"보유: {InventoryManager.Instance?.GetItemCount(data.itemId)}개");
            _useButton?.gameObject.SetActive(data.itemType == ItemType.Consumable);
            _dropButton?.gameObject.SetActive(data.itemType != ItemType.KeyItem);
        }

        private void OnUseClicked()
        {
            if (_selectedItem == null) return;
            var player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Characters.CharacterStats>();
            if (InventoryManager.Instance?.UseItem(_selectedItem.itemId, player) == true)
            {
                if (InventoryManager.Instance.GetItemCount(_selectedItem.itemId) <= 0) { _detailPanel?.SetActive(false); _selectedItem = null; }
                else OnSlotClicked(_selectedItem);
            }
        }

        private void OnDropClicked()
        {
            if (_selectedItem == null) return;
            InventoryManager.Instance?.RemoveItem(_selectedItem.itemId);
            _detailPanel?.SetActive(false); _selectedItem = null;
        }
    }
}
