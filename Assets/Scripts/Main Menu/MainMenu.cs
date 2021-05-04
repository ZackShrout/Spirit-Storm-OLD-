using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace SpiritStorm.MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] bool firstButtonSelected = false;
        [SerializeField] GameObject menuFirstButton;
        [SerializeField] string areaToLoad;

        void Update()
        {
            if (!firstButtonSelected)
            {
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(menuFirstButton);

                firstButtonSelected = true;
            }
        }

        public void NewGame()
        {
            //AudioManager.instance.StopMusic(0);
            SceneManager.LoadScene(areaToLoad);
        }

        public void LoadGame()
        {
            Debug.Log("Load game.");
        }

        public void NewGamePlus()
        {
            Debug.Log("New game plus.");
        }

        public void QuitGame()
        {
            print("Exiting the game");

            Application.Quit();
        }
    }
}
