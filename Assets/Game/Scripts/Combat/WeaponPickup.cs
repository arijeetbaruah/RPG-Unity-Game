using RPG.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] WeaponConfig weapon = null;

        public CursorType GetCursorType()
        {
            return CursorType.PickUp;
        }

        public bool HandleRaycast(PlayerController controller)
        {
            if (Mouse.current.leftButton.isPressed)
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
