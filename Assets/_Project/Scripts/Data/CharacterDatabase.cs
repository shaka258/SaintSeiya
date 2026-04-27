using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace SaintSeiya.Data
{
    [CreateAssetMenu(fileName = "CharacterDatabase", menuName = "SaintSeiya/CharacterDatabase")]
    public class CharacterDatabase : ScriptableObject
    {
        [SerializeField] private List<CharacterData> _characters = new();

        public CharacterData GetByName(string name) => _characters.FirstOrDefault(c => c.characterName == name);
        public CharacterData GetByConstellation(ConstellationType c) => _characters.FirstOrDefault(x => x.constellation == c);
        public List<CharacterData> GetByRank(ClothRank rank) => _characters.Where(c => c.clothRank == rank).ToList();
        public IReadOnlyList<CharacterData> GetAll() => _characters;
        public int Count => _characters.Count;

#if UNITY_EDITOR
        [ContextMenu("Refresh from Assets folder")]
        private void RefreshFromFolder()
        {
            _characters.Clear();
            var guids = UnityEditor.AssetDatabase.FindAssets("t:CharacterData", new[] { "Assets/_Project/Data/Characters" });
            foreach (var guid in guids)
            {
                var data = UnityEditor.AssetDatabase.LoadAssetAtPath<CharacterData>(UnityEditor.AssetDatabase.GUIDToAssetPath(guid));
                if (data != null) _characters.Add(data);
            }
            UnityEditor.EditorUtility.SetDirty(this);
            Debug.Log($"[CharacterDatabase] {_characters.Count}개 캐릭터 로드 완료");
        }
#endif
    }
}
