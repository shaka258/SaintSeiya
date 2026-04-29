using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SaintSeiya.Dialogue
{
    /// <summary>
    /// 대화 진행을 관리하는 매니저
    /// DialogueData를 받아 한 줄씩 표시하고
    /// 선택지 처리 및 퀘스트 연동을 담당
    /// </summary>
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager Instance { get; private set; }

        [Header("Dialogue Database")]
        [SerializeField] private List<DialogueData> _allDialogues = new();

        // 현재 진행 상태
        private DialogueData _currentDialogue;
        private int _currentLineIndex;
        private bool _isPlaying;
        private bool _isTyping;
        private Coroutine _typingCoroutine;

        // 이벤트
        public event System.Action<DialogueLine> OnLineStart;    // 줄 시작
        public event System.Action<string> OnTextUpdated;        // 타이핑 중 텍스트 업데이트
        public event System.Action OnLineComplete;               // 줄 완료 (타이핑 끝)
        public event System.Action<List<DialogueChoice>> OnChoicesShown; // 선택지 표시
        public event System.Action OnDialogueEnd;               // 대화 전체 종료

        void Awake()
        {
            if (Instance != null) { Destroy(gameObject); return; }
            Instance = this;
            Core.EventBus.Subscribe<Core.DialogueStartEvent>(OnDialogueStartEvent);
        }

        void OnDestroy()
        {
            Core.EventBus.Unsubscribe<Core.DialogueStartEvent>(OnDialogueStartEvent);
        }

        // ─── 외부 진입점 ────────────────────────────────────────

        private void OnDialogueStartEvent(Core.DialogueStartEvent e)
        {
            var data = FindDialogue(e.DialogueId);
            if (data != null) StartDialogue(data);
            else Debug.LogWarning($"[DialogueManager] 대화 없음: {e.DialogueId}");
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

        // ─── 줄 표시 ────────────────────────────────────────────

        private void ShowCurrentLine()
        {
            if (_currentLineIndex >= _currentDialogue.lines.Count)
            {
                OnAllLinesFinished();
                return;
            }

            var line = _currentDialogue.lines[_currentLineIndex];
            OnLineStart?.Invoke(line);

            if (_typingCoroutine != null) StopCoroutine(_typingCoroutine);
            _typingCoroutine = StartCoroutine(TypeLine(line));
        }

        private IEnumerator TypeLine(DialogueLine line)
        {
            _isTyping = true;
            string fullText = line.text;
            string current = "";

            // 보이스 재생
            if (line.voiceClip != null)
                Core.AudioManager.Instance?.PlaySFX(line.voiceClip);

            // 한 글자씩 표시
            foreach (char c in fullText)
            {
                current += c;
                OnTextUpdated?.Invoke(current);
                yield return new WaitForSeconds(line.displaySpeed);
            }

            _isTyping = false;
            OnLineComplete?.Invoke();

            // 자동 진행 (waitForInput = false)
            if (!line.waitForInput)
            {
                yield return new WaitForSeconds(1f);
                Next();
            }
        }

        // ─── 입력 처리 ──────────────────────────────────────────

        /// <summary>플레이어가 확인 버튼을 눌렀을 때 호출</summary>
        public void Next()
        {
            if (!_isPlaying) return;

            // 타이핑 중이면 전체 텍스트 즉시 표시
            if (_isTyping)
            {
                if (_typingCoroutine != null) StopCoroutine(_typingCoroutine);
                _isTyping = false;
                var line = _currentDialogue.lines[_currentLineIndex];
                OnTextUpdated?.Invoke(line.text);
                OnLineComplete?.Invoke();
                return;
            }

            _currentLineIndex++;
            ShowCurrentLine();
        }

        /// <summary>선택지 선택 시 호출</summary>
        public void SelectChoice(int choiceIndex)
        {
            if (_currentDialogue == null) return;
            if (choiceIndex >= _currentDialogue.choices.Count) return;

            var choice = _currentDialogue.choices[choiceIndex];

            // 퀘스트 수락
            if (!string.IsNullOrEmpty(choice.questIdToAccept))
                Core.EventBus.Publish(new Core.QuestUpdatedEvent { QuestId = choice.questIdToAccept });

            // 다음 대화로 이동
            if (!string.IsNullOrEmpty(choice.nextDialogueId))
            {
                var next = FindDialogue(choice.nextDialogueId);
                if (next != null) { StartDialogue(next); return; }
            }

            EndDialogue();
        }

        // ─── 종료 ───────────────────────────────────────────────

        private void OnAllLinesFinished()
        {
            // 선택지가 있으면 표시
            if (_currentDialogue.choices.Count > 0)
            {
                OnChoicesShown?.Invoke(_currentDialogue.choices);
                return;
            }

            // 이어질 대화가 있으면 자동 연결
            if (!string.IsNullOrEmpty(_currentDialogue.nextDialogueId))
            {
                var next = FindDialogue(_currentDialogue.nextDialogueId);
                if (next != null) { StartDialogue(next); return; }
            }

            // 퀘스트 트리거
            if (!string.IsNullOrEmpty(_currentDialogue.triggerQuestId))
                Core.EventBus.Publish(new Core.QuestUpdatedEvent
                    { QuestId = _currentDialogue.triggerQuestId });

            EndDialogue();
        }

        public void EndDialogue()
        {
            if (_typingCoroutine != null) StopCoroutine(_typingCoroutine);
            _isPlaying = false;
            _isTyping = false;
            _currentDialogue = null;
            _currentLineIndex = 0;

            OnDialogueEnd?.Invoke();

            if (Core.GameManager.Instance?.CurrentState == Core.GameManager.GameState.Dialogue)
                Core.GameManager.Instance.ChangeState(Core.GameManager.GameState.Field);
        }

        // ─── 유틸리티 ───────────────────────────────────────────

        private DialogueData FindDialogue(string id)
            => _allDialogues.Find(d => d.dialogueId == id);

        public void RegisterDialogue(DialogueData data)
        {
            if (!_allDialogues.Contains(data)) _allDialogues.Add(data);
        }

        public bool IsPlaying => _isPlaying;
    }
}
