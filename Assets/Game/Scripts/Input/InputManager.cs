using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace RPG.Input
{
    public class InputManager : MonoBehaviour
    {
        public Vector2 mousePosition;
        public UnityEvent OnMouseClick;

        void OnLook(InputValue value)
        {
            mousePosition = value.Get<Vector2>();
        }

        void OnAction(InputValue value)
        {
            //OnMouseClick.Invoke();
            bool click = value.Get<bool>();
            print(click);
        }
    }
}
