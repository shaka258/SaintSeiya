using UnityEngine;
using UnityEngine.UI;
using TMPro;
<<<<<<< HEAD
using DG.Tweening;

namespace SaintSeiya.UI
{
    /// <summary>
    /// 메인 메뉴 UI 컨트롤러
    /// - 게임 시작 / 불러오기 / 설정 / 종료
    /// </summary>
    public class MainMenuUI : MonoBehaviour
    {
        [Header("Buttons")]
=======

namespace SaintSeiya.UI
{
    public class MainMenuUI : MonoBehaviour
    {
>>>>>>> 85d6086137a2cfa6b961a0149bcb69432042ea76
        [SerializeField] private Button _newGameButton;
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _quitButton;
<<<<<<< HEAD

        [Header("Panels")]
        [SerializeField] private GameObject _settingsPanel;

        [Header("Title")]
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private CanvasGroup _mainGroup;

=======
        [SerializeField] private GameObject _settingsPanel;

>>>>>>> 85d6086137a2cfa6b961a0149bcb69432042ea76
        void Start()
        {
            _newGameButton?.onClick.AddListener(OnNewGame);
            _continueButton?.onClick.AddListener(OnContinue);
<<<<<<< HEAD
            _settingsButton?.onClick.AddListener(OnSettings);
            _quitButton?.onClick.AddListener(OnQuit);

            // 저장 데이터 있으면 계속하기 활성화
            bool hasSave = Core.GameManager.Instance?.SaveManager?.HasSaveData ?? false;
            _continueButton?.gameObject.SetActive(hasSave);

            // 페이드인 연출
            if (_mainGroup != null)
            {
                _mainGroup.alpha = 0f;
                _mainGroup.DOFade(1f, 1f).SetDelay(0.3f);
            }

            // 타이틀 맥동
            _titleText?.transform.DOPunchScale(Vector3.one * 0.05f, 2f, 3, 0.5f)
                .SetLoops(-1, LoopType.Restart);

            Core.AudioManager.Instance?.PlayBGM(Core.AudioManager.Instance.mainMenuBGM);
        }

        private void OnNewGame()
        {
            Core.AudioManager.Instance?.StopBGM();
            // 페이드아웃 후 씬 전환
            _mainGroup?.DOFade(0f, 0.5f).OnComplete(() =>
                Core.GameManager.Instance?.LoadScene("WorldMap"));
        }
=======
            _settingsButton?.onClick.AddListener(() => _settingsPanel?.SetActive(true));
            _quitButton?.onClick.AddListener(() => Core.GameManager.Instance?.QuitGame());

            bool hasSave = Core.GameManager.Instance?.SaveManager?.HasSaveData ?? false;
            _continueButton?.gameObject.SetActive(hasSave);

            Core.AudioManager.Instance?.PlayBGM(Core.AudioManager.Instance.mainMenuBGM);
        }

        private void OnNewGame() => Core.GameManager.Instance?.LoadScene("WorldMap");
>>>>>>> 85d6086137a2cfa6b961a0149bcb69432042ea76

        private void OnContinue()
        {
            var save = Core.GameManager.Instance?.SaveManager?.Load();
<<<<<<< HEAD
            if (save == null) return;

            Core.AudioManager.Instance?.StopBGM();
            _mainGroup?.DOFade(0f, 0.5f).OnComplete(() =>
                Core.GameManager.Instance?.LoadScene(save.playerProgress?.currentScene ?? "WorldMap"));
        }

        private void OnSettings()
        {
            _settingsPanel?.SetActive(true);
        }

        private void OnQuit()
        {
            Core.GameManager.Instance?.QuitGame();
=======
            if (save != null) Core.GameManager.Instance?.LoadScene(save.playerProgress?.currentScene ?? "WorldMap");
>>>>>>> 85d6086137a2cfa6b961a0149bcb69432042ea76
        }
    }
}
