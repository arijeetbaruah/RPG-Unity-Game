using UnityEngine;
using UnityEngine.Playables;
using RPG.Saving;

namespace RPG.Cinematic
{
    public class CinematicTrigger : MonoBehaviour, ISaveable
    {
        bool hadTriggered = false;

        public object CaptureState()
        {
            return hadTriggered;
        }

        public void RestoreState(object state)
        {
            hadTriggered = (bool)state;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player") && !hadTriggered)
            {
                GetComponent<PlayableDirector>().Play();
                hadTriggered = true;
            }
        }
    }
}