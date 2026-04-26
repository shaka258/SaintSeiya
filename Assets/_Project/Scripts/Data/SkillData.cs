using UnityEngine;

namespace SaintSeiya.Data
{
    public enum SkillType { Physical, Cosmos, Special, Passive }
    public enum ConstellationType
    {
        Pegasus, Dragon, Cygnus, Andromeda, Phoenix,
        Sagittarius, Gemini, Scorpio, Virgo, Aries,
        Taurus, Cancer, Capricorn, Aquarius, Pisces,
        Leo, Libra
    }
    public enum ClothRank { Bronze, Silver, Gold, God }

    [CreateAssetMenu(fileName = "Skill_New", menuName = "SaintSeiya/Skill")]
    public class SkillData : ScriptableObject
    {
        [Header("Info")]
        public string skillName;
        [TextArea] public string description;
        public SkillType skillType;

        [Header("Cost & Power")]
        public float cosmosCost = 20f;
        public float powerMultiplier = 1.5f;
        public float animationDuration = 1.2f;

        [Header("Effects")]
        public GameObject vfxPrefab;
        public AudioClip sfxClip;

        [Header("UI")]
        public Sprite icon;
    }
}
