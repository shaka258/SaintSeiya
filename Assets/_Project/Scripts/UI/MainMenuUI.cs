using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SaintSeiya.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private Button _newGameButton;
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _quitButton;
        [SerializeField] private GameObject _settingsPanel;

        void Start()
        {
            _newGameButton?.onClick.AddListener(OnNewGame);
            _continueButton?.onClick.AddListener(OnContinue);
            _settingsButton?.onClick.AddListener(() => _settingsPanel?.SetActive(true));
            _quitButton?.onClick.AddListener(() => Core.GameManager.Instance?.QuitGame());

            bool hasSave = Core.GameManager.Instance?.SaveManager?.HasSaveData ?? false;
            _continueButton?.gameObject.SetActive(hasSave);

            Core.AudioManager.Instance?.PlayBGM(Core.AudioManager.Instance.mainMenuBGM);
        }

        private void OnNewGame() => Core.GameManager.Instance?.LoadScene("WorldMap");

        private void OnContinue()
        {
            var save = Core.GameManager.Instance?.SaveManager?.Load();
            if (save != null) Core.GameManager.Instance?.LoadScene(save.playerProgress?.currentScene ?? "WorldMap");
        }
    }
}
