using RPG.Saving;
using System;
using UnityEngine;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] float experiencePoint = 0;

        public event Action OnXPGain;

        public float GetXP()
        {
            return experiencePoint;
        }

        public object CaptureState()
        {
            return experiencePoint;
        }

        public void GainXP(float XP)
        {
            experiencePoint += XP;

            OnXPGain();
        }

        public void RestoreState(object state)
        {
            experiencePoint = (float)state;
        }
    }
}
