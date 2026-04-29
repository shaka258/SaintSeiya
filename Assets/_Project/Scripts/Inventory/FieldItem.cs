using UnityEngine;

namespace SaintSeiya.Inventory
{
    /// <summary>
    /// 필드에 배치된 아이템 오브젝트
    /// 플레이어가 접근해 F키로 획득
    /// </summary>
    public class FieldItem : MonoBehaviour
    {
        [Header("Item")]
        [SerializeField] private ItemData _itemData;
        [SerializeField] private int _amount = 1;

        [Header("Visual")]
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private GameObject _pickupPrompt;  // "F키로 획득" 텍스트
        [SerializeField] private float _bobHeight  = 0.15f; // 둥실둥실 높이
        [SerializeField] private float _bobSpeed   = 2f;

        private Vector3 _startPos;
        private bool _playerInRange;
        private bool _isPickedUp;

        void Start()
        {
            _startPos = transform.position;
            _pickupPrompt?.SetActive(false);

            // 아이콘 스프라이트 세팅
            if (_spriteRenderer != null && _itemData?.icon != null)
                _spriteRenderer.sprite = _itemData.icon;

            Characters.PlayerController.OnInteractTriggered += OnPlayerInteract;
        }

        void OnDestroy()
        {
            Characters.PlayerController.OnInteractTriggered -= OnPlayerInteract;
        }

        void Update()
        {
            if (_isPickedUp) return;

            // 둥실둥실 애니메이션
            float y = _startPos.y + Mathf.Sin(Time.time * _bobSpeed) * _bobHeight;
            transform.position = new Vector3(_startPos.x, y, _startPos.z);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            _playerInRange = true;
            _pickupPrompt?.SetActive(true);
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            _playerInRange = false;
            _pickupPrompt?.SetActive(false);
        }

        private void OnPlayerInteract(GameObject target)
        {
            if (!_playerInRange || _isPickedUp) return;
            if (target != gameObject) return;
            Pickup();
        }

        private void Pickup()
        {
            if (_itemData == null || _isPickedUp) return;

            bool success = InventoryManager.Instance?.AddItem(_itemData, _amount) ?? false;
            if (!success) return;

            _isPickedUp = true;
            _pickupPrompt?.SetActive(false);

            // 획득 이펙트 후 삭제
            // TODO: 파티클 이펙트 연동
            Debug.Log($"[FieldItem] 획득: {_itemData.itemName} x{_amount}");
            Destroy(gameObject, 0.1f);
        }
    }
}
