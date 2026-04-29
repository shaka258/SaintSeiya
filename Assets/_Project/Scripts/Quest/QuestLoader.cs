using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SaintSeiya.Quest
{
    public class QuestLoader : MonoBehaviour
    {
        [SerializeField] private TextAsset[] _questJsonFiles;

        [System.Serializable] class QuestJsonRoot { public List<QuestJsonEntry> quests; }
        [System.Serializable] class QuestJsonEntry { public string questId,questName,description,type,status,prerequisiteQuestId; public int recommendedLevel; public List<ObjJson> objectives; public RewardJson reward; }
        [System.Serializable] class ObjJson { public string objectiveId,description; public int requiredCount,currentCount; }
        [System.Serializable] class RewardJson { public int exp,gold; public string itemId; }

        void Awake() { LoadAll(); }

        public void LoadAll()
        {
            if (_questJsonFiles == null) return;
            foreach (var file in _questJsonFiles)
            {
                if (file == null) continue;
                try
                {
                    var root = JsonConvert.DeserializeObject<QuestJsonRoot>(file.text);
                    if (root?.quests == null) continue;
                    foreach (var e in root.quests)
                    {
                        var quest = new QuestData { questId=e.questId, questName=e.questName, description=e.description, type=ParseType(e.type), status=ParseStatus(e.status), prerequisiteQuestId=e.prerequisiteQuestId, objectives=new List<QuestObjective>(), reward=new QuestReward{exp=e.reward?.exp??0,gold=e.reward?.gold??0,itemId=e.reward?.itemId??""}};
                        foreach (var o in e.objectives) quest.objectives.Add(new QuestObjective{objectiveId=o.objectiveId,description=o.description,requiredCount=o.requiredCount,currentCount=o.currentCount});
                        if (quest.status == QuestStatus.Available) QuestManager.Instance?.AcceptQuest(quest);
                    }
                    Debug.Log($"[QuestLoader] {file.name}: {root.quests.Count}개 퀘스트 로드");
                }
                catch (System.Exception ex) { Debug.LogError($"[QuestLoader] {file.name}: {ex.Message}"); }
            }
        }

        QuestType ParseType(string t) => t?.ToLower() switch { "main"=>"Main", "hidden"=>"Hidden", _=>"Sub" } switch { "Main"=>QuestType.Main,"Hidden"=>QuestType.Hidden,_=>QuestType.Sub };
        QuestStatus ParseStatus(string s) => s?.ToLower() switch { "available"=>QuestStatus.Available,"active"=>QuestStatus.Active,"completed"=>QuestStatus.Completed,_=>QuestStatus.Locked };
    }
}
