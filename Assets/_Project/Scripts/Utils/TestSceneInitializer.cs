using UnityEngine;
using SaintSeiya.Combat;
using SaintSeiya.Characters;
using SaintSeiya.UI;

namespace SaintSeiya.Utils
{
    public class TestSceneInitializer : MonoBehaviour
    {
        [SerializeField] private CharacterStats _player;
        [SerializeField] private CharacterStats _enemy;
        [SerializeField] private HUDController  _hud;
        [SerializeField] private bool _showDebugOverlay = true;

        private CosmosSystem _cosmos;

        void Start()
        {
            if (Core.GameManager.Instance == null)
            {
                var gm = new GameObject("GameManager");
                gm.AddComponent<Core.GameManager>();
                gm.AddComponent<Core.SaveManager>();
            }
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
            if (_cosmos != null)
            {
                if (GUILayout.Button("코스모 +30"))  _cosmos.GainCosmos(30f);
                if (GUILayout.Button("코스모 MAX"))  _cosmos.GainCosmos(100f);
                if (GUILayout.Button("코스모 연소")) _cosmos.BurnCosmos();
            }
            if (_player != null)
            {
                if (GUILayout.Button("HP -50"))   _player.TakeDamage(50f);
                if (GUILayout.Button("HP 풀회복")) _player.Heal(9999f);
            }
            if (_player != null && _enemy != null)
            {
                if (GUILayout.Button("전투 시작"))
                {
                    Core.GameManager.Instance?.ChangeState(Core.GameManager.GameState.Battle);
                    FindFirstObjectByType<BattleManager>()?.StartBattle(_player, new System.Collections.Generic.List<CharacterStats>{_enemy});
                }
            }
            GUILayout.EndArea();
        }
    }
}
