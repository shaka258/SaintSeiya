using UnityEngine;
using System.Collections.Generic;

namespace SaintSeiya.Dialogue
{
    public enum DialogueSpeaker { None, Seiya, Shiryu, Hyoga, Shun, Ikki, Saori, NPC, Enemy }

    [System.Serializable]
    public class DialogueLine
    {
        public DialogueSpeaker speaker;
        public string speakerName;
        [TextArea(2, 5)] public string text;
        public Sprite portrait;
        public AudioClip voiceClip;
        public float displaySpeed = 0.05f;
        public bool waitForInput = true;
    }

    [System.Serializable]
    public class DialogueChoice
    {
        public string choiceText;
        public string nextDialogueId;
        public string questIdToAccept;
    }

    [CreateAssetMenu(fileName = "Dialogue_New", menuName = "SaintSeiya/Dialogue")]
    public class DialogueData : ScriptableObject
    {
        public string dialogueId;
        public string title;
        public List<DialogueLine> lines = new();
        public List<DialogueChoice> choices = new();
        public string nextDialogueId;
        public string triggerQuestId;
        public bool returnToField = true;
    }
}
