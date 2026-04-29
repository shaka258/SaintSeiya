using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SaintSeiya.WorldMap
{
    public enum RegionStatus { Locked, Available, Cleared, Current }

    [System.Serializable]
    public class RegionData
    {
        public string regionId;
        public string regionName;
        public string sceneName;
        public RegionStatus status;
        public string requiredQuestId;
        public List<string> connectedRegions;
        public Vector2 mapPosition;
        public string description;
        public int recommendedLevel;
    }

    public class WorldMapManager : MonoBehaviour
    {
        public static WorldMapManager Instance { get; private set; }
        [SerializeField] private List<RegionData> _regions = new();

        public event System.Action<RegionData> OnRegionUnlocked;
        public event System.Action<RegionData> OnRegionEntered;

        void Awake()
        {
            if (Instance != null) { Destroy(gameObject); return; }
            Instance = this;
            InitDefaultRegions();
        }

        private void InitDefaultRegions()
        {
            if (_regions.Count > 0) return;
            _regions = new List<RegionData>
            {
                new RegionData { regionId="region_sanctuary_entrance", regionName="성역 입구", sceneName="Field_Sanctuary", status=RegionStatus.Current, connectedRegions=new List<string>{"region_bronze_arena","region_silver_gate"}, mapPosition=new Vector2(0,0), description="성역으로 향하는 입구. 청동 성투사들의 훈련장이 있다.", recommendedLevel=1 },
                new RegionData { regionId="region_bronze_arena", regionName="청동 성투사 수련장", sceneName="Field_BronzeArena", status=RegionStatus.Available, connectedRegions=new List<string>{"region_sanctuary_entrance","region_silver_gate"}, mapPosition=new Vector2(200,-100), description="청동 성의를 가진 성투사들이 수련하는 곳.", recommendedLevel=3 },
                new RegionData { regionId="region_silver_gate", regionName="은색 성역의 문", sceneName="Field_SilverGate", status=RegionStatus.Locked, requiredQuestId="quest_main_001", connectedRegions=new List<string>{"region_sanctuary_entrance","region_twelve_houses"}, mapPosition=new Vector2(0,200), description="황금 성투사들의 영역으로 향하는 문.", recommendedLevel=10 },
                new RegionData { regionId="region_twelve_houses", regionName="12궁", sceneName="Field_TwelveHouses", status=RegionStatus.Locked, requiredQuestId="quest_main_005", connectedRegions=new List<string>{"region_silver_gate"}, mapPosition=new Vector2(0,400), description="황금 성투사 12명이 지키는 성역의 심장부.", recommendedLevel=30 }
            };
        }

        public RegionData GetRegion(string id) => _regions.Find(r => r.regionId == id);
        public List<RegionData> GetAllRegions() => _regions;
        public List<RegionData> GetAvailableRegions() => _regions.FindAll(r => r.status != RegionStatus.Locked);

        public void UnlockRegion(string regionId)
        {
            var r = GetRegion(regionId);
            if (r == null || r.status != RegionStatus.Locked) return;
            r.status = RegionStatus.Available;
            OnRegionUnlocked?.Invoke(r);
            Debug.Log($"[WorldMap] 지역 해금: {r.regionName}");
        }

        public void SetRegionCleared(string regionId)
        {
            var r = GetRegion(regionId);
            if (r == null) return;
            r.status = RegionStatus.Cleared;
            foreach (var connId in r.connectedRegions)
            {
                var conn = GetRegion(connId);
                if (conn != null && conn.status == RegionStatus.Locked && string.IsNullOrEmpty(conn.requiredQuestId))
                    UnlockRegion(connId);
            }
        }

        public void EnterRegion(string regionId)
        {
            var r = GetRegion(regionId);
            if (r == null || r.status == RegionStatus.Locked) return;
            foreach (var region in _regions) if (region.status == RegionStatus.Current) region.status = RegionStatus.Available;
            r.status = RegionStatus.Current;
            OnRegionEntered?.Invoke(r);
            Core.GameManager.Instance?.LoadScene(r.sceneName);
        }

        public string Serialize() => JsonConvert.SerializeObject(_regions.ConvertAll(r => new[]{r.regionId, ((int)r.status).ToString()}));
        public void Deserialize(string json)
        {
            if (string.IsNullOrEmpty(json)) return;
            var list = JsonConvert.DeserializeObject<List<string[]>>(json);
            foreach (var item in list) { var r = GetRegion(item[0]); if (r != null && int.TryParse(item[1], out int s)) r.status = (RegionStatus)s; }
        }
    }
}
