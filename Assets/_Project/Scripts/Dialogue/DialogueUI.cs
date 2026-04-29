using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using DG.Tweening;

namespace SaintSeiya.Dialogue
{
    /// <summary>
    /// 대화 UI 컨트롤러
    /// DialogueManager의 이벤트를 받아 화면에 표시
    /// </summary>
    public class DialogueUI : MonoBehaviour
    {
        [Header("Panel")]
        [SerializeField] private CanvasGroup _panel;
        [SerializeField] private GameObject _dialogueBox;

        [Header("Speaker")]
        [SerializeField] private TextMeshProUGUI _speakerNameText;
        [SerializeField] private Image _portraitImage;
        [SerializeField] private GameObject _portraitFrame;

        [Header("Text")]
        [SerializeField] private TextMeshProUGUI _dialogueText;
        [SerializeField] private GameObject _nextIndicator; // ▼ 계속 표시

        [Header("Choices")]
        [SerializeField] private GameObject _choicePanel;
        [SerializeField] private Transform _choiceContainer;
        [SerializeField] private GameObject _choiceButtonPrefab;

        [Header("Speaker Colors")]
        [SerializeField] private Color _seiyaColor   = new Color(0.2f, 0.6f, 1f);
        [SerializeField] private Color _shiryuColor  = new Color(0.2f, 0.8f, 0.3f);
        [SerializeField] private Color _hyogaColor   = new Color(0.5f, 0.8f, 1f);
        [SerializeField] private Color _shunColor    = new Color(0.8f, 0.4f, 0.8f);
        [SerializeField] private Color _defaultColor = Color.white;

        void Start()
        {
            _panel?.gameObject.SetActive(false);
            _choicePanel?.SetActive(false);
            _nextIndicator?.SetActive(false);

            // DialogueManager 이벤트 구독
            if (DialogueManager.Instance != null)
            {
                DialogueManager.Instance.OnLineStart    += OnLineStart;
                DialogueManager.Instance.OnTextUpdated  += OnTextUpdated;
                DialogueManager.Instance.OnLineComplete += OnLineComplete;
                DialogueManager.Instance.OnChoicesShown += OnChoicesShown;
                DialogueManager.Instance.OnDialogueEnd  += OnDialogueEnd;
            }
        }

        void OnDestroy()
        {
            if (DialogueManager.Instance != null)
            {
                DialogueManager.Instance.OnLineStart    -= OnLineStart;
                DialogueManager.Instance.OnTextUpdated  -= OnTextUpdated;
                DialogueManager.Instance.OnLineComplete -= OnLineComplete;
                DialogueManager.Instance.OnChoicesShown -= OnChoicesShown;
                DialogueManager.Instance.OnDialogueEnd  -= OnDialogueEnd;
            }
        }

        void Update()
        {
            // 스페이스 / 엔터 / E키로 다음 진행
            if (Input.GetKeyDown(KeyCode.Space) ||
                Input.GetKeyDown(KeyCode.Return) ||
                Input.GetKeyDown(KeyCode.E))
            {
                DialogueManager.Instance?.Next();
            }
        }

        // ─── 이벤트 핸들러 ──────────────────────────────────────

        private void OnLineStart(DialogueLine line)
        {
            // 패널 표시
            if (!_panel.gameObject.activeSelf)
            {
                _panel.gameObject.SetActive(true);
                _panel.alpha = 0f;
                _panel.DOFade(1f, 0.3f);
            }

            _nextIndicator?.SetActive(false);
            _choicePanel?.SetActive(false);
            _dialogueText?.SetText("");

            // 화자 이름 & 색상
            string displayName = line.speaker == DialogueSpeaker.NPC
                ? line.speakerName
                : line.speaker == DialogueSpeaker.None
                    ? ""
                    : GetSpeakerDisplayName(line.speaker);

            _speakerNameText?.SetText(displayName);
            if (_speakerNameText != null)
                _speakerNameText.color = GetSpeakerColor(line.speaker);

            // 초상화
            bool hasPortrait = line.portrait != null;
            _portraitFrame?.SetActive(hasPortrait);
            if (hasPortrait && _portraitImage != null)
                _portraitImage.sprite = line.portrait;
        }

        private void OnTextUpdated(string text)
        {
            _dialogueText?.SetText(text);
        }

        private void OnLineComplete()
        {
            _nextIndicator?.SetActive(true);
            // ▼ 아이콘 맥동
            _nextIndicator?.transform.DOPunchScale(Vector3.one * 0.2f, 0.5f, 3, 0.5f)
                .SetLoops(-1, LoopType.Restart);
        }

        private void OnChoicesShown(List<DialogueChoice> choices)
        {
            _nextIndicator?.SetActive(false);
            _choicePanel?.SetActive(true);

            // 기존 선택지 버튼 제거
            foreach (Transform child in _choiceContainer)
                Destroy(child.gameObject);

            // 선택지 버튼 생성
            for (int i = 0; i < choices.Count; i++)
            {
                int index = i; // 클로저 캡처
                var btn = Instantiate(_choiceButtonPrefab, _choiceContainer);
                var text = btn.GetComponentInChildren<TextMeshProUGUI>();
                if (text != null) text.text = choices[i].choiceText;
                btn.GetComponent<Button>()?.onClick.AddListener(() =>
                    DialogueManager.Instance?.SelectChoice(index));
            }
        }

        private void OnDialogueEnd()
        {
            _panel?.DOFade(0f, 0.3f).OnComplete(() =>
                _panel?.gameObject.SetActive(false));
            _choicePanel?.SetActive(false);
        }

        // ─── 유틸리티 ───────────────────────────────────────────

        private string GetSpeakerDisplayName(DialogueSpeaker speaker)
        {
            return speaker switch
            {
                DialogueSpeaker.Seiya  => "성시",
                DialogueSpeaker.Shiryu => "시류",
                DialogueSpeaker.Hyoga  => "효가",
                DialogueSpeaker.Shun   => "순",
                DialogueSpeaker.Ikki   => "익키",
                DialogueSpeaker.Saori  => "사오리",
                DialogueSpeaker.Enemy  => "적",
                _                      => ""
            };
        }

        private Color GetSpeakerColor(DialogueSpeaker speaker)
        {
            return speaker switch
            {
                DialogueSpeaker.Seiya  => _seiyaColor,
                DialogueSpeaker.Shiryu => _shiryuColor,
                DialogueSpeaker.Hyoga  => _hyogaColor,
                DialogueSpeaker.Shun   => _shunColor,
                _                      => _defaultColor
            };
        }
    }
}
