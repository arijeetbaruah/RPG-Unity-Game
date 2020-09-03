using RPG.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

            if (Input.GetMouseButton(0))
            {
                figther.Attack(gameObject);
            }

            return true;
        }
    }
}
