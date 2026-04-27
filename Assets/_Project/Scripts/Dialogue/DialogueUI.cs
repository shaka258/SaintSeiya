using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

namespace SaintSeiya.Dialogue
{
    public class DialogueUI : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private TextMeshProUGUI _speakerNameText;
        [SerializeField] private Image _portraitImage;
        [SerializeField] private TextMeshProUGUI _dialogueText;
        [SerializeField] private GameObject _nextIndicator;
        [SerializeField] private GameObject _choicePanel;
        [SerializeField] private Transform _choiceContainer;
        [SerializeField] private GameObject _choiceButtonPrefab;

        void Start()
        {
            _panel?.SetActive(false);
            _choicePanel?.SetActive(false);
            _nextIndicator?.SetActive(false);
            if (DialogueManager.Instance == null) return;
            DialogueManager.Instance.OnLineStart    += OnLineStart;
            DialogueManager.Instance.OnTextUpdated  += t => _dialogueText?.SetText(t);
            DialogueManager.Instance.OnLineComplete += () => _nextIndicator?.SetActive(true);
            DialogueManager.Instance.OnChoicesShown += OnChoicesShown;
            DialogueManager.Instance.OnDialogueEnd  += () => _panel?.SetActive(false);
        }

        void OnDestroy()
        {
            if (DialogueManager.Instance == null) return;
            DialogueManager.Instance.OnLineStart    -= OnLineStart;
            DialogueManager.Instance.OnDialogueEnd  -= () => _panel?.SetActive(false);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.E))
                DialogueManager.Instance?.Next();
        }

        private void OnLineStart(DialogueLine line)
        {
            _panel?.SetActive(true);
            _nextIndicator?.SetActive(false);
            _choicePanel?.SetActive(false);
            _dialogueText?.SetText("");
            string name = line.speaker == DialogueSpeaker.NPC ? line.speakerName
                        : line.speaker == DialogueSpeaker.None ? ""
                        : line.speaker.ToString();
            _speakerNameText?.SetText(name);
            if (_portraitImage != null && line.portrait != null) _portraitImage.sprite = line.portrait;
        }

        private void OnChoicesShown(List<DialogueChoice> choices)
        {
            _nextIndicator?.SetActive(false);
            _choicePanel?.SetActive(true);
            foreach (Transform child in _choiceContainer) Destroy(child.gameObject);
            for (int i = 0; i < choices.Count; i++)
            {
                int idx = i;
                var btn = Instantiate(_choiceButtonPrefab, _choiceContainer);
                btn.GetComponentInChildren<TextMeshProUGUI>()?.SetText(choices[i].choiceText);
                btn.GetComponent<Button>()?.onClick.AddListener(() => DialogueManager.Instance?.SelectChoice(idx));
            }
        }
    }
}
