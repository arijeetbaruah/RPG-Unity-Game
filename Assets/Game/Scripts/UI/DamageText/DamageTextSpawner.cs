using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI.DamageText
{
    class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] DamageText damageTextPrefab;
        [SerializeField] float damageTextLifeSpan = 10f;

        public void Spawn(float value)
        {
            DamageText damageText = Instantiate<DamageText>(damageTextPrefab, transform);

            damageText.SetText(value);
            Destroy(damageText, damageTextLifeSpan);
        }
    }
}
