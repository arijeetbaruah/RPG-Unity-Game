using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using RPG.Resources;
using RPG.Stats;

namespace RPG.Combat
{
    public class Figther : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {
        Health combatTarget;
        [SerializeField] float timeBetweenAttack = 1f;
        [SerializeField] Transform handTransformL = null;
        [SerializeField] Transform handTransformR = null;
        [SerializeField] Weapon defaultWeapon = null;
        [SerializeField] string defaultWeaponName = "Unarmed";
        [SerializeField] int hitDieNumber = 1;
        [SerializeField] DieType hitDieType;

        Die hitDie;
        Weapon currentWeapon = null;
        string currentWeaponName = null;

        Mover mover;
        Animator animator;
        float timeSinceLastAttack = 0f;

        void Awake()
        {
            hitDie = new Die(hitDieType, hitDieNumber);
            mover = GetComponent<Mover>();
            animator = GetComponent<Animator>();

            Weapon weapon = UnityEngine.Resources.Load<Weapon>(defaultWeaponName);
            if (currentWeapon == null)
                EquipWeapon(weapon);
        }

        void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            if (combatTarget == null) return;

            bool inRange = Vector3.Distance(transform.position, combatTarget.transform.position) < currentWeapon.GetRange();
            
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

        public void EquipWeapon(Weapon weapon)
        {
            currentWeapon = weapon;

            currentWeaponName = weapon.name;

            currentWeapon.Spawn(handTransformL, handTransformR, animator);

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
                yield return currentWeapon.GetDamage();
            }
        }

        public IEnumerable<float> GetPercentageModifier(Stats.Stats stats)
        {
            if (stats == Stats.Stats.Damage)
            {
                yield return currentWeapon.GetDamage();
            }
        }

        void Hit()
        {
            if (combatTarget == null) return;
            if (currentWeapon.IsSpell())
            {
                Shoot();
                return;
            }
            float hit = hitDie.Roll();
            float damage = currentWeapon.GetDamage() + GetComponent<Stats.BaseStats>().GetDamageBonus();

            combatTarget.TakeDamage(gameObject, damage * (hit == 20 ? damage : 1));

            if (combatTarget.IsDead)
            {
                Cancel();
            }
        }

        void Shoot()
        {
            if (combatTarget == null) return;

            currentWeapon.LaunchProjectile(gameObject, handTransformR, handTransformL, combatTarget);

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
            Weapon weapon = UnityEngine.Resources.Load<Weapon>((string)state);
            currentWeaponName = weapon.name;
            EquipWeapon(weapon);
        }
    }
}
