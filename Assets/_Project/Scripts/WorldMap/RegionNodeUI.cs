using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SaintSeiya.WorldMap
{
    public class RegionNodeUI : MonoBehaviour
    {
        [SerializeField] private Image _nodeBg;
        [SerializeField] private TextMeshProUGUI _regionNameText;
        [SerializeField] private GameObject _lockIcon;
        [SerializeField] private GameObject _clearedBadge;
        [SerializeField] private GameObject _currentIndicator;

        private RegionData _data;
        private Button _button;
        public event System.Action<RegionData> OnNodeClicked;

        void Awake() { _button = GetComponent<Button>(); _button?.onClick.AddListener(() => OnNodeClicked?.Invoke(_data)); }

        public void Setup(RegionData data)
        {
            _data = data;
            _regionNameText?.SetText(data.regionName);
            _lockIcon?.SetActive(data.status == RegionStatus.Locked);
            _clearedBadge?.SetActive(data.status == RegionStatus.Cleared);
            _currentIndicator?.SetActive(data.status == RegionStatus.Current);
            if (_button != null) _button.interactable = data.status != RegionStatus.Locked;
            if (_nodeBg != null) _nodeBg.color = data.status switch
            {
                RegionStatus.Locked    => new Color(0.3f,0.3f,0.3f),
                RegionStatus.Available => new Color(0.8f,0.7f,0.2f),
                RegionStatus.Cleared   => new Color(0.2f,0.7f,0.3f),
                RegionStatus.Current   => new Color(0.2f,0.5f,1f),
                _ => Color.gray
            };
            var rt = GetComponent<RectTransform>();
            if (rt != null) rt.anchoredPosition = data.mapPosition;
        }
    }
}
