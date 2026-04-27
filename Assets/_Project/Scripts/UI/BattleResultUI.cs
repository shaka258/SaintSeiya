using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

        private void OnBattleEnd(Core.BattleEndEvent e)
        {
            gameObject.SetActive(true);
            _victoryPanel?.SetActive(e.IsVictory);
            _defeatPanel?.SetActive(!e.IsVictory);
            if (e.IsVictory) _expText?.SetText($"경험치 + {e.ExpGained}");
        }
    }
}
