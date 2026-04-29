using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SaintSeiya.Characters;
using SaintSeiya.Combat;

namespace SaintSeiya.UI
{
<<<<<<< HEAD
    /// <summary>
    /// 전투/필드 HUD 전체를 관리
    /// HP바, 코스모 게이지, 레벨/이름 표시
    /// </summary>
=======
>>>>>>> 85d6086137a2cfa6b961a0149bcb69432042ea76
    public class HUDController : MonoBehaviour
    {
        [Header("Player HP")]
        [SerializeField] private Slider _hpSlider;
        [SerializeField] private TextMeshProUGUI _hpText;
        [SerializeField] private Image _hpFill;
        [SerializeField] private Color _hpHighColor = new Color(0.2f, 0.8f, 0.2f);
        [SerializeField] private Color _hpMidColor  = new Color(0.9f, 0.7f, 0.1f);
        [SerializeField] private Color _hpLowColor  = new Color(0.9f, 0.1f, 0.1f);

        [Header("Cosmos Gauge")]
        [SerializeField] private CosmosGaugeUI _cosmosGauge;

        [Header("Player Info")]
        [SerializeField] private TextMeshProUGUI _playerNameText;
        [SerializeField] private TextMeshProUGUI _levelText;

        [Header("Battle UI")]
        [SerializeField] private GameObject _battlePanel;
<<<<<<< HEAD
        [SerializeField] private GameObject _skillButtonContainer;
=======
>>>>>>> 85d6086137a2cfa6b961a0149bcb69432042ea76

        private CharacterStats _player;

        void Start()
        {
<<<<<<< HEAD
            // GameManager에서 플레이어 연결 (전투씬 진입 시 호출)
=======
>>>>>>> 85d6086137a2cfa6b961a0149bcb69432042ea76
            Core.GameManager.OnGameStateChanged += OnGameStateChanged;
        }

        void OnDestroy()
        {
            Core.GameManager.OnGameStateChanged -= OnGameStateChanged;
<<<<<<< HEAD
            if (_player != null)
            {
                _player.OnHPChanged -= UpdateHP;
            }
=======
            if (_player != null) _player.OnHPChanged -= UpdateHP;
>>>>>>> 85d6086137a2cfa6b961a0149bcb69432042ea76
        }

        public void BindPlayer(CharacterStats player)
        {
<<<<<<< HEAD
            if (_player != null)
                _player.OnHPChanged -= UpdateHP;

            _player = player;
            _player.OnHPChanged += UpdateHP;

            // 초기값 세팅
            UpdateHP(_player.CurrentHP, _player.MaxHP);

            if (_player.data != null)
            {
                _playerNameText?.SetText(_player.data.characterName);
            }

            // 코스모 게이지 연결
            if (_player.cosmos != null)
                _cosmosGauge?.Bind(_player.cosmos);
=======
            if (_player != null) _player.OnHPChanged -= UpdateHP;
            _player = player;
            _player.OnHPChanged += UpdateHP;
            UpdateHP(_player.CurrentHP, _player.MaxHP);
            if (_player.data != null) _playerNameText?.SetText(_player.data.characterName);
            if (_player.cosmos != null) _cosmosGauge?.Bind(_player.cosmos);
>>>>>>> 85d6086137a2cfa6b961a0149bcb69432042ea76
        }

        private void UpdateHP(float current, float max)
        {
<<<<<<< HEAD
            if (_hpSlider != null)
            {
                _hpSlider.value = max > 0 ? current / max : 0f;
            }

            _hpText?.SetText($"{Mathf.CeilToInt(current)} / {Mathf.CeilToInt(max)}");

            // HP 비율에 따라 색상 변경
            if (_hpFill != null)
            {
                float ratio = max > 0 ? current / max : 0f;
                _hpFill.color = ratio > 0.5f ? _hpHighColor
                              : ratio > 0.25f ? _hpMidColor
                              : _hpLowColor;
=======
            if (_hpSlider != null) _hpSlider.value = max > 0 ? current / max : 0f;
            _hpText?.SetText($"{Mathf.CeilToInt(current)} / {Mathf.CeilToInt(max)}");
            if (_hpFill != null)
            {
                float ratio = max > 0 ? current / max : 0f;
                _hpFill.color = ratio > 0.5f ? _hpHighColor : ratio > 0.25f ? _hpMidColor : _hpLowColor;
>>>>>>> 85d6086137a2cfa6b961a0149bcb69432042ea76
            }
        }

        private void OnGameStateChanged(Core.GameManager.GameState state)
        {
<<<<<<< HEAD
            bool isBattle = state == Core.GameManager.GameState.Battle;
            _battlePanel?.SetActive(isBattle);
        }

        public void SetLevel(int level)
        {
            _levelText?.SetText($"Lv.{level}");
        }
=======
            _battlePanel?.SetActive(state == Core.GameManager.GameState.Battle);
        }

        public void SetLevel(int level) => _levelText?.SetText($"Lv.{level}");
>>>>>>> 85d6086137a2cfa6b961a0149bcb69432042ea76
    }
}
