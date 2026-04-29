using UnityEngine;
using System.Collections.Generic;

namespace SaintSeiya.Dialogue
{
<<<<<<< HEAD
    public enum DialogueSpeaker
    {
        None,       // 나레이션
        Seiya,      // 성시
        Shiryu,     // 시류
        Hyoga,      // 효가
        Shun,       // 순
        Ikki,       // 익키
        Saori,      // 사오리
        NPC,        // 일반 NPC
        Enemy       // 적
    }
=======
    public enum DialogueSpeaker { None, Seiya, Shiryu, Hyoga, Shun, Ikki, Saori, NPC, Enemy }
>>>>>>> 85d6086137a2cfa6b961a0149bcb69432042ea76

    [System.Serializable]
    public class DialogueLine
    {
        public DialogueSpeaker speaker;
<<<<<<< HEAD
        public string speakerName;          // 커스텀 이름 (NPC용)
        [TextArea(2, 5)]
        public string text;
        public Sprite portrait;             // 말하는 캐릭터 초상화
        public AudioClip voiceClip;         // 보이스 클립 (선택)
        public float displaySpeed = 0.05f;  // 타이핑 속도 (초/글자)
        public bool waitForInput = true;    // 입력 대기 여부
=======
        public string speakerName;
        [TextArea(2, 5)] public string text;
        public Sprite portrait;
        public AudioClip voiceClip;
        public float displaySpeed = 0.05f;
        public bool waitForInput = true;
>>>>>>> 85d6086137a2cfa6b961a0149bcb69432042ea76
    }

    [System.Serializable]
    public class DialogueChoice
    {
        public string choiceText;
<<<<<<< HEAD
        public string nextDialogueId;       // 선택 시 이동할 대화 ID
        public string questIdToAccept;      // 선택 시 수락할 퀘스트
=======
        public string nextDialogueId;
        public string questIdToAccept;
>>>>>>> 85d6086137a2cfa6b961a0149bcb69432042ea76
    }

    [CreateAssetMenu(fileName = "Dialogue_New", menuName = "SaintSeiya/Dialogue")]
    public class DialogueData : ScriptableObject
    {
<<<<<<< HEAD
        [Header("Info")]
        public string dialogueId;
        public string title;

        [Header("Lines")]
        public List<DialogueLine> lines = new();

        [Header("Choices (마지막 대사 후)")]
        public List<DialogueChoice> choices = new(); // 비어있으면 그냥 종료

        [Header("After Dialogue")]
        public string nextDialogueId;       // 자동으로 이어질 대화 ID
        public string triggerQuestId;       // 대화 종료 후 시작할 퀘스트
        public bool returnToField = true;   // 대화 후 필드 상태로 복귀
=======
        public string dialogueId;
        public string title;
        public List<DialogueLine> lines = new();
        public List<DialogueChoice> choices = new();
        public string nextDialogueId;
        public string triggerQuestId;
        public bool returnToField = true;
>>>>>>> 85d6086137a2cfa6b961a0149bcb69432042ea76
    }
}
