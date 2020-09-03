using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RPG.UI.DamageText
{
    public class DamageText : MonoBehaviour
    {
        public void SetText(float value)
        {
            GetComponentInChildren<TextMeshProUGUI>().SetText(value.ToString());
        }
    }
}
