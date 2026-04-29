using UnityEngine;
using UnityEngine.UI;
using TMPro;
<<<<<<< HEAD
using DG.Tweening;

namespace SaintSeiya.UI
{
    /// <summary>
    /// 전투 결과 팝업 (승리 / 패배)
    /// 획득 경험치, 보상 표시 후 씬 전환
    /// </summary>
    public class BattleResultUI : MonoBehaviour
    {
        [Header("Panels")]
        [SerializeField] private GameObject _victoryPanel;
        [SerializeField] private GameObject _defeatPanel;

        [Header("Victory UI")]
        [SerializeField] private TextMeshProUGUI _expText;
        [SerializeField] private Button _continueButton;

        [Header("Defeat UI")]
        [SerializeField] private Button _retryButton;
        [SerializeField] private Button _mainMenuButton;

        [Header("Animation")]
        [SerializeField] private CanvasGroup _canvasGroup;

        void Start()
        {
            gameObject.SetActive(false);
            _continueButton?.onClick.AddListener(OnContinue);
            _retryButton?.onClick.AddListener(OnRetry);
            _mainMenuButton?.onClick.AddListener(OnMainMenu);

            Core.EventBus.Subscribe<Core.BattleEndEvent>(OnBattleEnd);
        }

        void OnDestroy()
        {
            Core.EventBus.Unsubscribe<Core.BattleEndEvent>(OnBattleEnd);
        }
=======

namespace SaintSeiya.UI
{
    public class BattleResultUI : MonoBehaviour
    {
        [SerializeField] private GameObject _victoryPanel;
        [SerializeField] private GameObject _defeatPanel;
        [SerializeField] private TextMeshProUGUI _expText;
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _retryButton;
        [SerializeField] private Button _mainMenuButton;

        void Start()
        {
            gameObject.SetActive(false);
            _continueButton?.onClick.AddListener(() => Core.GameManager.Instance?.LoadScene("WorldMap"));
            _retryButton?.onClick.AddListener(() => Core.GameManager.Instance?.LoadScene("Battle"));
            _mainMenuButton?.onClick.AddListener(() => Core.GameManager.Instance?.LoadScene("MainMenu"));
            Core.EventBus.Subscribe<Core.BattleEndEvent>(OnBattleEnd);
        }

        void OnDestroy() => Core.EventBus.Unsubscribe<Core.BattleEndEvent>(OnBattleEnd);
>>>>>>> 85d6086137a2cfa6b961a0149bcb69432042ea76

        private void OnBattleEnd(Core.BattleEndEvent e)
        {
            gameObject.SetActive(true);
<<<<<<< HEAD

            _victoryPanel?.SetActive(e.IsVictory);
            _defeatPanel?.SetActive(!e.IsVictory);

            if (e.IsVictory)
                _expText?.SetText($"경험치 + {e.ExpGained}");

            // 페이드인
            if (_canvasGroup != null)
            {
                _canvasGroup.alpha = 0f;
                _canvasGroup.DOFade(1f, 0.8f).SetDelay(0.5f);
            }
        }

        private void OnContinue()
        {
            Core.GameManager.Instance?.LoadScene("WorldMap");
        }

        private void OnRetry()
        {
            Core.GameManager.Instance?.LoadScene("Battle");
        }

        private void OnMainMenu()
        {
            Core.GameManager.Instance?.LoadScene("MainMenu");
=======
            _victoryPanel?.SetActive(e.IsVictory);
            _defeatPanel?.SetActive(!e.IsVictory);
            if (e.IsVictory) _expText?.SetText($"경험치 + {e.ExpGained}");
>>>>>>> 85d6086137a2cfa6b961a0149bcb69432042ea76
        }
    }
}
