using UnityEngine;

namespace SaintSeiya.Characters
{
    public class LevelSystem : MonoBehaviour
    {
        [SerializeField] private int _currentLevel = 1;
        [SerializeField] private int _maxLevel = 100;
        [SerializeField] private int _currentExp = 0;

        public int CurrentLevel => _currentLevel;
        public int CurrentExp => _currentExp;
        public int ExpToNextLevel => GetExpRequired(_currentLevel);

        public event System.Action<int> OnLevelUp;
        public event System.Action<int, int> OnExpChanged;

        void Start() => Core.EventBus.Subscribe<Core.BattleEndEvent>(OnBattleEnd);
        void OnDestroy() => Core.EventBus.Unsubscribe<Core.BattleEndEvent>(OnBattleEnd);

        public void GainExp(int amount)
        {
            if (_currentLevel >= _maxLevel) return;
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
    }
}
