using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class HealthBar : MonoBehaviour
    {
        Slider slider;

        void Start()
        {
            slider = GetComponentInChildren<Slider>();
        }

        public void UpdateHealthBar(float value)
        {
            slider.value = value;

            if (value == 0)
            {
                Destroy(gameObject, 3);
            }
        }
    }
}
