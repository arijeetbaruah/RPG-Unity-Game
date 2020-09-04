using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] AudioClip attackClip;

        AudioSource source;
        public void Start()
        {
            source = GetComponent<AudioSource>();
            if (source == null)
            {
                source = gameObject.AddComponent<AudioSource>();
            }
            source.clip = attackClip;
        }

        public void OnHit()
        {
            if (source.clip != null)
                source.Play();
        }
    }
}
