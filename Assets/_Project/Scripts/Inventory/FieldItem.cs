using UnityEngine;

namespace SaintSeiya.Inventory
{
    public class FieldItem : MonoBehaviour
    {
        [SerializeField] private ItemData _itemData;
        [SerializeField] private int _amount = 1;
        [SerializeField] private GameObject _pickupPrompt;
        [SerializeField] private float _bobHeight = 0.15f;
        [SerializeField] private float _bobSpeed  = 2f;

        private Vector3 _startPos;
        private bool _playerInRange;
        private bool _isPickedUp;

        void Start()
        {
            _startPos = transform.position;
            _pickupPrompt?.SetActive(false);
            Characters.PlayerController.OnInteractTriggered += OnPlayerInteract;
        }

        void OnDestroy() => Characters.PlayerController.OnInteractTriggered -= OnPlayerInteract;

        void Update()
        {
            if (_isPickedUp) return;
            transform.position = new Vector3(_startPos.x, _startPos.y + Mathf.Sin(Time.time * _bobSpeed) * _bobHeight, _startPos.z);
        }

        void OnTriggerEnter2D(Collider2D other) { if (other.CompareTag("Player")) { _playerInRange = true; _pickupPrompt?.SetActive(true); } }
        void OnTriggerExit2D(Collider2D other)  { if (other.CompareTag("Player")) { _playerInRange = false; _pickupPrompt?.SetActive(false); } }

        private void OnPlayerInteract(GameObject target)
        {
            if (!_playerInRange || _isPickedUp || target != gameObject) return;
            if (InventoryManager.Instance?.AddItem(_itemData, _amount) == true)
            {
                _isPickedUp = true; _pickupPrompt?.SetActive(false);
                Destroy(gameObject, 0.1f);
            }
        }
    }
}
