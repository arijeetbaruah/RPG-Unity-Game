using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using RPG.Saving;
using RPG.Stats;
using GameDevTV.Utils;
using RPG.UI.DamageText;
using RPG.UI;

namespace RPG.Resources
{
    public class Health : MonoBehaviour, ISaveable
    {
        LazyValue<float> health;
        bool isDead = false;
        
        public bool IsDead { get => isDead; }

        [SerializeField] UnityEvent onDie;
        [SerializeField] UnityEvent onTakingDamage;

        private void Awake()
        {
            GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
            if (gameObject.CompareTag("Player"))
                GetComponent<BaseStats>().OnLevelUp += RegenerateHealth;

            health = new LazyValue<float>(GetInitHealth);
        }

        private void Start()
        {
            health.ForceInit();
        }

        private float GetInitHealth()
        {
            return GetComponent<BaseStats>().GetHealth();
        }

        private void RegenerateHealth()
        {
            float percent = GetHealthPercentage();
            health.value = (percent / 100) * GetComponent<BaseStats>().GetHealth();
        }

        public float GetHealthPercentage()
        {
            float maxHealth = GetComponent<BaseStats>().GetHealth();
            return (health.value / maxHealth) * 100;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            health.value = Mathf.Max(health.value - damage, 0);
            GetComponentInChildren<DamageTextSpawner>().Spawn(damage);
            HealthBar healthBar = GetComponentInChildren<HealthBar>();
            if (healthBar != null)
                healthBar.UpdateHealthBar(GetHealthPercentage());
            
            if (health.value == 0)
            {
                GetComponent<Animator>().SetTrigger("death");
                GetComponent<RPG.Core.ActionScheduler>().CancelCurrentAction();
                isDead = true;
                onDie.Invoke();
                RewardExperience(instigator);
            }
            else
            {
                onTakingDamage.Invoke();
            }
        }

        private void RewardExperience(GameObject instigator)
        {
            Stats.Experience experience = instigator.GetComponent<Stats.Experience>();
            if (experience == null) return;
            experience.GainXP(GetComponent<BaseStats>().GetExperiencePoint());
        }

        public object CaptureState()
        {
            return health.value;
        }

        public void RestoreState(object state)
        {
            health.value = (float)state;

            if (health.value == 0)
            {
                GetComponent<Animator>().SetTrigger("death");
                isDead = true;
            }
        }
    }
}
