using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "RPG/Progression", order = 1)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClasses;
        Dictionary<CharacterClass, Dictionary<Stats, float[]>> lookupTable;

        public float GetHealth(CharacterClass characterClass, int level)
        {
            BuildLookUp();
            float[] levels = lookupTable[characterClass][Stats.Health];

            return levels[Math.Min(level, levels.Length) - 1];
        }

        private void BuildLookUp()
        {
            if (lookupTable != null) return;
            lookupTable = new Dictionary<CharacterClass, Dictionary<Stats, float[]>>();

            foreach (ProgressionCharacterClass progressionClass in characterClasses)
            {
                Dictionary<Stats, float[]> statLookupTable = new Dictionary<Stats, float[]>();
                foreach (ProgressionStat levels in progressionClass.stats)
                {
                    statLookupTable[levels.stat] = levels.levels;
                }
                lookupTable[progressionClass.characterClass] = statLookupTable;
            }
        }

        public float GetStat(Stats stat, CharacterClass characterClass, int level)
        {
            BuildLookUp();
            float[] levels = lookupTable[characterClass][stat];

            if (levels.Length < level) return 0;
            return levels[level - 1];
        }

        public int GetLevels(Stats stat, CharacterClass characterClass)
        {
            BuildLookUp();

            float[] levels = lookupTable[characterClass][stat];

            return levels.Length;
        }

        public float GetXP(CharacterClass characterClass, int level)
        {
            BuildLookUp();
            float[] levels = lookupTable[characterClass][Stats.ExperienceReward];

            return levels[Math.Min(level, levels.Length) - 1];
        }

        public float GetDamage(CharacterClass characterClass, int level)
        {
            BuildLookUp();
            float[] levels = lookupTable[characterClass][Stats.Damage];

            return levels[Math.Min(level, levels.Length) - 1];
        }

        [System.Serializable]
        class ProgressionCharacterClass
        {
            public CharacterClass characterClass;
            public ProgressionStat[] stats = null;
        }

        [System.Serializable]
        class ProgressionStat
        {
            public Stats stat;
            public float[] levels;
        }
    }
}
