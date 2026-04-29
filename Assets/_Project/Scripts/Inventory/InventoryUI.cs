using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

namespace SaintSeiya.Inventory
{
    /// <summary>
    /// 인벤토리 UI — 아이템 목록 표시 및 사용
    /// </summary>
    public class InventoryUI : MonoBehaviour
    {
        [Header("Panel")]
        [SerializeField] private GameObject _panel;
        [SerializeField] private KeyCode _toggleKey = KeyCode.I;

        [Header("Grid")]
        [SerializeField] private Transform _itemGrid;
        [SerializeField] private GameObject _itemSlotPrefab;

        [Header("Detail Panel")]
        [SerializeField] private GameObject _detailPanel;
        [SerializeField] private Image _detailIcon;
        [SerializeField] private TextMeshProUGUI _detailName;
        [SerializeField] private TextMeshProUGUI _detailDescription;
        [SerializeField] private TextMeshProUGUI _detailCount;
        [SerializeField] private Button _useButton;
        [SerializeField] private Button _dropButton;

        [Header("Slot Count")]
        [SerializeField] private TextMeshProUGUI _slotCountText;

        [Header("Tabs")]
        [SerializeField] private Button _tabAll;
        [SerializeField] private Button _tabConsumable;
        [SerializeField] private Button _tabKeyItem;

        private ItemData _selectedItem;
        private ItemType _currentTab = ItemType.Consumable;
        private bool _showAll = true;

        void Start()
        {
            _panel?.SetActive(false);
            _detailPanel?.SetActive(false);

            _useButton?.onClick.AddListener(OnUseClicked);
            _dropButton?.onClick.AddListener(OnDropClicked);
            _tabAll?.onClick.AddListener(() => { _showAll = true; RefreshGrid(); });
            _tabConsumable?.onClick.AddListener(() => { _showAll = false; _currentTab = ItemType.Consumable; RefreshGrid(); });
            _tabKeyItem?.onClick.AddListener(() => { _showAll = false; _currentTab = ItemType.KeyItem; RefreshGrid(); });

            if (InventoryManager.Instance != null)
                InventoryManager.Instance.OnInventoryChanged += RefreshGrid;
        }

        void OnDestroy()
        {
            if (InventoryManager.Instance != null)
                InventoryManager.Instance.OnInventoryChanged -= RefreshGrid;
        }

        void Update()
        {
            if (Input.GetKeyDown(_toggleKey)) Toggle();
        }

        // ─── 패널 토글 ──────────────────────────────────────────

        public void Toggle()
        {
            bool next = !_panel.activeSelf;
            _panel?.SetActive(next);
            if (next) RefreshGrid();

            // 게임 일시정지/재개
            if (next) Core.GameManager.Instance?.PauseGame();
            else      Core.GameManager.Instance?.ResumeGame();
        }

        // ─── 그리드 갱신 ────────────────────────────────────────

        private void RefreshGrid()
        {
            if (_itemGrid == null || InventoryManager.Instance == null) return;

            // 기존 슬롯 제거
            foreach (Transform child in _itemGrid)
                Destroy(child.gameObject);

            // 아이템 목록 가져오기
            var items = _showAll
                ? InventoryManager.Instance.GetAllItems()
                : InventoryManager.Instance.GetItemsByType(_currentTab);

            // 슬롯 생성
            foreach (var (data, count) in items)
            {
                var slot = Instantiate(_itemSlotPrefab, _itemGrid);
                var slotUI = slot.GetComponent<ItemSlotUI>();
                slotUI?.Setup(data, count, OnSlotClicked);
            }

            // 슬롯 카운트 표시
            if (_slotCountText != null && InventoryManager.Instance != null)
                _slotCountText.SetText($"{InventoryManager.Instance.UsedSlots} / {InventoryManager.Instance.MaxSlots}");
        }

        // ─── 슬롯 클릭 ──────────────────────────────────────────

        private void OnSlotClicked(ItemData data)
        {
            _selectedItem = data;
            ShowDetail(data);
        }

        private void ShowDetail(ItemData data)
        {
            _detailPanel?.SetActive(true);
            if (_detailIcon != null && data.icon != null) _detailIcon.sprite = data.icon;
            _detailName?.SetText(data.itemName);
            _detailDescription?.SetText(data.description);
            _detailCount?.SetText($"보유: {InventoryManager.Instance?.GetItemCount(data.itemId)}개");

            // 소비 아이템만 사용 버튼 활성화
            if (_useButton != null)
                _useButton.gameObject.SetActive(data.itemType == ItemType.Consumable);

            // 중요 아이템은 버리기 불가
            if (_dropButton != null)
                _dropButton.gameObject.SetActive(data.itemType != ItemType.KeyItem);
        }

        private void OnUseClicked()
        {
            if (_selectedItem == null) return;

            // 전투 중이면 전투 타겟, 아니면 플레이어
            var player = GameObject.FindGameObjectWithTag("Player")
                ?.GetComponent<Characters.CharacterStats>();

            bool used = InventoryManager.Instance?.UseItem(_selectedItem.itemId, player) ?? false;
            if (used)
            {
                int remaining = InventoryManager.Instance?.GetItemCount(_selectedItem.itemId) ?? 0;
                if (remaining <= 0) _detailPanel?.SetActive(false);
                else ShowDetail(_selectedItem);
            }
        }

        private void OnDropClicked()
        {
            if (_selectedItem == null) return;
            InventoryManager.Instance?.RemoveItem(_selectedItem.itemId, 1);
            _detailPanel?.SetActive(false);
            _selectedItem = null;
        }
    }
}
