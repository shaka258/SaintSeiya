using UnityEngine;
using UnityEngine.SceneManagement;

namespace SaintSeiya.Core
{
    /// <summary>
    /// 게임 전체 상태를 관리하는 싱글톤 매니저
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public enum GameState
        {
            Boot,
            MainMenu,
            Field,
            Battle,
            Dialogue,
            Paused
        }

        [Header("Game State")]
        [SerializeField] private GameState _currentState = GameState.Boot;
        public GameState CurrentState => _currentState;

        [Header("References")]
        public SaveManager SaveManager;
        public AudioManager AudioManager;

        public static event System.Action<GameState> OnGameStateChanged;

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        void Start()
        {
            Application.targetFrameRate = 60;
            ChangeState(GameState.MainMenu);
        }

        public void ChangeState(GameState newState)
        {
            _currentState = newState;
            OnGameStateChanged?.Invoke(newState);
            Debug.Log($"[GameManager] State → {newState}");
        }

        public void LoadScene(string sceneName)
        {
            StartCoroutine(LoadSceneAsync(sceneName));
        }

        private System.Collections.IEnumerator LoadSceneAsync(string sceneName)
        {
            // TODO: 로딩 화면 표시
            AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
            while (!op.isDone) yield return null;
        }

        public void PauseGame()
        {
            if (_currentState == GameState.Paused) return;
            Time.timeScale = 0f;
            ChangeState(GameState.Paused);
        }

        public void ResumeGame()
        {
            Time.timeScale = 1f;
            ChangeState(GameState.Field); // or previous state
        }

        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
