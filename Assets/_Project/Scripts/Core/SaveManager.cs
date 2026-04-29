using UnityEngine;
using System.IO;
using Newtonsoft.Json;

namespace SaintSeiya.Core
{
    /// <summary>
    /// JSON 기반 저장/불러오기 시스템
    /// </summary>
    public class SaveManager : MonoBehaviour
    {
        private const string SAVE_FILE = "save_data.json";
        private string SavePath => Path.Combine(Application.persistentDataPath, SAVE_FILE);

        [System.Serializable]
        public class SaveData
        {
            public string playerId;
            public int currentChapter;
            public float playTime;
            public PlayerProgressData playerProgress;
            public string inventoryJson;   // 인벤토리 직렬화
            public string lastSaveTime;
        }

        [System.Serializable]
        public class PlayerProgressData
        {
            public int level;
            public int experience;
            public int cosmos;
            public string currentScene;
            public float posX, posY;
        }

        private SaveData _currentSave;
        public SaveData CurrentSave => _currentSave;

        public bool HasSaveData => File.Exists(SavePath);

        public void Save(SaveData data)
        {
            // 인벤토리 자동 직렬화
            data.inventoryJson = Inventory.InventoryManager.Instance?.Serialize();
            data.lastSaveTime  = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(SavePath, json);
            _currentSave = data;
            Debug.Log($"[SaveManager] 저장 완료: {SavePath}");
        }

        public SaveData Load()
        {
            if (!HasSaveData)
            {
                Debug.LogWarning("[SaveManager] 저장 파일 없음. 새 게임 데이터 반환.");
                return NewGame();
            }

            string json = File.ReadAllText(SavePath);
            _currentSave = JsonConvert.DeserializeObject<SaveData>(json);
            // 인벤토리 복원
            if (!string.IsNullOrEmpty(_currentSave.inventoryJson))
                Inventory.InventoryManager.Instance?.Deserialize(_currentSave.inventoryJson);
            Debug.Log($"[SaveManager] 불러오기 완료: {_currentSave.lastSaveTime}");
            return _currentSave;
        }

        public void DeleteSave()
        {
            if (File.Exists(SavePath))
            {
                File.Delete(SavePath);
                _currentSave = null;
                Debug.Log("[SaveManager] 저장 데이터 삭제");
            }
        }

        private SaveData NewGame()
        {
            return new SaveData
            {
                playerId = System.Guid.NewGuid().ToString(),
                currentChapter = 1,
                playTime = 0f,
                playerProgress = new PlayerProgressData
                {
                    level = 1,
                    experience = 0,
                    cosmos = 0,
                    currentScene = "Field_SagittariusArena",
                    posX = 0f,
                    posY = 0f
                }
            };
        }
    }
}
