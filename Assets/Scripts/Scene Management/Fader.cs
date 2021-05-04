using UnityEngine;
using System.Collections;

namespace SpiritStorm.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup canvasGroup;
        

        private void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void FadeOutImmediate()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.alpha = 1f;
        }

        public IEnumerator FadeOut(float time)
        {
            while (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 1f, Time.deltaTime / time);
                yield return null;
            }
        }

        public IEnumerator FadeIn(float time)
        {
            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 0f, Time.deltaTime / time);
                yield return null;
            }
        }
    }
}