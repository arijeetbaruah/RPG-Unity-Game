using System;
using UnityEngine;
using TMPro;
using RPG.Resources;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        Figther figther;
        
        private void Start()
        {
            figther = GameObject.FindGameObjectWithTag("Player").GetComponent<Figther>();
        }

        private void Update()
        {
            Health target = figther.GetTarget();
            if (target == null)
            {
                GetComponent<TextMeshProUGUI>().SetText(String.Format("N/A"));
                return;
            }

            float percent = target.GetHealthPercentage();

            GetComponent<TextMeshProUGUI>().SetText(String.Format("{0:0}%", percent));
        }
    }
}
