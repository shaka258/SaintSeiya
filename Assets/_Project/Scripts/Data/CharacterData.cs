using UnityEngine;
using System.Collections.Generic;

namespace SaintSeiya.Data
{
    [CreateAssetMenu(fileName = "Char_New", menuName = "SaintSeiya/Character")]
    public class CharacterData : ScriptableObject
    {
        [Header("Identity")]
        public string characterName;
        public ConstellationType constellation;
        public ClothRank clothRank;

        [Header("Base Stats")]
        public int baseHP = 500;
        public int baseAttack = 80;
        public int baseDefense = 40;
        public int baseSpeed = 60;

        [Header("Growth Per Level")]
        public float hpGrowth = 30f;
        public float attackGrowth = 5f;
        public float defenseGrowth = 2f;

        [Header("Skills")]
        public List<SkillData> skills;

        [Header("Visuals")]
        public Sprite portrait;
        public Sprite battleSprite;
        public RuntimeAnimatorController animator;

        [Header("Voice")]
        public AudioClip[] battleVoiceLines;

        public int GetStatAtLevel(int level, int baseStat, float growth)
            => Mathf.RoundToInt(baseStat + growth * (level - 1));
    }
}
