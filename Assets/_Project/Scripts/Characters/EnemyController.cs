using UnityEngine;
<<<<<<< HEAD
using UnityEngine.AI;

namespace SaintSeiya.Characters
{
    /// <summary>
    /// 적 AI 컨트롤러 — 필드 패트롤 + 플레이어 감지 + 전투 돌입
    /// NavMesh2D 없이 직접 이동 구현 (2D 프로젝트 대응)
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
=======

namespace SaintSeiya.Characters
{
    [RequireComponent(typeof(Rigidbody2D))]
>>>>>>> 85d6086137a2cfa6b961a0149bcb69432042ea76
    public class EnemyController : MonoBehaviour
    {
        public enum EnemyState { Patrol, Chase, Battle, Dead }

<<<<<<< HEAD
        [Header("Detection")]
        [SerializeField] private float _detectRange  = 4f;   // 플레이어 감지 거리
        [SerializeField] private float _battleRange  = 1.2f; // 전투 돌입 거리
        [SerializeField] private float _loseRange    = 8f;   // 추적 포기 거리

        [Header("Movement")]
        [SerializeField] private float _patrolSpeed  = 1.5f;
        [SerializeField] private float _chaseSpeed   = 3.5f;
        [SerializeField] private float _patrolRadius = 3f;   // 원점 기준 패트롤 범위

        [Header("Battle")]
        [SerializeField] private string _enemyId;            // 전투씬에서 로드할 적 ID
        [SerializeField] private float _battleCooldown = 2f; // 전투 종료 후 재돌입 방지

        private Rigidbody2D _rb;
        private Animator _anim;
        private Transform _player;

=======
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
>>>>>>> 85d6086137a2cfa6b961a0149bcb69432042ea76
        private EnemyState _state = EnemyState.Patrol;
        private Vector2 _patrolOrigin;
        private Vector2 _patrolTarget;
        private float _patrolWaitTimer;
        private float _battleCooldownTimer;

<<<<<<< HEAD
        private static readonly int AnimSpeed  = Animator.StringToHash("Speed");
        private static readonly int AnimMoveX  = Animator.StringToHash("MoveX");
        private static readonly int AnimMoveY  = Animator.StringToHash("MoveY");

        void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _anim = GetComponent<Animator>();
            _patrolOrigin = transform.position;
            SetNewPatrolTarget();
        }

        void Start()
        {
            // 플레이어 참조
            var playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) _player = playerObj.transform;

            // 전투 종료 이벤트 구독
            Core.EventBus.Subscribe<Core.BattleEndEvent>(OnBattleEnd);
        }

        void OnDestroy()
        {
            Core.EventBus.Unsubscribe<Core.BattleEndEvent>(OnBattleEnd);
        }
=======
        void Awake() { _rb = GetComponent<Rigidbody2D>(); _patrolOrigin = transform.position; SetNewPatrolTarget(); }

        void Start()
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p) _player = p.transform;
            Core.EventBus.Subscribe<Core.BattleEndEvent>(OnBattleEnd);
        }

        void OnDestroy() => Core.EventBus.Unsubscribe<Core.BattleEndEvent>(OnBattleEnd);
>>>>>>> 85d6086137a2cfa6b961a0149bcb69432042ea76

        void Update()
        {
            if (_state == EnemyState.Dead) return;
            if (_battleCooldownTimer > 0f) _battleCooldownTimer -= Time.deltaTime;
<<<<<<< HEAD

            UpdateState();
=======
            if (_player == null) return;
            float dist = Vector2.Distance(transform.position, _player.position);
            if (_state == EnemyState.Patrol && dist <= _detectRange) _state = EnemyState.Chase;
            else if (_state == EnemyState.Chase)
            {
                if (dist > _loseRange) { _state = EnemyState.Patrol; SetNewPatrolTarget(); }
                else if (dist <= _battleRange && _battleCooldownTimer <= 0f) TriggerBattle();
            }
>>>>>>> 85d6086137a2cfa6b961a0149bcb69432042ea76
        }

        void FixedUpdate()
        {
<<<<<<< HEAD
            if (_state == EnemyState.Dead) return;
            MoveByState();
        }

        // ─── 상태 관리 ─────────────────────────────────────────

        private void UpdateState()
        {
            if (_player == null) return;
            float dist = Vector2.Distance(transform.position, _player.position);

            switch (_state)
            {
                case EnemyState.Patrol:
                    if (dist <= _detectRange)
                        ChangeState(EnemyState.Chase);
                    break;

                case EnemyState.Chase:
                    if (dist > _loseRange)
                        ChangeState(EnemyState.Patrol);
                    else if (dist <= _battleRange && _battleCooldownTimer <= 0f)
                        TriggerBattle();
                    break;
            }
        }

        private void ChangeState(EnemyState newState)
        {
            _state = newState;
            if (newState == EnemyState.Patrol) SetNewPatrolTarget();
        }

        // ─── 이동 ──────────────────────────────────────────────

        private void MoveByState()
        {
            Vector2 dir = Vector2.zero;

            if (_state == EnemyState.Patrol)
            {
                dir = (_patrolTarget - (Vector2)transform.position);
                if (dir.magnitude < 0.2f)
                {
                    _patrolWaitTimer -= Time.fixedDeltaTime;
                    if (_patrolWaitTimer <= 0f) SetNewPatrolTarget();
                    _rb.linearVelocity = Vector2.zero;
                    UpdateAnimator(Vector2.zero);
                    return;
                }
                dir = dir.normalized;
                _rb.linearVelocity = dir * _patrolSpeed;
            }
            else if (_state == EnemyState.Chase && _player != null)
            {
                dir = ((Vector2)_player.position - (Vector2)transform.position).normalized;
                _rb.linearVelocity = dir * _chaseSpeed;
            }

            UpdateAnimator(dir);
        }

        private void SetNewPatrolTarget()
        {
            Vector2 offset = Random.insideUnitCircle * _patrolRadius;
            _patrolTarget = _patrolOrigin + offset;
            _patrolWaitTimer = Random.Range(1f, 3f);
        }

        private void UpdateAnimator(Vector2 dir)
        {
            _anim.SetFloat(AnimSpeed, dir.sqrMagnitude);
            if (dir.sqrMagnitude > 0.01f)
            {
                _anim.SetFloat(AnimMoveX, dir.x);
                _anim.SetFloat(AnimMoveY, dir.y);
            }
        }

        // ─── 전투 ──────────────────────────────────────────────

        private void TriggerBattle()
        {
            ChangeState(EnemyState.Battle);
            _rb.linearVelocity = Vector2.zero;

            Core.EventBus.Publish(new Core.BattleStartEvent
            {
                EnemyId = _enemyId,
                SceneContext = gameObject.scene.name
            });

=======
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
>>>>>>> 85d6086137a2cfa6b961a0149bcb69432042ea76
            Core.GameManager.Instance?.ChangeState(Core.GameManager.GameState.Battle);
        }

        private void OnBattleEnd(Core.BattleEndEvent e)
        {
            if (_state != EnemyState.Battle) return;
<<<<<<< HEAD

            if (e.IsVictory)
            {
                // 플레이어 승리 → 적 사망 처리
                ChangeState(EnemyState.Dead);
                _rb.linearVelocity = Vector2.zero;
                gameObject.SetActive(false); // 또는 사망 애니메이션 후 비활성화
            }
            else
            {
                // 플레이어 패배 → 패트롤 복귀
                _battleCooldownTimer = _battleCooldown;
                ChangeState(EnemyState.Patrol);
            }
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _detectRange);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _battleRange);
            Gizmos.color = Color.gray;
            Gizmos.DrawWireSphere(Application.isPlaying ? (Vector3)_patrolOrigin : transform.position, _patrolRadius);
=======
            if (e.IsVictory) { _state = EnemyState.Dead; gameObject.SetActive(false); }
            else { _battleCooldownTimer = _battleCooldown; _state = EnemyState.Patrol; SetNewPatrolTarget(); }
>>>>>>> 85d6086137a2cfa6b961a0149bcb69432042ea76
        }
    }
}
