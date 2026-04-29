using UnityEngine;

namespace SaintSeiya.Characters
{
<<<<<<< HEAD
    /// <summary>
    /// 씬 전환 트리거 — 특정 지점에 닿으면 다음 씬으로 이동
    /// Collider2D (IsTrigger=true) 와 함께 사용
    /// </summary>
    public class SceneTransitionTrigger : MonoBehaviour
    {
        [Header("Transition")]
        [SerializeField] private string _targetScene;
        [SerializeField] private Vector2 _spawnPosition; // 도착 씬에서의 스폰 위치
        [SerializeField] private float _transitionDelay = 0.3f;

        [Header("Condition")]
        [SerializeField] private string _requiredQuestId; // 특정 퀘스트 완료 시에만 통과

=======
    public class SceneTransitionTrigger : MonoBehaviour
    {
        [SerializeField] private string _targetScene;
        [SerializeField] private Vector2 _spawnPosition;
        [SerializeField] private float _transitionDelay = 0.3f;
>>>>>>> 85d6086137a2cfa6b961a0149bcb69432042ea76
        private bool _triggered;

        void OnTriggerEnter2D(Collider2D other)
        {
<<<<<<< HEAD
            if (_triggered) return;
            if (!other.CompareTag("Player")) return;

            // 퀘스트 조건 체크
            if (!string.IsNullOrEmpty(_requiredQuestId))
            {
                // TODO: QuestManager에서 완료 여부 확인
                Debug.Log($"[SceneTransition] 퀘스트 조건: {_requiredQuestId}");
            }

=======
            if (_triggered || !other.CompareTag("Player")) return;
>>>>>>> 85d6086137a2cfa6b961a0149bcb69432042ea76
            _triggered = true;
            StartCoroutine(TransitionRoutine());
        }

        System.Collections.IEnumerator TransitionRoutine()
        {
<<<<<<< HEAD
            // 이동 잠금
            var player = GameObject.FindGameObjectWithTag("Player");
            player?.GetComponent<PlayerController>()?.SetCanMove(false);

            Core.EventBus.Publish(new Core.SceneTransitionEvent
            {
                FromScene = gameObject.scene.name,
                ToScene = _targetScene
            });

            yield return new UnityEngine.WaitForSeconds(_transitionDelay);

            // 스폰 위치 저장
            PlayerPrefs.SetFloat("SpawnX", _spawnPosition.x);
            PlayerPrefs.SetFloat("SpawnY", _spawnPosition.y);

            Core.GameManager.Instance?.LoadScene(_targetScene);
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            var col = GetComponent<Collider2D>();
            if (col != null)
                Gizmos.DrawWireCube(transform.position, col.bounds.size);
        }
=======
            GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerController>()?.SetCanMove(false);
            Core.EventBus.Publish(new Core.SceneTransitionEvent { FromScene = gameObject.scene.name, ToScene = _targetScene });
            yield return new WaitForSeconds(_transitionDelay);
            PlayerPrefs.SetFloat("SpawnX", _spawnPosition.x);
            PlayerPrefs.SetFloat("SpawnY", _spawnPosition.y);
            Core.GameManager.Instance?.LoadScene(_targetScene);
        }
>>>>>>> 85d6086137a2cfa6b961a0149bcb69432042ea76
    }
}
