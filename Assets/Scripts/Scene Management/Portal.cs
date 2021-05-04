using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using SpiritStorm.Control;

namespace SpiritStorm.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        enum DestinationIdentifier
        {
            A, B, C, D, E
        }
        
        [SerializeField] int sceneToLoad = -1;
        [SerializeField] Transform spawnPoint;
        [SerializeField] DestinationIdentifier destination;
        [SerializeField] float fadeOutTime = 1f;
        [SerializeField] float fadeInTime = 2f;
        [SerializeField] float fadeWaitTime = .5f;
        [SerializeField] bool canMove = true;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.tag == "Player")
            {

                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition()
        {
            if(sceneToLoad < 0)
            {
                Debug.LogError("Scene to load not set");
                yield break;
            }

            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<PlayerController>().enabled = false;
            
            DontDestroyOnLoad(gameObject);

            Fader fader = FindObjectOfType<Fader>();

            yield return fader.FadeOut(fadeOutTime);

            SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();
            wrapper.Save();

            yield return SceneManager.LoadSceneAsync(sceneToLoad);

            wrapper.Load();

            player = GameObject.FindWithTag("Player");
            player.GetComponent<PlayerController>().enabled = false;

            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);
            
            wrapper.Save();

            yield return new WaitForSeconds(fadeWaitTime);
            yield return fader.FadeIn(fadeInTime);

            player.GetComponent<PlayerController>().enabled = true;

            Destroy(gameObject);
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.transform.position = otherPortal.spawnPoint.position;
        }

        private Portal GetOtherPortal()
        {
            foreach(Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal == this) continue;
                if (portal.destination != destination) continue;

                return portal;
            }

            return null;
        }
    }
}
