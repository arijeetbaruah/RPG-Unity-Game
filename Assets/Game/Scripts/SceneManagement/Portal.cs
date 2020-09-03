using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using RPG.Saving;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        public enum DestinationIdentifier
        {
            A, B, C, D
        };

        [SerializeField] int sceneToLoad = -1;
        [SerializeField] DestinationIdentifier destination;
        [SerializeField] float fadeOutTime = 3f;
        [SerializeField] float fadeInTime = 2f;
        [SerializeField] float fadeWaitTime = 0.5f;
        public Transform spawnPoint;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                StartCoroutine(Transition());
            }
        }

        void DisableControl()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<RPG.Control.PlayerController>().SetCursor(RPG.Control.CursorType.None);

            player.GetComponent<RPG.Control.PlayerController>().enabled = false;
        }

        void EnableControl()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<RPG.Control.PlayerController>().enabled = true;
        }

        IEnumerator Transition()
        {
            Fader fader = FindObjectOfType<Fader>();
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
            

            if (sceneToLoad < 0)
            {
                Debug.LogError("Scene to Load is not set");
                yield break;
            }

            DontDestroyOnLoad(gameObject);

            yield return fader.FadeOut(fadeOutTime);
            DisableControl();

            savingWrapper.Save();

            yield return SceneManager.LoadSceneAsync(sceneToLoad);
            DisableControl();

            savingWrapper.Load();

            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);

            savingWrapper.Save();

            yield return new WaitForSeconds(fadeWaitTime);
            yield return fader.FadeIn(fadeInTime);
            EnableControl();

            Destroy(gameObject);
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            player.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
            player.transform.position = otherPortal.spawnPoint.transform.position;
            player.transform.rotation = otherPortal.spawnPoint.transform.rotation;
            player.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
        }

        private Portal GetOtherPortal()
        {
            foreach(Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal == this) continue;
                if (portal.destination == destination) continue;

                return portal;
            }

            return null;
        }
    }
}
