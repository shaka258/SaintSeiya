using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SaintSeiya.Inventory
{
    public class ItemSlotUI : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _countText;
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
        }
    }
}
