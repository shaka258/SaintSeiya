using UnityEngine;
using UnityEngine.InputSystem;

namespace SaintSeiya.Characters
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float _moveSpeed    = 5f;
        [SerializeField] private float _dashSpeed    = 12f;
        [SerializeField] private float _dashDuration = 0.2f;
        [SerializeField] private float _dashCooldown = 1f;
        [SerializeField] private float _interactRadius = 1.2f;
        [SerializeField] private LayerMask _interactLayer;

        private Rigidbody2D _rb;
        private Animator _anim;
        private Vector2 _moveInput;
        private Vector2 _lastMoveDir = Vector2.down;
        private bool _isDashing;
        private float _dashTimer;
        private float _dashCooldownTimer;
        private bool _canMove = true;

        private static readonly int AnimMoveX  = Animator.StringToHash("MoveX");
        private static readonly int AnimMoveY  = Animator.StringToHash("MoveY");
        private static readonly int AnimSpeed  = Animator.StringToHash("Speed");
        private static readonly int AnimIsDash = Animator.StringToHash("IsDash");

        public static event System.Action<GameObject> OnInteractTriggered;

        void Awake() { _rb = GetComponent<Rigidbody2D>(); _anim = GetComponent<Animator>(); }
        void OnEnable()  => Core.GameManager.OnGameStateChanged += OnGameStateChanged;
        void OnDisable() => Core.GameManager.OnGameStateChanged -= OnGameStateChanged;

        public void OnMove(InputValue v)    => _moveInput = v.Get<Vector2>();
        public void OnDash(InputValue v)    { if (v.isPressed) TryDash(); }
        public void OnInteract(InputValue v){ if (v.isPressed) TryInteract(); }

        void Update()
        {
            if (_isDashing) { _dashTimer -= Time.deltaTime; if (_dashTimer <= 0f) EndDash(); }
            if (_dashCooldownTimer > 0f) _dashCooldownTimer -= Time.deltaTime;
            _anim.SetFloat(AnimMoveX, _lastMoveDir.x);
            _anim.SetFloat(AnimMoveY, _lastMoveDir.y);
            _anim.SetFloat(AnimSpeed, _moveInput.sqrMagnitude);
        }

        void FixedUpdate()
        {
            if (!_canMove) return;
            if (_isDashing) { _rb.linearVelocity = _lastMoveDir * _dashSpeed; return; }
            _rb.linearVelocity = _moveInput * _moveSpeed;
            if (_moveInput.sqrMagnitude > 0.01f) _lastMoveDir = _moveInput.normalized;
        }

        private void TryDash()
        {
            if (_isDashing || _dashCooldownTimer > 0f || !_canMove) return;
            _isDashing = true; _dashTimer = _dashDuration; _dashCooldownTimer = _dashCooldown;
            _anim.SetBool(AnimIsDash, true);
        }

        private void EndDash() { _isDashing = false; _anim.SetBool(AnimIsDash, false); }

        private void TryInteract()
        {
            var hit = Physics2D.OverlapCircle(transform.position + (Vector3)_lastMoveDir * 0.5f, _interactRadius, _interactLayer);
            if (hit != null) OnInteractTriggered?.Invoke(hit.gameObject);
        }

        public void SetCanMove(bool v) { _canMove = v; if (!v) { _rb.linearVelocity = Vector2.zero; _moveInput = Vector2.zero; } }

        private void OnGameStateChanged(Core.GameManager.GameState state)
            => SetCanMove(state == Core.GameManager.GameState.Field);
    }

    public struct PlayerInteractEvent { public GameObject Target; public Vector3 PlayerPosition; }
}
