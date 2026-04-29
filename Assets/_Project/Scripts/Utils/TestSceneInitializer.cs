using UnityEngine;
using SaintSeiya.Combat;
using SaintSeiya.Characters;
using SaintSeiya.UI;

namespace SaintSeiya.Utils
{
<<<<<<< HEAD
    /// <summary>
    /// 테스트 씬 전용 빠른 초기화 스크립트
    /// 스프라이트/애니메이터 없이도 이동/전투/코스모를 바로 테스트 가능
    /// </summary>
    public class TestSceneInitializer : MonoBehaviour
    {
        [Header("테스트 설정")]
        [SerializeField] private bool _autoInitPlayer  = true;
        [SerializeField] private bool _autoInitEnemy   = true;
        [SerializeField] private bool _autoInitHUD     = true;
        [SerializeField] private bool _showDebugOverlay = true;

        [Header("References")]
        [SerializeField] private CharacterStats _player;
        [SerializeField] private CharacterStats _enemy;
        [SerializeField] private HUDController  _hud;
=======
    public class TestSceneInitializer : MonoBehaviour
    {
        [SerializeField] private CharacterStats _player;
        [SerializeField] private CharacterStats _enemy;
        [SerializeField] private HUDController  _hud;
        [SerializeField] private bool _showDebugOverlay = true;
>>>>>>> 85d6086137a2cfa6b961a0149bcb69432042ea76

        private CosmosSystem _cosmos;

        void Start()
        {
<<<<<<< HEAD
            // GameManager가 없으면 생성 (Boot 씬 없이 테스트할 때)
=======
>>>>>>> 85d6086137a2cfa6b961a0149bcb69432042ea76
            if (Core.GameManager.Instance == null)
            {
                var gm = new GameObject("GameManager");
                gm.AddComponent<Core.GameManager>();
                gm.AddComponent<Core.SaveManager>();
            }
<<<<<<< HEAD

            if (_autoInitPlayer && _player != null)
            {
                _player.InitFromData();
                _cosmos = _player.GetComponent<CosmosSystem>();

                // 자동 스프라이트 (흰색 사각형)
                var sr = _player.GetComponent<SpriteRenderer>();
                if (sr != null && sr.sprite == null)
                    sr.sprite = DebugSpriteFactory.CreateRect(new Color(0.3f, 0.6f, 1f));

                Debug.Log($"[TestInit] 플레이어 초기화: HP={_player.CurrentHP}/{_player.MaxHP}");
            }

            if (_autoInitEnemy && _enemy != null)
            {
                _enemy.InitFromData();

                var sr = _enemy.GetComponent<SpriteRenderer>();
                if (sr != null && sr.sprite == null)
                    sr.sprite = DebugSpriteFactory.CreateRect(new Color(1f, 0.3f, 0.3f));

                Debug.Log($"[TestInit] 적 초기화: HP={_enemy.CurrentHP}/{_enemy.MaxHP}");
            }

            if (_autoInitHUD && _hud != null && _player != null)
            {
                _hud.BindPlayer(_player);
                _hud.SetLevel(1);
            }

            // 필드 상태로 진입
            Core.GameManager.Instance?.ChangeState(Core.GameManager.GameState.Field);
        }

        // ─── 디버그 GUI ─────────────────────────────────────────

        void OnGUI()
        {
            if (!_showDebugOverlay) return;

            GUILayout.BeginArea(new Rect(10, 10, 280, 400));
            GUILayout.Label("=== 🔧 Debug Overlay ===");

            if (_player != null)
            {
                GUILayout.Label($"Player HP: {_player.CurrentHP:F0} / {_player.MaxHP:F0}");
                if (_cosmos != null)
                    GUILayout.Label($"Cosmos: {_cosmos.CurrentCosmos:F0} / {_cosmos.MaxCosmos:F0}" +
                                    (_cosmos.IsBurning ? " 🔥 BURNING" : ""));
            }

            GUILayout.Space(5);
            GUILayout.Label("--- 조작 안내 ---");
            GUILayout.Label("WASD / 화살표: 이동");
            GUILayout.Label("Shift: 대시");
            GUILayout.Label("F: 상호작용");
            GUILayout.Label("I: 인벤토리");

            GUILayout.Space(5);
            GUILayout.Label("--- 빠른 테스트 ---");

=======
            if (Core.AudioManager.Instance == null) new GameObject("AudioManager").AddComponent<Core.AudioManager>();
            if (Inventory.InventoryManager.Instance == null) new GameObject("InventoryManager").AddComponent<Inventory.InventoryManager>();

            if (_player != null)
            {
                _player.InitFromData();
                _cosmos = _player.GetComponent<CosmosSystem>();
                var sr = _player.GetComponent<SpriteRenderer>();
                if (sr != null && sr.sprite == null) sr.sprite = DebugSpriteFactory.CreateRect(new Color(0.3f,0.6f,1f));
            }
            if (_enemy != null)
            {
                _enemy.InitFromData();
                var sr = _enemy.GetComponent<SpriteRenderer>();
                if (sr != null && sr.sprite == null) sr.sprite = DebugSpriteFactory.CreateRect(new Color(1f,0.3f,0.3f));
            }
            if (_hud != null && _player != null) { _hud.BindPlayer(_player); _hud.SetLevel(1); }

            Core.GameManager.Instance?.ChangeState(Core.GameManager.GameState.Field);
            Debug.Log("[TestInit] 테스트 씬 초기화 완료!");
        }

        void OnGUI()
        {
            if (!_showDebugOverlay) return;
            GUILayout.BeginArea(new Rect(10, 10, 260, 380));
            GUILayout.Label("=== 🔧 Debug Overlay ===");
            if (_player != null) GUILayout.Label($"Player HP: {_player.CurrentHP:F0} / {_player.MaxHP:F0}");
            if (_cosmos  != null) GUILayout.Label($"Cosmos: {_cosmos.CurrentCosmos:F0} / {_cosmos.MaxCosmos:F0}" + (_cosmos.IsBurning ? " 🔥" : ""));
            GUILayout.Space(5);
            GUILayout.Label("WASD: 이동 | Shift: 대시 | F: 상호작용 | I: 인벤토리");
            GUILayout.Space(5);
>>>>>>> 85d6086137a2cfa6b961a0149bcb69432042ea76
            if (_cosmos != null)
            {
                if (GUILayout.Button("코스모 +30"))  _cosmos.GainCosmos(30f);
                if (GUILayout.Button("코스모 MAX"))  _cosmos.GainCosmos(100f);
                if (GUILayout.Button("코스모 연소")) _cosmos.BurnCosmos();
            }
<<<<<<< HEAD

            if (_player != null)
            {
                if (GUILayout.Button("HP -50"))  _player.TakeDamage(50f);
                if (GUILayout.Button("HP 풀회복")) _player.Heal(9999f);
            }

            if (_enemy != null)
=======
            if (_player != null)
            {
                if (GUILayout.Button("HP -50"))   _player.TakeDamage(50f);
                if (GUILayout.Button("HP 풀회복")) _player.Heal(9999f);
            }
            if (_player != null && _enemy != null)
>>>>>>> 85d6086137a2cfa6b961a0149bcb69432042ea76
            {
                if (GUILayout.Button("전투 시작"))
                {
                    Core.GameManager.Instance?.ChangeState(Core.GameManager.GameState.Battle);
<<<<<<< HEAD
                    var bm = FindFirstObjectByType<BattleManager>();
                    bm?.StartBattle(_player, new System.Collections.Generic.List<CharacterStats> { _enemy });
                }
            }

=======
                    FindFirstObjectByType<BattleManager>()?.StartBattle(_player, new System.Collections.Generic.List<CharacterStats>{_enemy});
                }
            }
>>>>>>> 85d6086137a2cfa6b961a0149bcb69432042ea76
            GUILayout.EndArea();
        }
    }
}
