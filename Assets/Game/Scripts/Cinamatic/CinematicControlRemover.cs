using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematic
{
    public class CinematicControlRemover : MonoBehaviour
    {
        void Start()
        {
            GetComponent<PlayableDirector>().stopped += EnableControl;
            GetComponent<PlayableDirector>().played += DisableControl;
        }

        void DisableControl(PlayableDirector pd)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<RPG.Core.ActionScheduler>().CancelCurrentAction();
            player.GetComponent<RPG.Movement.Mover>().Cancel();
            player.GetComponent<RPG.Control.PlayerController>().SetCursor(RPG.Control.CursorType.None);

            player.GetComponent<RPG.Control.PlayerController>().enabled = false;
        }

        void EnableControl(PlayableDirector pd)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<RPG.Control.PlayerController>().enabled = true;
        }
    }
}
