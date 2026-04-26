using UnityEngine;
using SaintSeiya.Combat;
using SaintSeiya.Data;

namespace SaintSeiya.Characters
{
    /// <summary>
    /// 캐릭터 스탯 및 상태 관리 컴포넌트
    /// Player, Enemy, NPC 공통으로 사용
    /// </summary>
    public class CharacterStats : MonoBehaviour
    {
        [Header("Character Data")]
        public CharacterData data;

        [Header("Runtime Stats")]
        [SerializeField] private float _currentHP;
        public float CurrentHP => _currentHP;
        public float MaxHP { get; private set; }
        public float Attack { get; private set; }
        public float Defense { get; private set; }
        public float Speed { get; private set; }

        [Header("Components")]
        public CosmosSystem cosmos;

        [Header("Reward")]
        public int ExpReward = 50;

        public bool IsDead => _currentHP <= 0f;

        // 이벤트
        public event System.Action<float, float> OnHPChanged; // (current, max)
        public event System.Action OnDeath;
        public event System.Action<float> OnDamageTaken;

        void Awake()
        {
            cosmos = GetComponent<CosmosSystem>();
        }

        void Start()
        {
            if (data != null) InitFromData();
        }

        public void InitFromData()
        {
            MaxHP = data.baseHP;
            Attack = data.baseAttack;
            Defense = data.baseDefense;
            Speed = data.baseSpeed;
            _currentHP = MaxHP;
            OnHPChanged?.Invoke(_currentHP, MaxHP);
        }

        public void TakeDamage(float amount)
        {
            if (IsDead) return;
            float actual = Mathf.Min(amount, _currentHP);
            _currentHP -= actual;
            OnDamageTaken?.Invoke(actual);
            OnHPChanged?.Invoke(_currentHP, MaxHP);
            Debug.Log($"[{gameObject.name}] HP {_currentHP + actual:F0} → {_currentHP:F0} (-{actual:F0})");

            if (_currentHP <= 0f) Die();
        }

        public void Heal(float amount)
        {
            if (IsDead) return;
            _currentHP = Mathf.Min(_currentHP + amount, MaxHP);
            OnHPChanged?.Invoke(_currentHP, MaxHP);
        }

        private void Die()
        {
            Debug.Log($"[{gameObject.name}] 사망");
            OnDeath?.Invoke();
            // 애니메이션 트리거는 별도 컴포넌트에서 처리
        }

        public void Revive(float hpRatio = 0.3f)
        {
            _currentHP = MaxHP * hpRatio;
            OnHPChanged?.Invoke(_currentHP, MaxHP);
        }
    }
}
