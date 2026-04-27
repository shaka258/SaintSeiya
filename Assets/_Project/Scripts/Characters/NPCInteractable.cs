using UnityEngine;

namespace SaintSeiya.Characters
{
    public class NPCInteractable : MonoBehaviour
    {
        public string npcName;
        [TextArea] public string[] dialogueLines;
        public string questId;
        [SerializeField] private GameObject _interactPrompt;
        [SerializeField] private float _promptRange = 1.5f;

        private Transform _player;
        private bool _isInRange;

        void Start()
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p) _player = p.transform;
            _interactPrompt?.SetActive(false);
            PlayerController.OnInteractTriggered += OnInteractTriggered;
        }

        void OnDestroy() => PlayerController.OnInteractTriggered -= OnInteractTriggered;

        void Update()
        {
            if (_player == null) return;
            bool inRange = Vector2.Distance(transform.position, _player.position) <= _promptRange;
            if (inRange != _isInRange) { _isInRange = inRange; _interactPrompt?.SetActive(_isInRange); }
        }

        private void OnInteractTriggered(GameObject target)
        {
            if (target != gameObject) return;
            Core.GameManager.Instance?.ChangeState(Core.GameManager.GameState.Dialogue);
            Core.EventBus.Publish(new Core.DialogueStartEvent { DialogueId = $"npc_{npcName.ToLower()}_default" });
        }
    }
}
