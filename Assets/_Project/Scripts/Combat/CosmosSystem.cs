using UnityEngine;

namespace SaintSeiya.Combat
{
    /// <summary>
    /// 코스모(Cosmos) 시스템 — 세인트 세이야의 핵심 전투 메카닉
    /// 코스모를 축적해 스킬 사용, 각성 상태(Burning) 진입 가능
    /// </summary>
    public class CosmosSystem : MonoBehaviour
    {
        [Header("Cosmos Settings")]
        [SerializeField] private float _maxCosmos = 100f;
        [SerializeField] private float _currentCosmos = 0f;
        [SerializeField] private float _regenRate = 5f;   // 초당 자연 회복
        [SerializeField] private float _burnDuration = 8f; // 연소 지속 시간 (초)

        public float MaxCosmos => _maxCosmos;
        public float CurrentCosmos => _currentCosmos;
        public float CosmosRatio => _currentCosmos / _maxCosmos;

        public bool IsBurning { get; private set; } = false;
        public bool IsMaxed => _currentCosmos >= _maxCosmos;

        private float _burnTimer = 0f;

        // ─── 이벤트 ───────────────────────────────────────────
        public event System.Action<float, float> OnCosmosChanged; // (current, max)
        public event System.Action OnCosmosMax;
        public event System.Action OnBurnStart;
        public event System.Action OnBurnEnd;

        void Update()
        {
            if (IsBurning)
            {
                HandleBurnTimer();
            }
            else
            {
                RegenerateCosmos();
            }
        }

        private void RegenerateCosmos()
        {
            if (_currentCosmos >= _maxCosmos) return;

            _currentCosmos = Mathf.Min(_currentCosmos + _regenRate * Time.deltaTime, _maxCosmos);
            OnCosmosChanged?.Invoke(_currentCosmos, _maxCosmos);
            Core.EventBus.Publish(new Core.CosmosChangedEvent { Current = _currentCosmos, Max = _maxCosmos });

            if (_currentCosmos >= _maxCosmos)
            {
                OnCosmosMax?.Invoke();
                Core.EventBus.Publish(new Core.CosmosMaxEvent { CharacterId = gameObject.name });
            }
        }

        private void HandleBurnTimer()
        {
            _burnTimer -= Time.deltaTime;
            if (_burnTimer <= 0f) EndBurn();
        }

        /// <summary>코스모 소비. 부족하면 false 반환</summary>
        public bool ConsumeCosmos(float amount)
        {
            if (_currentCosmos < amount)
            {
                Debug.Log($"[CosmosSystem] 코스모 부족: {_currentCosmos:F1} / 필요: {amount}");
                return false;
            }
            _currentCosmos -= amount;
            OnCosmosChanged?.Invoke(_currentCosmos, _maxCosmos);
            Core.EventBus.Publish(new Core.CosmosChangedEvent { Current = _currentCosmos, Max = _maxCosmos });
            return true;
        }

        /// <summary>코스모 획득 (전투 중 공격받거나 특정 스킬 사용 시)</summary>
        public void GainCosmos(float amount)
        {
            _currentCosmos = Mathf.Min(_currentCosmos + amount, _maxCosmos);
            OnCosmosChanged?.Invoke(_currentCosmos, _maxCosmos);
        }

        /// <summary>코스모 완전 연소 — 일시적 초강화 상태</summary>
        public void BurnCosmos()
        {
            if (IsBurning || _currentCosmos < _maxCosmos * 0.5f) return;

            IsBurning = true;
            _burnTimer = _burnDuration;
            _currentCosmos = _maxCosmos; // 연소 중 유지
            OnBurnStart?.Invoke();
            Debug.Log("[CosmosSystem] 코스모 연소 시작!");
        }

        private void EndBurn()
        {
            IsBurning = false;
            _burnTimer = 0f;
            _currentCosmos = _maxCosmos * 0.1f; // 연소 후 잔량 10%
            OnBurnEnd?.Invoke();
            OnCosmosChanged?.Invoke(_currentCosmos, _maxCosmos);
            Debug.Log("[CosmosSystem] 코스모 연소 종료");
        }

        public void Reset()
        {
            IsBurning = false;
            _burnTimer = 0f;
            _currentCosmos = 0f;
            OnCosmosChanged?.Invoke(_currentCosmos, _maxCosmos);
        }
    }
}
