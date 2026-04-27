using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SaintSeiya.Dialogue
{
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager Instance { get; private set; }

        [SerializeField] private List<DialogueData> _allDialogues = new();

        private DialogueData _currentDialogue;
        private int _currentLineIndex;
        private bool _isPlaying;
        private bool _isTyping;
        private Coroutine _typingCoroutine;

        public event System.Action<DialogueLine> OnLineStart;
        public event System.Action<string> OnTextUpdated;
        public event System.Action OnLineComplete;
        public event System.Action<List<DialogueChoice>> OnChoicesShown;
        public event System.Action OnDialogueEnd;

        public bool IsPlaying => _isPlaying;

        void Awake()
        {
            if (Instance != null) { Destroy(gameObject); return; }
            Instance = this;
            Core.EventBus.Subscribe<Core.DialogueStartEvent>(e => {
                var d = FindDialogue(e.DialogueId);
                if (d != null) StartDialogue(d);
            });
        }

        public void StartDialogue(DialogueData data)
        {
            if (_isPlaying) EndDialogue();
            _currentDialogue = data;
            _currentLineIndex = 0;
            _isPlaying = true;
            Core.GameManager.Instance?.ChangeState(Core.GameManager.GameState.Dialogue);
            ShowCurrentLine();
        }

        private void ShowCurrentLine()
        {
            if (_currentLineIndex >= _currentDialogue.lines.Count) { OnAllLinesFinished(); return; }
            var line = _currentDialogue.lines[_currentLineIndex];
            OnLineStart?.Invoke(line);
            if (_typingCoroutine != null) StopCoroutine(_typingCoroutine);
            _typingCoroutine = StartCoroutine(TypeLine(line));
        }

        private IEnumerator TypeLine(DialogueLine line)
        {
            _isTyping = true;
            string current = "";
            if (line.voiceClip != null) Core.AudioManager.Instance?.PlaySFX(line.voiceClip);
            foreach (char c in line.text)
            {
                current += c;
                OnTextUpdated?.Invoke(current);
                yield return new WaitForSeconds(line.displaySpeed);
            }
            _isTyping = false;
            OnLineComplete?.Invoke();
            if (!line.waitForInput) { yield return new WaitForSeconds(1f); Next(); }
        }

        public void Next()
        {
            if (!_isPlaying) return;
            if (_isTyping)
            {
                if (_typingCoroutine != null) StopCoroutine(_typingCoroutine);
                _isTyping = false;
                OnTextUpdated?.Invoke(_currentDialogue.lines[_currentLineIndex].text);
                OnLineComplete?.Invoke();
                return;
            }
            _currentLineIndex++;
            ShowCurrentLine();
        }

        public void SelectChoice(int idx)
        {
            if (_currentDialogue == null || idx >= _currentDialogue.choices.Count) return;
            var choice = _currentDialogue.choices[idx];
            if (!string.IsNullOrEmpty(choice.questIdToAccept))
                Core.EventBus.Publish(new Core.QuestUpdatedEvent { QuestId = choice.questIdToAccept });
            if (!string.IsNullOrEmpty(choice.nextDialogueId))
            {
                var next = FindDialogue(choice.nextDialogueId);
                if (next != null) { StartDialogue(next); return; }
            }
            EndDialogue();
        }

        private void OnAllLinesFinished()
        {
            if (_currentDialogue.choices.Count > 0) { OnChoicesShown?.Invoke(_currentDialogue.choices); return; }
            if (!string.IsNullOrEmpty(_currentDialogue.nextDialogueId))
            {
                var next = FindDialogue(_currentDialogue.nextDialogueId);
                if (next != null) { StartDialogue(next); return; }
            }
            if (!string.IsNullOrEmpty(_currentDialogue.triggerQuestId))
                Core.EventBus.Publish(new Core.QuestUpdatedEvent { QuestId = _currentDialogue.triggerQuestId });
            EndDialogue();
        }

        public void EndDialogue()
        {
            if (_typingCoroutine != null) StopCoroutine(_typingCoroutine);
            _isPlaying = false; _isTyping = false; _currentDialogue = null; _currentLineIndex = 0;
            OnDialogueEnd?.Invoke();
            if (Core.GameManager.Instance?.CurrentState == Core.GameManager.GameState.Dialogue)
                Core.GameManager.Instance.ChangeState(Core.GameManager.GameState.Field);
        }

        private DialogueData FindDialogue(string id) => _allDialogues.Find(d => d.dialogueId == id);
        public void RegisterDialogue(DialogueData data) { if (!_allDialogues.Contains(data)) _allDialogues.Add(data); }
    }
}
