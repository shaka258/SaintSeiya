using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace SaintSeiya.Data
{
<<<<<<< HEAD
    /// <summary>
    /// 모든 캐릭터 데이터를 보관하는 ScriptableObject 데이터베이스
    /// Resources 폴더에 배치 후 Resources.Load 또는
    /// Inspector에서 GameManager에 직접 연결해서 사용
    /// </summary>
=======
>>>>>>> 85d6086137a2cfa6b961a0149bcb69432042ea76
    [CreateAssetMenu(fileName = "CharacterDatabase", menuName = "SaintSeiya/CharacterDatabase")]
    public class CharacterDatabase : ScriptableObject
    {
        [SerializeField] private List<CharacterData> _characters = new();

<<<<<<< HEAD
        // ─── 조회 ───────────────────────────────────────────────

        /// <summary>이름으로 캐릭터 데이터 검색</summary>
        public CharacterData GetByName(string characterName)
        {
            return _characters.FirstOrDefault(c =>
                c.characterName == characterName);
        }

        /// <summary>별자리로 캐릭터 데이터 검색</summary>
        public CharacterData GetByConstellation(ConstellationType constellation)
        {
            return _characters.FirstOrDefault(c =>
                c.constellation == constellation);
        }

        /// <summary>등급별 캐릭터 목록</summary>
        public List<CharacterData> GetByRank(ClothRank rank)
        {
            return _characters.Where(c => c.clothRank == rank).ToList();
        }

        /// <summary>전체 캐릭터 목록</summary>
        public IReadOnlyList<CharacterData> GetAll() => _characters;

        /// <summary>캐릭터 수</summary>
        public int Count => _characters.Count;

#if UNITY_EDITOR
        // 에디터에서 자동으로 Assets/_Project/Data/Characters 폴더 스캔
=======
        public CharacterData GetByName(string name) => _characters.FirstOrDefault(c => c.characterName == name);
        public CharacterData GetByConstellation(ConstellationType c) => _characters.FirstOrDefault(x => x.constellation == c);
        public List<CharacterData> GetByRank(ClothRank rank) => _characters.Where(c => c.clothRank == rank).ToList();
        public IReadOnlyList<CharacterData> GetAll() => _characters;
        public int Count => _characters.Count;

#if UNITY_EDITOR
>>>>>>> 85d6086137a2cfa6b961a0149bcb69432042ea76
        [ContextMenu("Refresh from Assets folder")]
        private void RefreshFromFolder()
        {
            _characters.Clear();
<<<<<<< HEAD
            var guids = UnityEditor.AssetDatabase.FindAssets(
                "t:CharacterData", new[] { "Assets/_Project/Data/Characters" });
            foreach (var guid in guids)
            {
                var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                var data = UnityEditor.AssetDatabase.LoadAssetAtPath<CharacterData>(path);
=======
            var guids = UnityEditor.AssetDatabase.FindAssets("t:CharacterData", new[] { "Assets/_Project/Data/Characters" });
            foreach (var guid in guids)
            {
                var data = UnityEditor.AssetDatabase.LoadAssetAtPath<CharacterData>(UnityEditor.AssetDatabase.GUIDToAssetPath(guid));
>>>>>>> 85d6086137a2cfa6b961a0149bcb69432042ea76
                if (data != null) _characters.Add(data);
            }
            UnityEditor.EditorUtility.SetDirty(this);
            Debug.Log($"[CharacterDatabase] {_characters.Count}개 캐릭터 로드 완료");
        }
#endif
    }
}
