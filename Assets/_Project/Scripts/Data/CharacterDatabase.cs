using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace SaintSeiya.Data
{
    /// <summary>
    /// 모든 캐릭터 데이터를 보관하는 ScriptableObject 데이터베이스
    /// Resources 폴더에 배치 후 Resources.Load 또는
    /// Inspector에서 GameManager에 직접 연결해서 사용
    /// </summary>
    [CreateAssetMenu(fileName = "CharacterDatabase", menuName = "SaintSeiya/CharacterDatabase")]
    public class CharacterDatabase : ScriptableObject
    {
        [SerializeField] private List<CharacterData> _characters = new();

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
        [ContextMenu("Refresh from Assets folder")]
        private void RefreshFromFolder()
        {
            _characters.Clear();
            var guids = UnityEditor.AssetDatabase.FindAssets(
                "t:CharacterData", new[] { "Assets/_Project/Data/Characters" });
            foreach (var guid in guids)
            {
                var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                var data = UnityEditor.AssetDatabase.LoadAssetAtPath<CharacterData>(path);
                if (data != null) _characters.Add(data);
            }
            UnityEditor.EditorUtility.SetDirty(this);
            Debug.Log($"[CharacterDatabase] {_characters.Count}개 캐릭터 로드 완료");
        }
#endif
    }
}
