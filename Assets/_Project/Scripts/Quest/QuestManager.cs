using UnityEngine;
using System.Collections.Generic;

namespace SaintSeiya.Quest
{
    public enum QuestType { Main, Sub, Hidden }
    public enum QuestStatus { Locked, Available, Active, Completed, Failed }

    [System.Serializable]
    public class QuestObjective
    {
        public string objectiveId;
        public string description;
        public int requiredCount;
        public int currentCount;
        public bool IsComplete => currentCount >= requiredCount;
    }

    [System.Serializable]
    public class QuestReward
    {
        public int exp;
        public int gold;
        public string itemId;
    }

    [System.Serializable]
    public class QuestData
    {
        public string questId;
        public string questName;
        public string description;
        public QuestType type;
        public QuestStatus status;
        public List<QuestObjective> objectives;
        public QuestReward reward;
        public string prerequisiteQuestId;
    }

    public class QuestManager : MonoBehaviour
    {
        public static QuestManager Instance { get; private set; }

        private List<QuestData> _activeQuests = new();
        private List<QuestData> _completedQuests = new();
        private List<QuestData> _allQuests = new();

        public event System.Action<QuestData> OnQuestAccepted;
        public event System.Action<QuestData> OnQuestCompleted;
        public event System.Action<QuestData, QuestObjective> OnObjectiveUpdated;

        void Awake()
        {
            if (Instance != null) { Destroy(gameObject); return; }
            Instance = this;

            Core.EventBus.Subscribe<Core.QuestUpdatedEvent>(OnQuestUpdated);
            Core.EventBus.Subscribe<Core.QuestCompletedEvent>(OnQuestCompletedEvent);
        }

        void OnDestroy()
        {
            Core.EventBus.Unsubscribe<Core.QuestUpdatedEvent>(OnQuestUpdated);
            Core.EventBus.Unsubscribe<Core.QuestCompletedEvent>(OnQuestCompletedEvent);
        }

        public void AcceptQuest(QuestData quest)
        {
            if (quest.status != QuestStatus.Available) return;
            quest.status = QuestStatus.Active;
            _activeQuests.Add(quest);
            OnQuestAccepted?.Invoke(quest);
            Debug.Log($"[QuestManager] 퀘스트 수락: {quest.questName}");
        }

        public void UpdateObjective(string questId, string objectiveId, int amount = 1)
        {
            var quest = _activeQuests.Find(q => q.questId == questId);
            if (quest == null) return;

            var obj = quest.objectives.Find(o => o.objectiveId == objectiveId);
            if (obj == null || obj.IsComplete) return;

            obj.currentCount = Mathf.Min(obj.currentCount + amount, obj.requiredCount);
            OnObjectiveUpdated?.Invoke(quest, obj);

            if (quest.objectives.TrueForAll(o => o.IsComplete))
                CompleteQuest(questId);
        }

        public void CompleteQuest(string questId)
        {
            var quest = _activeQuests.Find(q => q.questId == questId);
            if (quest == null) return;

            quest.status = QuestStatus.Completed;
            _activeQuests.Remove(quest);
            _completedQuests.Add(quest);
            OnQuestCompleted?.Invoke(quest);
            Debug.Log($"[QuestManager] 퀘스트 완료: {quest.questName}");
        }

        private void OnQuestUpdated(Core.QuestUpdatedEvent e)
            => UpdateObjective(e.QuestId, e.ObjectiveId, e.Progress);

        private void OnQuestCompletedEvent(Core.QuestCompletedEvent e)
            => CompleteQuest(e.QuestId);
    }
}
