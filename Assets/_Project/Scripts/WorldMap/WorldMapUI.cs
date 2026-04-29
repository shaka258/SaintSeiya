using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

namespace SaintSeiya.WorldMap
{
    public class WorldMapUI : MonoBehaviour
    {
        [SerializeField] private Transform _nodeContainer;
        [SerializeField] private GameObject _regionNodePrefab;
        [SerializeField] private GameObject _infoPanel;
        [SerializeField] private TextMeshProUGUI _regionNameText;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private TextMeshProUGUI _recommendLevelText;
        [SerializeField] private TextMeshProUGUI _statusText;
        [SerializeField] private Button _enterButton;

        private RegionData _selectedRegion;
        private List<RegionNodeUI> _nodes = new();

        void Start()
        {
            _infoPanel?.SetActive(false);
            _enterButton?.onClick.AddListener(() => WorldMapManager.Instance?.EnterRegion(_selectedRegion?.regionId));
            if (WorldMapManager.Instance != null) WorldMapManager.Instance.OnRegionUnlocked += _ => BuildMap();
            BuildMap();
        }

        private void BuildMap()
        {
            if (WorldMapManager.Instance == null || _regionNodePrefab == null || _nodeContainer == null) return;
            foreach (Transform child in _nodeContainer) Destroy(child.gameObject);
            _nodes.Clear();
            foreach (var region in WorldMapManager.Instance.GetAllRegions())
            {
                var nodeGO = Instantiate(_regionNodePrefab, _nodeContainer);
                var nodeUI = nodeGO.GetComponent<RegionNodeUI>();
                if (nodeUI == null) continue;
                nodeUI.Setup(region);
                nodeUI.OnNodeClicked += OnNodeClicked;
                _nodes.Add(nodeUI);
            }
        }

        private void OnNodeClicked(RegionData region)
        {
            _selectedRegion = region;
            _infoPanel?.SetActive(true);
            _regionNameText?.SetText(region.regionName);
            _descriptionText?.SetText(region.description);
            _recommendLevelText?.SetText($"권장 레벨: {region.recommendedLevel}");
            _statusText?.SetText(region.status switch { RegionStatus.Locked => "🔒 잠김", RegionStatus.Available => "✅ 진입 가능", RegionStatus.Cleared => "⭐ 클리어", RegionStatus.Current => "📍 현재 위치", _ => "" });
            if (_enterButton != null) _enterButton.interactable = region.status != RegionStatus.Locked;
        }
    }
}
