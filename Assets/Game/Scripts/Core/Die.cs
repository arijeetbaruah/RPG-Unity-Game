using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class Die : MonoBehaviour
    {
        [SerializeField] DieType dieType;
        [SerializeField] int count = 1;

        public Die(DieType die, int count = 1)
        {
            dieType = die;
            this.count = count;
        }

        public float Roll()
        {
            float sum = 0;

            for (int i = 0;i < count; i++)
            {
                sum += RollOnce();
            }

            return sum;
        }

        public float RollOnce()
        {
            switch (dieType)
            {
                case DieType.d4:
                    return Random.Range(1, 4);
                case DieType.d6:
                    return Random.Range(1, 6);
                case DieType.d8:
                    return Random.Range(1, 8);
                case DieType.d10:
                    return Random.Range(1, 10);
                case DieType.d12:
                    return Random.Range(1, 12);
                case DieType.d20:
                    return Random.Range(1, 20);
                case DieType.d100:
                    return Random.Range(1, 100);
            }
            return 0;
        }
    }
}
