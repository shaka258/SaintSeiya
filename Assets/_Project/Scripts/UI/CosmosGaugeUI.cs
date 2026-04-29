using UnityEngine;
using UnityEngine.UI;
using TMPro;
<<<<<<< HEAD
using DG.Tweening;
=======
>>>>>>> 85d6086137a2cfa6b961a0149bcb69432042ea76
using SaintSeiya.Combat;

namespace SaintSeiya.UI
{
<<<<<<< HEAD
    /// <summary>
    /// 코스모 게이지 UI
    /// - 게이지 바 채움
    /// - 연소(Burning) 상태 시 황금빛 맥동 이펙트
    /// - 최대치 도달 시 반짝임 연출
    /// </summary>
    public class CosmosGaugeUI : MonoBehaviour
    {
        [Header("Gauge")]
        [SerializeField] private Slider _slider;
        [SerializeField] private Image _fill;
        [SerializeField] private TextMeshProUGUI _valueText;

        [Header("Colors")]
        [SerializeField] private Color _normalColor   = new Color(0.2f, 0.5f, 1f);
        [SerializeField] private Color _maxColor      = new Color(1f, 0.85f, 0.1f);
        [SerializeField] private Color _burningColor  = new Color(1f, 0.4f, 0f);

        [Header("VFX Objects")]
        [SerializeField] private GameObject _maxGlow;      // 최대 도달 글로우
        [SerializeField] private GameObject _burningEffect; // 연소 이펙트

        [Header("Burn Button")]
        [SerializeField] private Button _burnButton;       // 코스모 연소 버튼
        [SerializeField] private TextMeshProUGUI _burnButtonText;

        private CosmosSystem _cosmos;
        private Tweener _pulseTween;

        void Start()
        {
            _burnButton?.onClick.AddListener(OnBurnButtonClicked);
            _maxGlow?.SetActive(false);
            _burningEffect?.SetActive(false);
        }

        void OnDestroy()
        {
            Unbind();
        }
=======
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
>>>>>>> 85d6086137a2cfa6b961a0149bcb69432042ea76

        public void Bind(CosmosSystem cosmos)
        {
            Unbind();
            _cosmos = cosmos;
            _cosmos.OnCosmosChanged += UpdateGauge;
            _cosmos.OnCosmosMax     += OnCosmosMax;
            _cosmos.OnBurnStart     += OnBurnStart;
            _cosmos.OnBurnEnd       += OnBurnEnd;
<<<<<<< HEAD

            // 초기값
=======
>>>>>>> 85d6086137a2cfa6b961a0149bcb69432042ea76
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
<<<<<<< HEAD
            if (_slider != null)
                _slider.value = max > 0 ? current / max : 0f;

            _valueText?.SetText($"{Mathf.FloorToInt(current)} / {Mathf.FloorToInt(max)}");

            // 색상: 일반 → 최대 그라데이션
            if (_fill != null && !(_cosmos?.IsBurning ?? false))
            {
                float ratio = max > 0 ? current / max : 0f;
                _fill.color = Color.Lerp(_normalColor, _maxColor, ratio);
            }

            // 연소 버튼 활성화 (50% 이상)
            if (_burnButton != null)
            {
                float ratio = max > 0 ? current / max : 0f;
                _burnButton.interactable = ratio >= 0.5f && !(_cosmos?.IsBurning ?? false);
            }
        }

        private void OnCosmosMax()
        {
            _maxGlow?.SetActive(true);

            // 글로우 맥동 효과 (DOTween)
            if (_fill != null)
            {
                _pulseTween?.Kill();
                _pulseTween = _fill.DOColor(_maxColor * 1.3f, 0.4f)
                    .SetLoops(4, LoopType.Yoyo)
                    .OnComplete(() => _maxGlow?.SetActive(false));
            }
        }

        private void OnBurnStart()
        {
            _burningEffect?.SetActive(true);
            _maxGlow?.SetActive(false);

            if (_fill != null)
            {
                _pulseTween?.Kill();
                _fill.color = _burningColor;
                _pulseTween = _fill.DOColor(_burningColor * 1.5f, 0.3f)
                    .SetLoops(-1, LoopType.Yoyo); // 무한 맥동
            }

            _burnButton?.gameObject.SetActive(false);
            _burnButtonText?.SetText("연소 중...");
        }

        private void OnBurnEnd()
        {
            _burningEffect?.SetActive(false);
            _pulseTween?.Kill();

            if (_fill != null)
                _fill.color = _normalColor;

            _burnButton?.gameObject.SetActive(true);
            _burnButtonText?.SetText("코스모 연소");
        }

        private void OnBurnButtonClicked()
        {
            _cosmos?.BurnCosmos();
        }
=======
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
>>>>>>> 85d6086137a2cfa6b961a0149bcb69432042ea76
    }
}
