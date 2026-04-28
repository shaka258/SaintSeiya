using UnityEngine;

namespace SaintSeiya.Utils
{
    /// <summary>
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
        }
    }
}
