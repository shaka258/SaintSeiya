using UnityEngine;

namespace SaintSeiya.Characters
{
<<<<<<< HEAD
    /// <summary>
    /// NPC 상호작용 컴포넌트
    /// 플레이어가 접근해 상호작용 키를 누르면 대화/퀘스트 시작
    /// </summary>
    public class NPCInteractable : MonoBehaviour
    {
        [Header("NPC Info")]
        public string npcName;
        [TextArea] public string[] dialogueLines; // 간단한 대화 라인
        public string questId;                    // 부여할 퀘스트 ID (있으면)

        [Header("Visual")]
        [SerializeField] private GameObject _interactPrompt; // "!" 말풍선 오브젝트
=======
    public class NPCInteractable : MonoBehaviour
    {
        public string npcName;
        [TextArea] public string[] dialogueLines;
        public string questId;
        [SerializeField] private GameObject _interactPrompt;
>>>>>>> 85d6086137a2cfa6b961a0149bcb69432042ea76
        [SerializeField] private float _promptRange = 1.5f;

        private Transform _player;
        private bool _isInRange;

        void Start()
        {
<<<<<<< HEAD
            var playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) _player = playerObj.transform;

            _interactPrompt?.SetActive(false);

            // 플레이어 상호작용 이벤트 구독
            PlayerController.OnInteractTriggered += OnInteractTriggered;
        }

        void OnDestroy()
        {
            PlayerController.OnInteractTriggered -= OnInteractTriggered;
        }
=======
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p) _player = p.transform;
            _interactPrompt?.SetActive(false);
            PlayerController.OnInteractTriggered += OnInteractTriggered;
        }

        void OnDestroy() => PlayerController.OnInteractTriggered -= OnInteractTriggered;
>>>>>>> 85d6086137a2cfa6b961a0149bcb69432042ea76

        void Update()
        {
            if (_player == null) return;
<<<<<<< HEAD

            float dist = Vector2.Distance(transform.position, _player.position);
            bool inRange = dist <= _promptRange;

            if (inRange != _isInRange)
            {
                _isInRange = inRange;
                _interactPrompt?.SetActive(_isInRange);
            }
=======
            bool inRange = Vector2.Distance(transform.position, _player.position) <= _promptRange;
            if (inRange != _isInRange) { _isInRange = inRange; _interactPrompt?.SetActive(_isInRange); }
>>>>>>> 85d6086137a2cfa6b961a0149bcb69432042ea76
        }

        private void OnInteractTriggered(GameObject target)
        {
            if (target != gameObject) return;
<<<<<<< HEAD

            // 대화 시작
            Core.GameManager.Instance?.ChangeState(Core.GameManager.GameState.Dialogue);
            Core.EventBus.Publish(new Core.DialogueStartEvent
            {
                DialogueId = $"npc_{npcName.ToLower()}_default"
            });

            // 퀘스트 있으면 수락 제안
            if (!string.IsNullOrEmpty(questId))
            {
                Debug.Log($"[NPC:{npcName}] 퀘스트 제안: {questId}");
                // TODO: QuestUI 팝업 연동
            }

            _interactPrompt?.SetActive(false);
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, _promptRange);
=======
            Core.GameManager.Instance?.ChangeState(Core.GameManager.GameState.Dialogue);
            Core.EventBus.Publish(new Core.DialogueStartEvent { DialogueId = $"npc_{npcName.ToLower()}_default" });
>>>>>>> 85d6086137a2cfa6b961a0149bcb69432042ea76
        }
    }
}
