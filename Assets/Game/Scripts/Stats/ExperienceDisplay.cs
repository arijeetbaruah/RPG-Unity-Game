using System;
using UnityEngine;
using TMPro;
using RPG.Resources;

namespace RPG.Stats
{
    public class ExperienceDisplay : MonoBehaviour
    {
        Experience figther;
        
        private void Start()
        {
            figther = GameObject.FindGameObjectWithTag("Player").GetComponent<Experience>();
        }

        private void Update()
        {
            float percent = figther.GetXP();

            GetComponent<TextMeshProUGUI>().SetText(String.Format("{0:0}", percent));
        }
    }
}
