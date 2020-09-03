using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup fader;
        Coroutine currentlyActiveFade;

        void Awake()
        {
            fader = GetComponent<CanvasGroup>();
        }

        IEnumerator FadeInOut()
        {
            yield return FadeOut(3f);
            print("faded out");
            yield return FadeIn(2f);
            print("faded in");
        }

        public void FadeOutImmediate()
        {
            fader.alpha = 1;
        }

        public IEnumerator FadeOut(float time)
        {
            if (currentlyActiveFade != null)
                StopCoroutine(currentlyActiveFade);
            currentlyActiveFade = StartCoroutine(FadeOutRoutine(time));
            yield return currentlyActiveFade;
        }

        public IEnumerator FadeOutRoutine(float time)
        {
            while (fader.alpha < 1)
            {
                fader.alpha += Time.deltaTime / time;
                yield return null;
            }
        }

        public IEnumerator FadeIn(float time) {
            if (currentlyActiveFade != null)
                StopCoroutine(currentlyActiveFade);
            currentlyActiveFade = StartCoroutine(FadeInRoutine(time));
            yield return currentlyActiveFade;
        }

        public IEnumerator FadeInRoutine(float time)
        {
            while (fader.alpha > 0)
            {
                fader.alpha -= Time.deltaTime / time;
                yield return null;
            }

        }
    }
}
