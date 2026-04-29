using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SaintSeiya.Inventory
{
<<<<<<< HEAD
    /// <summary>
    /// 인벤토리 그리드의 개별 슬롯 UI
    /// </summary>
=======
>>>>>>> 85d6086137a2cfa6b961a0149bcb69432042ea76
    public class ItemSlotUI : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _countText;
<<<<<<< HEAD
        [SerializeField] private Image _rarityBorder;   // 등급별 테두리 색
        [SerializeField] private Button _button;

        [Header("Rarity Colors")]
        [SerializeField] private Color _commonColor    = Color.gray;
        [SerializeField] private Color _uncommonColor  = Color.green;
        [SerializeField] private Color _rareColor      = new Color(0.3f, 0.5f, 1f);
        [SerializeField] private Color _legendaryColor = new Color(1f, 0.6f, 0f);

        private System.Action<ItemData> _onClicked;

        public void Setup(ItemData data, int count, System.Action<ItemData> onClicked)
        {
            _onClicked = onClicked;

            // 아이콘
            if (_icon != null)
            {
                _icon.sprite = data.icon;
                _icon.enabled = data.icon != null;
            }

            // 수량 (1개면 숨기기)
            if (_countText != null)
            {
                _countText.gameObject.SetActive(count > 1);
                _countText.SetText(count.ToString());
            }

            // 등급 테두리 색상
            if (_rarityBorder != null)
            {
                _rarityBorder.color = data.rarity switch
                {
                    ItemRarity.Common    => _commonColor,
                    ItemRarity.Uncommon  => _uncommonColor,
                    ItemRarity.Rare      => _rareColor,
                    ItemRarity.Legendary => _legendaryColor,
                    _                    => _commonColor
                };
            }

            _button?.onClick.RemoveAllListeners();
            _button?.onClick.AddListener(() => _onClicked?.Invoke(data));
=======
        [SerializeField] private Image _rarityBorder;
        [SerializeField] private Button _button;

        public void Setup(ItemData data, int count, System.Action<ItemData> onClicked)
        {
            if (_icon != null) { _icon.sprite = data.icon; _icon.enabled = data.icon != null; }
            if (_countText != null) { _countText.gameObject.SetActive(count > 1); _countText.SetText(count.ToString()); }
            if (_rarityBorder != null) _rarityBorder.color = data.rarity switch
            {
                ItemRarity.Uncommon  => Color.green,
                ItemRarity.Rare      => new Color(0.3f, 0.5f, 1f),
                ItemRarity.Legendary => new Color(1f, 0.6f, 0f),
                _                    => Color.gray
            };
            _button?.onClick.RemoveAllListeners();
            _button?.onClick.AddListener(() => onClicked?.Invoke(data));
>>>>>>> 85d6086137a2cfa6b961a0149bcb69432042ea76
        }
    }
}
