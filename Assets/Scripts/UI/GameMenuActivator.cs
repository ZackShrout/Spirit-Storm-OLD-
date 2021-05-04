using UnityEngine;

namespace SpiritStorm.UI
{
    public class GameMenuActivator : MonoBehaviour
    {
        [SerializeField] GameObject gameMenu, dialogUI;

        void Update()
        {
            if (Input.GetButtonDown("Cancel") && !dialogUI.activeInHierarchy)
            {
                if (!gameMenu.activeInHierarchy)
                {
                    gameMenu.SetActive(true);
                    gameMenu.GetComponent<GameMenuUI>().OpenMenu();
                }
                else
                {
                    gameMenu.GetComponent<GameMenuUI>().CloseMenu();
                    gameMenu.SetActive(false);
                }
                
            }
        }
    }
}
