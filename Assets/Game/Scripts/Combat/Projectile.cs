using RPG.Core;
using RPG.Resources;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] Health target = null;
        [SerializeField] float speed = 1;
        [SerializeField] bool homing = false;
        [SerializeField] GameObject hitEffect = null;
        [SerializeField] float maxLifetime = 100;

        float projectileDamage = 10f;
        GameObject instigator;
        Die die;

        private void Start()
        {
            transform.LookAt(GetAimPotion());
        }

        private void Update()
        {
            if (target == null) return;
            if (homing && !target.IsDead) transform.LookAt(GetAimPotion());
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget(GameObject instigator, Health target, float damage, Die die)
        {
            this.target = target;
            this.instigator = instigator;
            projectileDamage = damage;
            this.die = die;

            Destroy(gameObject, maxLifetime);
        }

        private Vector3 GetAimPotion()
        {
            CapsuleCollider targetCollider = target.GetComponent<CapsuleCollider>();
            if (targetCollider == null)
                return target.transform.position;
            return target.transform.position + Vector3.up * targetCollider.height / 2;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() != target) return;
            if (target.IsDead) return;

            Figther figther = GetComponent<Figther>();
            float hit = -1;
            if (figther != null)
            {
                hit = figther.GetHitDie().Roll();
            }

            float damage = projectileDamage + die.Roll();

            target.TakeDamage(instigator, damage * (hit == 20 ? damage : 1));

            if (hitEffect)
                Instantiate(hitEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
