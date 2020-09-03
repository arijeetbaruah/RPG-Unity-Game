using GameDevTV.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;
        [SerializeField] GameObject levelUpEffect = null;
        [SerializeField] bool shouldUseModifier = false;
        public event Action OnLevelUp;

        LazyValue<int> currentLevel;

        private void Awake()
        {
            currentLevel = new LazyValue<int>(CalculatetLevel);
        }

        private void Start()
        {
            currentLevel.ForceInit();

            Experience experience = GetComponent<Experience>();
            if (experience != null)
            {
                experience.OnXPGain += UpdateLevel;
            }
        }

        private void UpdateLevel()
        {
            int newLevel = CalculatetLevel();
            if (newLevel > currentLevel.value)
            {
                currentLevel.value = newLevel;
                LevelUpEffect();
                OnLevelUp();
            }
        }

        private void LevelUpEffect()
        {
            Instantiate(levelUpEffect, gameObject.transform);
        }

        public int GetLevel()
        {
            return currentLevel.value;
        }

        public float GetHealth()
        {
            return progression.GetHealth(characterClass, GetLevel());
        }

        public float GetExperiencePoint()
        {
            return progression.GetXP(characterClass, GetLevel());
        }

        public float GetStat(Stats stat)
        {
            return (GetBaseStats(stat) + GetAdditionalModifier(stat)) * (1 + GetPercentageModifier(stat) / 100);
        }

        private float GetBaseStats(Stats stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        private float GetAdditionalModifier(Stats stat)
        {
            if (!shouldUseModifier) return 0;
            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetAdditionalModifier(stat))
                {
                    total += modifier;
                }
            }

            return total;
        }

        private float GetPercentageModifier(Stats stat)
        {
            if (!shouldUseModifier) return 0;
            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetPercentageModifier(stat))
                {
                    total += modifier;
                }
            }

            return total;
        }

        public float GetDamageBonus()
        {
            return GetStat(Stats.Damage);
        }

        private int CalculatetLevel()
        {
            if (GetComponent<Experience>() == null) return 1;
            float currentXP = GetComponent<Experience>().GetXP();

            int maxLevel = progression.GetLevels(Stats.XPToLevelUp, characterClass);
            for (int levels = 1; levels < maxLevel; levels++)
            {
                float XPToLevelUp = progression.GetStat(Stats.XPToLevelUp, characterClass, levels);

                if (XPToLevelUp > currentXP)
                {
                    return levels;
                }
            }

            return maxLevel + 1;

        }
    }
}
