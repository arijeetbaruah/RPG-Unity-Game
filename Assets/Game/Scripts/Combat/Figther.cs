using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using RPG.Resources;
using RPG.Stats;
using GameDevTV.Utils;

namespace RPG.Combat
{
    public class Figther : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {
        Health combatTarget;
        [SerializeField] float timeBetweenAttack = 1f;
        [SerializeField] Transform handTransformL = null;
        [SerializeField] Transform handTransformR = null;
        [SerializeField] WeaponConfig defaultWeapon = null;
        [SerializeField] string defaultWeaponName = "Unarmed";
        [SerializeField] int hitDieNumber = 1;
        [SerializeField] DieType hitDieType;

        Die hitDie;
        WeaponConfig currentWeaponConfig = null;
        LazyValue<Weapon> currentWeapon = null;
        string currentWeaponName = null;

        Mover mover;
        Animator animator;
        float timeSinceLastAttack = 0f;

        void Awake()
        {
            hitDie = new Die(hitDieType, hitDieNumber);
            mover = GetComponent<Mover>();
            animator = GetComponent<Animator>();

            currentWeapon = new LazyValue<Weapon>(SetUpDefaultWeapon);

            WeaponConfig weapon = UnityEngine.Resources.Load<WeaponConfig>(defaultWeaponName);
            if (currentWeaponConfig == null)
                EquipWeapon(weapon);
        }

        void Start()
        {
            currentWeapon.ForceInit();
        }

        Weapon SetUpDefaultWeapon()
        {
            return EquipWeapon(currentWeaponConfig);
        }

        void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            if (combatTarget == null) return;

            bool inRange = Vector3.Distance(transform.position, combatTarget.transform.position) < currentWeaponConfig.GetRange();
            
            if (!inRange)
            {
                mover.MoveToPosition(combatTarget.transform.position, 1);
            }
            else
            {
                mover.Cancel();
                AttackBehaviour();
            }
        }

        public Die GetHitDie()
        {
            return hitDie;
        }

        public Weapon EquipWeapon(WeaponConfig weapon)
        {
            currentWeaponConfig = weapon;

            currentWeaponName = weapon.name;

            currentWeapon.value = currentWeaponConfig.Spawn(handTransformL, handTransformR, animator);
            return currentWeapon.value;
        }

        public Health GetTarget()
        {
            if (combatTarget == null)
            {
                return null;
            }
            return combatTarget.GetComponent<Health>();
        }

        private void AttackBehaviour()
        {
            transform.LookAt(combatTarget.transform);
            if (timeSinceLastAttack >= timeBetweenAttack)
            {
                animator.ResetTrigger("stopAttack");
                animator.SetTrigger("attack");
                timeSinceLastAttack = 0f;
                if (currentWeapon.value != null)
                    currentWeapon.value.OnHit();
            }
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) return false;
            Health targetToKill = combatTarget.GetComponent<Health>();
            return targetToKill != null && !targetToKill.IsDead;
        }

        public void Attack(GameObject target)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            combatTarget = target.GetComponent<Health>();
        }

        public void Cancel()
        {
            combatTarget = null;
            animator.ResetTrigger("attack");
            animator.SetTrigger("stopAttack");
        }

        public IEnumerable<float> GetAdditionalModifier(Stats.Stats stats)
        {
            if (stats == Stats.Stats.Damage)
            {
                yield return currentWeaponConfig.GetDamage();
            }
        }

        public IEnumerable<float> GetPercentageModifier(Stats.Stats stats)
        {
            if (stats == Stats.Stats.Damage)
            {
                yield return currentWeaponConfig.GetDamage();
            }
        }

        void Hit()
        {
            if (combatTarget == null) return;
            if (currentWeaponConfig.IsSpell())
            {
                Shoot();
                return;
            }
            float hit = hitDie.Roll();
            float damage = currentWeaponConfig.GetDamage() + GetComponent<Stats.BaseStats>().GetDamageBonus();

            combatTarget.TakeDamage(gameObject, damage * (hit == 20 ? damage : 1));

            if (combatTarget.IsDead)
            {
                Cancel();
            }
        }

        void Shoot()
        {
            if (combatTarget == null) return;

            currentWeaponConfig.LaunchProjectile(gameObject, handTransformR, handTransformL, combatTarget);

            if (combatTarget.IsDead)
            {
                Cancel();
            }
        }

        public object CaptureState()
        {
            return currentWeaponName;
        }

        public void RestoreState(object state)
        {
            WeaponConfig weapon = UnityEngine.Resources.Load<WeaponConfig>((string)state);
            if (weapon == null) return;
            currentWeaponName = weapon.name;
            EquipWeapon(weapon);
        }
    }
}
