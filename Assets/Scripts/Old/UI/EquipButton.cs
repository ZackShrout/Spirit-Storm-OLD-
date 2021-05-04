using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EquipButton : MonoBehaviour, ISelectHandler
{
    public Image buttonImage;
    public Text amountText;
    public int buttonValue;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Press()
    {
        if (GameMenu.instance.equipToShow[buttonValue] != "")
        {
            GameMenu.instance.SelectEquip(GameManager.instance.GetItemDetails(GameMenu.instance.equipToShow[buttonValue]));
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (GameMenu.instance.equipToShow[buttonValue] != "")
        {
            Item highlightedItem = GameManager.instance.GetItemDetails(GameMenu.instance.equipToShow[buttonValue]);
            GameMenu.instance.menuFeedText.text = highlightedItem.itemName + ": " + highlightedItem.description;
        }
        else
        {
            GameMenu.instance.menuFeedText.text = "";
        }
    }
}
