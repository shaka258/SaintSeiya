using UnityEngine;
<<<<<<< HEAD
using System.Collections.Generic;

namespace SaintSeiya.Characters
{
    /// <summary>
    /// 경험치 획득 및 레벨업 시스템
    /// CharacterStats와 함께 부착해서 사용
    /// </summary>
    public class LevelSystem : MonoBehaviour
    {
        [Header("Level Settings")]
=======

namespace SaintSeiya.Characters
{
    public class LevelSystem : MonoBehaviour
    {
>>>>>>> 85d6086137a2cfa6b961a0149bcb69432042ea76
        [SerializeField] private int _currentLevel = 1;
        [SerializeField] private int _maxLevel = 100;
        [SerializeField] private int _currentExp = 0;

        public int CurrentLevel => _currentLevel;
        public int CurrentExp => _currentExp;
        public int ExpToNextLevel => GetExpRequired(_currentLevel);

<<<<<<< HEAD
        // 이벤트
        public event System.Action<int> OnLevelUp;     // 새 레벨
        public event System.Action<int, int> OnExpChanged; // (현재, 다음 레벨 필요량)

        private CharacterStats _stats;

        void Awake()
        {
            _stats = GetComponent<CharacterStats>();
        }

        void Start()
        {
            Core.EventBus.Subscribe<Core.BattleEndEvent>(OnBattleEnd);
        }

        void OnDestroy()
        {
            Core.EventBus.Unsubscribe<Core.BattleEndEvent>(OnBattleEnd);
        }

        // ─── 경험치 획득 ───────────────────────────────────────
=======
        public event System.Action<int> OnLevelUp;
        public event System.Action<int, int> OnExpChanged;

        void Start() => Core.EventBus.Subscribe<Core.BattleEndEvent>(OnBattleEnd);
        void OnDestroy() => Core.EventBus.Unsubscribe<Core.BattleEndEvent>(OnBattleEnd);
>>>>>>> 85d6086137a2cfa6b961a0149bcb69432042ea76

        public void GainExp(int amount)
        {
            if (_currentLevel >= _maxLevel) return;
<<<<<<< HEAD

            _currentExp += amount;
            OnExpChanged?.Invoke(_currentExp, ExpToNextLevel);
            Debug.Log($"[LevelSystem] 경험치 +{amount} ({_currentExp}/{ExpToNextLevel})");

            // 레벨업 체크 (복수 레벨업 처리)
            while (_currentExp >= ExpToNextLevel && _currentLevel < _maxLevel)
            {
                _currentExp -= ExpToNextLevel;
                LevelUp();
            }
        }

        private void LevelUp()
        {
            _currentLevel++;
            Debug.Log($"[LevelSystem] 레벨업! → Lv.{_currentLevel}");

            // 스탯 증가
            if (_stats?.data != null)
            {
                // CharacterStats에 레벨 기반 스탯 재계산 요청
                // TODO: CharacterStats.ApplyLevelStats(_currentLevel) 연동
            }

            OnLevelUp?.Invoke(_currentLevel);
        }

        private void OnBattleEnd(Core.BattleEndEvent e)
        {
            if (e.IsVictory && e.ExpGained > 0)
                GainExp(e.ExpGained);
        }

        // ─── 경험치 테이블 ─────────────────────────────────────
        // 공식: 기본 100 + (레벨 * 50) + (레벨^2 * 10)
        // Lv.1→2: 160  Lv.10→11: 1,100  Lv.50→51: 28,100

        public int GetExpRequired(int level)
        {
            return 100 + (level * 50) + (level * level * 10);
        }

        /// <summary>특정 레벨까지의 누적 경험치</summary>
        public int GetTotalExpForLevel(int targetLevel)
        {
            int total = 0;
            for (int i = 1; i < targetLevel; i++)
                total += GetExpRequired(i);
            return total;
        }

        /// <summary>저장용 데이터 직렬화</summary>
        public (int level, int exp) Serialize() => (_currentLevel, _currentExp);

        /// <summary>불러오기 시 복원</summary>
        public void Deserialize(int level, int exp)
        {
            _currentLevel = Mathf.Clamp(level, 1, _maxLevel);
            _currentExp = exp;
            OnExpChanged?.Invoke(_currentExp, ExpToNextLevel);
        }
=======
            _currentExp += amount;
            OnExpChanged?.Invoke(_currentExp, ExpToNextLevel);
            while (_currentExp >= ExpToNextLevel && _currentLevel < _maxLevel)
            {
                _currentExp -= ExpToNextLevel;
                _currentLevel++;
                OnLevelUp?.Invoke(_currentLevel);
                Debug.Log($"[LevelSystem] 레벨업! → Lv.{_currentLevel}");
            }
        }

        private void OnBattleEnd(Core.BattleEndEvent e) { if (e.IsVictory && e.ExpGained > 0) GainExp(e.ExpGained); }

        public int GetExpRequired(int level) => 100 + (level * 50) + (level * level * 10);
        public void Deserialize(int level, int exp) { _currentLevel = Mathf.Clamp(level, 1, _maxLevel); _currentExp = exp; }
>>>>>>> 85d6086137a2cfa6b961a0149bcb69432042ea76
    }
}
