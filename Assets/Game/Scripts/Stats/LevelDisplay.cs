using System;
using UnityEngine;
using TMPro;
using RPG.Resources;

namespace RPG.Stats
{
    public class LevelDisplay : MonoBehaviour
    {
        BaseStats figther;
        
        private void Start()
        {
            figther = GameObject.FindGameObjectWithTag("Player").GetComponent<BaseStats>();
        }

        private void Update()
        {
            float percent = figther.GetLevel();

            GetComponent<TextMeshProUGUI>().SetText(String.Format("{0:0}", percent));
        }
    }
}
