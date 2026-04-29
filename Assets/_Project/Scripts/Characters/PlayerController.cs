using UnityEngine;
using UnityEngine.InputSystem;

namespace SaintSeiya.Characters
{
    /// <summary>
    /// 플레이어 필드 이동 컨트롤러
    /// New Input System 기반 8방향 이동 + 대시 + 상호작용
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float _moveSpeed    = 5f;
        [SerializeField] private float _dashSpeed    = 12f;
        [SerializeField] private float _dashDuration = 0.2f;
        [SerializeField] private float _dashCooldown = 1f;

        [Header("Interaction")]
        [SerializeField] private float _interactRadius = 1.2f;
        [SerializeField] private LayerMask _interactLayer;

        [Header("References")]
        [SerializeField] private SpriteRenderer _spriteRenderer;

        // Components
        private Rigidbody2D _rb;
        private Animator _anim;
        private PlayerInput _input;

        // State
        private Vector2 _moveInput;
        private Vector2 _lastMoveDir = Vector2.down;
        private bool _isDashing;
        private float _dashTimer;
        private float _dashCooldownTimer;
        private bool _canMove = true;

        // Animator Parameter IDs (string → int 캐싱으로 성능 최적화)
        private static readonly int AnimMoveX    = Animator.StringToHash("MoveX");
        private static readonly int AnimMoveY    = Animator.StringToHash("MoveY");
        private static readonly int AnimSpeed    = Animator.StringToHash("Speed");
        private static readonly int AnimIsDash   = Animator.StringToHash("IsDash");

        // 이벤트
        public static event System.Action<GameObject> OnInteractTriggered;

        void Awake()
        {
            _rb    = GetComponent<Rigidbody2D>();
            _anim  = GetComponent<Animator>();
            _input = GetComponent<PlayerInput>();
        }

        void OnEnable()
        {
            Core.GameManager.OnGameStateChanged += OnGameStateChanged;
        }

        void OnDisable()
        {
            Core.GameManager.OnGameStateChanged -= OnGameStateChanged;
        }

        // ─── Input System 콜백 ─────────────────────────────────

        public void OnMove(InputValue value)
        {
            _moveInput = value.Get<Vector2>();
        }

        public void OnDash(InputValue value)
        {
            if (value.isPressed) TryDash();
        }

        public void OnInteract(InputValue value)
        {
            if (value.isPressed) TryInteract();
        }

        // ─── Update / FixedUpdate ──────────────────────────────

        void Update()
        {
            if (_isDashing)
            {
                _dashTimer -= Time.deltaTime;
                if (_dashTimer <= 0f) EndDash();
            }

            if (_dashCooldownTimer > 0f)
                _dashCooldownTimer -= Time.deltaTime;

            UpdateAnimation();
        }

        void FixedUpdate()
        {
            if (!_canMove) return;

            if (_isDashing)
            {
                _rb.linearVelocity = _lastMoveDir * _dashSpeed;
            }
            else
            {
                _rb.linearVelocity = _moveInput * _moveSpeed;
                if (_moveInput.sqrMagnitude > 0.01f)
                    _lastMoveDir = _moveInput.normalized;
            }
        }

        // ─── 대시 ──────────────────────────────────────────────

        private void TryDash()
        {
            if (_isDashing || _dashCooldownTimer > 0f || !_canMove) return;

            _isDashing = true;
            _dashTimer = _dashDuration;
            _dashCooldownTimer = _dashCooldown;
            _anim.SetBool(AnimIsDash, true);

            // 대시 방향 없으면 바라보는 방향으로
            if (_moveInput.sqrMagnitude < 0.01f)
                _lastMoveDir = Vector2.down;
        }

        private void EndDash()
        {
            _isDashing = false;
            _anim.SetBool(AnimIsDash, false);
        }

        // ─── 상호작용 ──────────────────────────────────────────

        private void TryInteract()
        {
            Collider2D hit = Physics2D.OverlapCircle(
                transform.position + (Vector3)_lastMoveDir * 0.5f,
                _interactRadius,
                _interactLayer
            );

            if (hit != null)
            {
                OnInteractTriggered?.Invoke(hit.gameObject);
                Core.EventBus.Publish(new PlayerInteractEvent
                {
                    Target = hit.gameObject,
                    PlayerPosition = transform.position
                });
            }
        }

        // ─── 애니메이션 ────────────────────────────────────────

        private void UpdateAnimation()
        {
            _anim.SetFloat(AnimMoveX, _lastMoveDir.x);
            _anim.SetFloat(AnimMoveY, _lastMoveDir.y);
            _anim.SetFloat(AnimSpeed, _moveInput.sqrMagnitude);
        }

        // ─── 이동 제어 ─────────────────────────────────────────

        public void SetCanMove(bool canMove)
        {
            _canMove = canMove;
            if (!canMove)
            {
                _rb.linearVelocity = Vector2.zero;
                _moveInput = Vector2.zero;
            }
        }

        private void OnGameStateChanged(Core.GameManager.GameState state)
        {
            // 전투/대화 중에는 이동 불가
            bool movable = state == Core.GameManager.GameState.Field;
            SetCanMove(movable);
        }

        // ─── 기즈모 ────────────────────────────────────────────

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(
                transform.position + (Vector3)_lastMoveDir * 0.5f,
                _interactRadius
            );
        }
    }

    // 이벤트 정의
    public struct PlayerInteractEvent
    {
        public GameObject Target;
        public Vector3 PlayerPosition;
    }
}
