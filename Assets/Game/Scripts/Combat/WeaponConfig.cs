using UnityEngine;

using RPG.Resources;
using RPG.Core;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "RPG/Weapon", order = 0)]
    public class WeaponConfig : ScriptableObject
    {
        [SerializeField] AnimatorOverrideController weaponOverride = null;
        [SerializeField] Weapon weaponPrefab = null;
        [SerializeField] float WeaponRange = 2f;
        [SerializeField] float weaponPercentageBonus = 1f;
        [SerializeField] float weaponDamage = 0f;
        [SerializeField] bool isRightHanded = true;
        [SerializeField] bool isSpell = false;
        [SerializeField] Projectile projectile = null;
        [SerializeField] int damageDieNumber = 1;
        [SerializeField] DieType damageDie;

        const string weaponName = "Weapon";
        Die dieRoller;

        public bool IsRightHanded()
        {
            return isRightHanded;
        }

        public Weapon Spawn(Transform handL, Transform handR, Animator animator)
        {
            DestroyOldWeapon(handL, handR);
            Weapon weapon;

            if (weaponPrefab == null) return null;
            if (isRightHanded)
                weapon = Instantiate(weaponPrefab, handR);
            else
                weapon = Instantiate(weaponPrefab, handL);

            weapon.name = weaponName;
            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;

            if (weaponOverride != null)
            {
                animator.runtimeAnimatorController = weaponOverride;
            }
            else if (overrideController != null)
            {
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }

            return weapon;
        }

        void DestroyOldWeapon(Transform handL, Transform handR)
        {
            Transform oldWeapon = handR.Find(weaponName);

            if (oldWeapon == null) oldWeapon = handL.Find(weaponName);
            if (oldWeapon == null) return;

            oldWeapon.name = "DESTROYING";
            Destroy(oldWeapon.gameObject);
        }

        public float GetPercentageBonus()
        {
            return weaponPercentageBonus;
        }

        public float GetDamage()
        {
            if (dieRoller == null)
                dieRoller = new Die(damageDie, damageDieNumber);

            return dieRoller.Roll() + weaponDamage;
        }

        public float GetRange()
        {
            return WeaponRange;
        }

        public bool IsSpell()
        {
            return isSpell;
        }

        public Projectile GetProjectile()
        {
            return projectile;
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }

        public void LaunchProjectile(GameObject instigator, Transform rightHand, Transform leftHand, Health target)
        {
            Transform parent;
            if (isRightHanded)
                parent = rightHand;
            else
                parent = leftHand;

            Projectile projectileInstance = Instantiate<Projectile>(projectile, parent.position, Quaternion.identity);
            projectileInstance.SetTarget(instigator, target, GetDamage(), dieRoller);
        }
    }
}
