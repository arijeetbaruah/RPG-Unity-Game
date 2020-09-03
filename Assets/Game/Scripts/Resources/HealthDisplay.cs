using System;
using UnityEngine;
using TMPro;

namespace RPG.Resources
{
    public class HealthDisplay : MonoBehaviour
    {
        Health health;

        private void Start()
        {
            health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        }

        private void Update()
        {
            float percent = health.GetHealthPercentage();

            GetComponent<TextMeshProUGUI>().SetText(String.Format("{0:0}%", percent));
        }
    }
}
