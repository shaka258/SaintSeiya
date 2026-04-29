using UnityEngine;

namespace SaintSeiya.Utils
{
    /// <summary>
<<<<<<< HEAD
    /// 개발 중 빠른 테스트를 위한 씬 초기화 헬퍼
    /// Field 씬에서 단독 실행 시 필요한 매니저들을 자동 생성
    /// (Boot 씬 없이 현재 씬에서 Play 버튼 눌렀을 때)
    /// </summary>
    public class DevSceneBootstrap : MonoBehaviour
    {
        [Header("Dev Only — 빌드에서 자동 비활성화")]
        [SerializeField] private bool _enableInBuild = false;

        [Header("Manager Prefabs")]
        [SerializeField] private GameObject _gameManagerPrefab;
        [SerializeField] private GameObject _audioManagerPrefab;
        [SerializeField] private GameObject _inventoryManagerPrefab;
        [SerializeField] private GameObject _dialogueManagerPrefab;

        void Awake()
        {
#if !UNITY_EDITOR
            if (!_enableInBuild) { gameObject.SetActive(false); return; }
#endif
            EnsureManager<Core.GameManager>(_gameManagerPrefab);
            EnsureManager<Core.AudioManager>(_audioManagerPrefab);
            EnsureManager<Inventory.InventoryManager>(_inventoryManagerPrefab);
            EnsureManager<Dialogue.DialogueManager>(_dialogueManagerPrefab);

            Debug.Log("[DevBootstrap] 개발용 매니저 초기화 완료");
        }

        private void EnsureManager<T>(GameObject prefab) where T : MonoBehaviour
        {
            if (FindFirstObjectByType<T>() != null) return;

            if (prefab != null)
                Instantiate(prefab);
            else
            {
                // 프리팹 없으면 빈 오브젝트로 생성
                var go = new GameObject(typeof(T).Name);
                go.AddComponent<T>();
            }
=======
    /// Boot 씬 없이 개발 중 특정 씬에서 바로 Play 버튼 눌렀을 때
    /// 필요한 매니저들을 자동으로 생성해주는 개발용 헬퍼
    /// </summary>
    public class DevSceneBootstrap : MonoBehaviour
    {
        void Awake()
        {
#if !UNITY_EDITOR
            Destroy(gameObject); return;
#endif
            EnsureManager<Core.GameManager>();
            EnsureManager<Core.AudioManager>();
            EnsureManager<Inventory.InventoryManager>();
            EnsureManager<Dialogue.DialogueManager>();
            Debug.Log("[DevBootstrap] 개발용 매니저 초기화 완료");
        }

        private void EnsureManager<T>() where T : MonoBehaviour
        {
            if (FindFirstObjectByType<T>() != null) return;
            new GameObject(typeof(T).Name).AddComponent<T>();
>>>>>>> 85d6086137a2cfa6b961a0149bcb69432042ea76
        }
    }
}
