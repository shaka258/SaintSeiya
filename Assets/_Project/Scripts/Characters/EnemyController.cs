using UnityEngine;

namespace SaintSeiya.Characters
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyController : MonoBehaviour
    {
        public enum EnemyState { Patrol, Chase, Battle, Dead }

        [SerializeField] private float _detectRange = 4f;
        [SerializeField] private float _battleRange = 1.2f;
        [SerializeField] private float _loseRange   = 8f;
        [SerializeField] private float _patrolSpeed = 1.5f;
        [SerializeField] private float _chaseSpeed  = 3.5f;
        [SerializeField] private float _patrolRadius = 3f;
        [SerializeField] private string _enemyId;
        [SerializeField] private float _battleCooldown = 2f;

        private Rigidbody2D _rb;
        private Transform _player;
        private EnemyState _state = EnemyState.Patrol;
        private Vector2 _patrolOrigin;
        private Vector2 _patrolTarget;
        private float _patrolWaitTimer;
        private float _battleCooldownTimer;

        void Awake() { _rb = GetComponent<Rigidbody2D>(); _patrolOrigin = transform.position; SetNewPatrolTarget(); }

        void Start()
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p) _player = p.transform;
            Core.EventBus.Subscribe<Core.BattleEndEvent>(OnBattleEnd);
        }

        void OnDestroy() => Core.EventBus.Unsubscribe<Core.BattleEndEvent>(OnBattleEnd);

        void Update()
        {
            if (_state == EnemyState.Dead) return;
            if (_battleCooldownTimer > 0f) _battleCooldownTimer -= Time.deltaTime;
            if (_player == null) return;
            float dist = Vector2.Distance(transform.position, _player.position);
            if (_state == EnemyState.Patrol && dist <= _detectRange) _state = EnemyState.Chase;
            else if (_state == EnemyState.Chase)
            {
                if (dist > _loseRange) { _state = EnemyState.Patrol; SetNewPatrolTarget(); }
                else if (dist <= _battleRange && _battleCooldownTimer <= 0f) TriggerBattle();
            }
        }

        void FixedUpdate()
        {
            if (_state == EnemyState.Dead || _state == EnemyState.Battle) { _rb.linearVelocity = Vector2.zero; return; }
            Vector2 dir = _state == EnemyState.Patrol
                ? (_patrolTarget - (Vector2)transform.position).magnitude < 0.2f ? Vector2.zero : (_patrolTarget - (Vector2)transform.position).normalized
                : _player != null ? ((Vector2)_player.position - (Vector2)transform.position).normalized : Vector2.zero;
            _rb.linearVelocity = dir * (_state == EnemyState.Chase ? _chaseSpeed : _patrolSpeed);
        }

        private void SetNewPatrolTarget() { _patrolTarget = _patrolOrigin + Random.insideUnitCircle * _patrolRadius; _patrolWaitTimer = Random.Range(1f, 3f); }

        private void TriggerBattle()
        {
            _state = EnemyState.Battle;
            Core.EventBus.Publish(new Core.BattleStartEvent { EnemyId = _enemyId, SceneContext = gameObject.scene.name });
            Core.GameManager.Instance?.ChangeState(Core.GameManager.GameState.Battle);
        }

        private void OnBattleEnd(Core.BattleEndEvent e)
        {
            if (_state != EnemyState.Battle) return;
            if (e.IsVictory) { _state = EnemyState.Dead; gameObject.SetActive(false); }
            else { _battleCooldownTimer = _battleCooldown; _state = EnemyState.Patrol; SetNewPatrolTarget(); }
        }
    }
}
