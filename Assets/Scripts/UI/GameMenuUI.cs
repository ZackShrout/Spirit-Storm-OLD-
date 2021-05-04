using SpiritStorm.Control;
using SpiritStorm.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SpiritStorm.UI
{
    public class GameMenuUI : MonoBehaviour, IAction
    {
        [SerializeField] GameObject menuFirstButton, suppliesFirstButton, suppliesSelectFirstButton, suppliesSelectCharFirstButton, equipFirstButton;
        [SerializeField] GameObject equipChoiceFirstButton, equipSelectionFirstButton;
        
        [SerializeField] GameObject gameMenu, player;
        [SerializeField] GameObject[] windows;

        [SerializeField] PlayerController playerController;
        [SerializeField] Timer timer;

        [SerializeField] bool firstButtonSelected = false;

        [SerializeField] Text timePlayedText;

        void Update()
        {
            if (!firstButtonSelected)
            {
                SetFirstButtonActive(menuFirstButton);
            }

            UpdateTimePlayed();
        }

        void SetFirstButtonActive(GameObject buttonToSet)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(buttonToSet);

            firstButtonSelected = true;
        }

        void UpdateTimePlayed()
        {
            float timePlayed = timer.GetTimePlayed();
            int hours = Mathf.FloorToInt(timePlayed / 3600);
            int minutes = Mathf.FloorToInt((timePlayed % 3600) / 60);
            int seconds = Mathf.FloorToInt((timePlayed % 3600) % 60);

            timePlayedText.text = hours.ToString("D2") + ":" + minutes.ToString("D2") + ":" + seconds.ToString("D2");
        }


        void Quit()
        {
            Debug.Log("Quit to Main Menu");
        }

        public void ToggleWindow(int windowNumber)
        {
            for (int i = 0; i < windows.Length; i++)
            {
                if (i == windowNumber)
                {
                    windows[i].SetActive(!windows[i].activeInHierarchy);
                }
                else
                {
                    windows[i].SetActive(false);
                }
            }
        }

        public void OpenMenu()
        {
            if (playerController == null)
            {
                player = GameObject.FindWithTag("Player");
                playerController = player.GetComponent<PlayerController>();
                timer = FindObjectOfType<Timer>();
            }
            player.GetComponent<Collider2D>().enabled = false;
            playerController.enabled = false;
            player.GetComponent<ActionScheduler>().StartAction(this);
            firstButtonSelected = false;
        }

        public void CloseMenu()
        {
            playerController.enabled = true;
            player.GetComponent<Collider2D>().enabled = true;
            firstButtonSelected = false;
        }

        public void CancelAction()
        {
            
        }
    }
}
