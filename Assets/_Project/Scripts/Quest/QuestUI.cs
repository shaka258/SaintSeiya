using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SaintSeiya.Quest
{
    public class QuestUI : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private KeyCode _toggleKey = KeyCode.Q;
        [SerializeField] private Transform _listContainer;
        [SerializeField] private GameObject _questEntryPrefab;
        [SerializeField] private GameObject _detailPanel;
        [SerializeField] private TextMeshProUGUI _questNameText;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private Transform _objectiveContainer;
        [SerializeField] private GameObject _objectivePrefab;
        [SerializeField] private TextMeshProUGUI _rewardText;

        private QuestData _selectedQuest;

        void Start()
        {
            _panel?.SetActive(false); _detailPanel?.SetActive(false);
            if (QuestManager.Instance != null)
            {
                QuestManager.Instance.OnQuestAccepted  += _ => RefreshList();
                QuestManager.Instance.OnQuestCompleted += _ => RefreshList();
            }
        }

        void Update() { if (Input.GetKeyDown(_toggleKey)) Toggle(); }
        public void Toggle() { bool n = !_panel.activeSelf; _panel?.SetActive(n); if (n) RefreshList(); }

        private void RefreshList()
        {
            if (_listContainer == null) return;
            foreach (Transform child in _listContainer) Destroy(child.gameObject);
            // TODO: QuestManager.GetActiveQuests() 연동
        }

        private void ShowDetail(QuestData quest)
        {
            _detailPanel?.SetActive(true);
            _questNameText?.SetText(quest.questName);
            _descriptionText?.SetText(quest.description);
            if (_objectiveContainer != null)
            {
                foreach (Transform child in _objectiveContainer) Destroy(child.gameObject);
                foreach (var obj in quest.objectives)
                {
                    var e = Instantiate(_objectivePrefab, _objectiveContainer);
                    e.GetComponentInChildren<TextMeshProUGUI>()?.SetText($"{(obj.IsComplete?"✅":"⬜")} {obj.description} ({obj.currentCount}/{obj.requiredCount})");
                }
            }
            _rewardText?.SetText($"보상: 경험치 {quest.reward.exp} / 골드 {quest.reward.gold}");
        }
    }
}
