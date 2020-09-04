using RPG.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] WeaponConfig weapon;

        public CursorType GetCursorType()
        {
            return CursorType.PickUp;
        }

        public bool HandleRaycast(PlayerController controller)
        {
            if (Input.GetMouseButtonDown(0))
            {
                PickUp(controller.GetComponent<Figther>());
            }
            return true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                PickUp(other.GetComponent<Figther>());
            }
        }

        private void PickUp(Figther figther)
        {

            figther.EquipWeapon(weapon);
            Destroy(gameObject);
        }
    }
}
