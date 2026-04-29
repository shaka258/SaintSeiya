using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SaintSeiya.Dialogue
{
    /// <summary>
    /// JSON 파일에서 DialogueData를 로드해서
    /// DialogueManager에 등록하는 로더
    /// Resources/Dialogues/ 폴더에 JSON 파일 배치
    /// </summary>
    public class DialogueLoader : MonoBehaviour
    {
        [Header("JSON Files")]
        [SerializeField] private TextAsset[] _dialogueJsonFiles;

        [System.Serializable]
        private class DialogueJsonRoot
        {
            public List<DialogueJsonEntry> dialogues;
        }

        [System.Serializable]
        private class DialogueJsonEntry
        {
            public string dialogueId;
            public string title;
            public List<DialogueLineJson> lines;
            public List<DialogueChoiceJson> choices;
            public string nextDialogueId;
            public string triggerQuestId;
            public bool returnToField;
        }

        [System.Serializable]
        private class DialogueLineJson
        {
            public string speaker;
            public string speakerName;
            public string text;
            public float displaySpeed = 0.05f;
            public bool waitForInput = true;
        }

        [System.Serializable]
        private class DialogueChoiceJson
        {
            public string choiceText;
            public string nextDialogueId;
            public string questIdToAccept;
        }

        void Awake()
        {
            LoadAll();
        }

        public void LoadAll()
        {
            if (_dialogueJsonFiles == null) return;

            foreach (var jsonFile in _dialogueJsonFiles)
            {
                if (jsonFile == null) continue;

                try
                {
                    var root = JsonConvert.DeserializeObject<DialogueJsonRoot>(jsonFile.text);
                    if (root?.dialogues == null) continue;

                    foreach (var entry in root.dialogues)
                    {
                        var data = ConvertToDialogueData(entry);
                        DialogueManager.Instance?.RegisterDialogue(data);
                    }

                    Debug.Log($"[DialogueLoader] {jsonFile.name}: {root.dialogues.Count}개 대화 로드");
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"[DialogueLoader] {jsonFile.name} 로드 실패: {e.Message}");
                }
            }
        }

        private DialogueData ConvertToDialogueData(DialogueJsonEntry entry)
        {
            var data = ScriptableObject.CreateInstance<DialogueData>();
            data.dialogueId     = entry.dialogueId;
            data.title          = entry.title;
            data.nextDialogueId = entry.nextDialogueId;
            data.triggerQuestId = entry.triggerQuestId;
            data.returnToField  = entry.returnToField;

            // Lines 변환
            foreach (var lineJson in entry.lines)
            {
                data.lines.Add(new DialogueLine
                {
                    speaker      = ParseSpeaker(lineJson.speaker),
                    speakerName  = lineJson.speakerName,
                    text         = lineJson.text,
                    displaySpeed = lineJson.displaySpeed,
                    waitForInput = lineJson.waitForInput
                });
            }

            // Choices 변환
            if (entry.choices != null)
            {
                foreach (var choiceJson in entry.choices)
                {
                    data.choices.Add(new DialogueChoice
                    {
                        choiceText      = choiceJson.choiceText,
                        nextDialogueId  = choiceJson.nextDialogueId,
                        questIdToAccept = choiceJson.questIdToAccept
                    });
                }
            }

            return data;
        }

        private DialogueSpeaker ParseSpeaker(string speaker)
        {
            return speaker?.ToLower() switch
            {
                "seiya"  => DialogueSpeaker.Seiya,
                "shiryu" => DialogueSpeaker.Shiryu,
                "hyoga"  => DialogueSpeaker.Hyoga,
                "shun"   => DialogueSpeaker.Shun,
                "ikki"   => DialogueSpeaker.Ikki,
                "saori"  => DialogueSpeaker.Saori,
                "npc"    => DialogueSpeaker.NPC,
                "enemy"  => DialogueSpeaker.Enemy,
                _        => DialogueSpeaker.None
            };
        }
    }
}
