﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class DestroyAfterEffect : MonoBehaviour
    {
        [SerializeField] GameObject targetToDestroy = null;

        void Update()
        {
            if (!GetComponentInChildren<ParticleSystem>().IsAlive())
            {
                Destroy(targetToDestroy == null ? gameObject : targetToDestroy);
            }
        }
    }
}
