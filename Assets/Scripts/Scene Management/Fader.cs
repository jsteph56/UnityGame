using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup canvasGroup;

        private void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
        
        IEnumerator FadeOutIn()
        {
            yield return FadeOut(4f);
            print("Faded Out");
            yield return FadeIn(4f);
            print("Faded In");
        }
        
        public IEnumerator FadeOut(float time)
        {
            float deltaAlpha = Time.deltaTime / time;

            while (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += deltaAlpha;
                yield return null;
            }
        }

       public IEnumerator FadeIn(float time)
        {
            float deltaAlpha = Time.deltaTime / time;

            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= deltaAlpha;
                yield return null;
            }
        }
    }
}
