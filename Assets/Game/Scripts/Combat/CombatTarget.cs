using RPG.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RPG.Combat
{
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        public CursorType GetCursorType()
        {
            return CursorType.Combat;
        }

        public bool HandleRaycast(PlayerController controller)
        {
            Figther figther = controller.GetComponent<Figther>();
            if (!figther.CanAttack(gameObject)) return false;

            if (Mouse.current.leftButton.isPressed)
            {
                figther.Attack(gameObject);
            }

            return true;
        }
    }
}
