using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SaintSeiya.Combat;

namespace SaintSeiya.UI
{
    public class CosmosGaugeUI : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private Image _fill;
        [SerializeField] private TextMeshProUGUI _valueText;
        [SerializeField] private Color _normalColor  = new Color(0.2f, 0.5f, 1f);
        [SerializeField] private Color _maxColor     = new Color(1f, 0.85f, 0.1f);
        [SerializeField] private Color _burningColor = new Color(1f, 0.4f, 0f);
        [SerializeField] private GameObject _maxGlow;
        [SerializeField] private GameObject _burningEffect;
        [SerializeField] private Button _burnButton;

        private CosmosSystem _cosmos;

        void Start() => _burnButton?.onClick.AddListener(() => _cosmos?.BurnCosmos());
        void OnDestroy() => Unbind();

        public void Bind(CosmosSystem cosmos)
        {
            Unbind();
            _cosmos = cosmos;
            _cosmos.OnCosmosChanged += UpdateGauge;
            _cosmos.OnCosmosMax     += OnCosmosMax;
            _cosmos.OnBurnStart     += OnBurnStart;
            _cosmos.OnBurnEnd       += OnBurnEnd;
            UpdateGauge(_cosmos.CurrentCosmos, _cosmos.MaxCosmos);
        }

        public void Unbind()
        {
            if (_cosmos == null) return;
            _cosmos.OnCosmosChanged -= UpdateGauge;
            _cosmos.OnCosmosMax     -= OnCosmosMax;
            _cosmos.OnBurnStart     -= OnBurnStart;
            _cosmos.OnBurnEnd       -= OnBurnEnd;
            _cosmos = null;
        }

        private void UpdateGauge(float current, float max)
        {
            if (_slider != null) _slider.value = max > 0 ? current / max : 0f;
            _valueText?.SetText($"{Mathf.FloorToInt(current)} / {Mathf.FloorToInt(max)}");
            if (_fill != null && !(_cosmos?.IsBurning ?? false))
                _fill.color = Color.Lerp(_normalColor, _maxColor, max > 0 ? current / max : 0f);
            if (_burnButton != null)
                _burnButton.interactable = (max > 0 ? current / max : 0f) >= 0.5f && !(_cosmos?.IsBurning ?? false);
        }

        private void OnCosmosMax()    => _maxGlow?.SetActive(true);
        private void OnBurnStart()    { _burningEffect?.SetActive(true); _maxGlow?.SetActive(false); if (_fill != null) _fill.color = _burningColor; }
        private void OnBurnEnd()      { _burningEffect?.SetActive(false); if (_fill != null) _fill.color = _normalColor; }
    }
}
