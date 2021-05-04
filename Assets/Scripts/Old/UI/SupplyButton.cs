using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SupplyButton : MonoBehaviour, ISelectHandler
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
        if(GameManager.instance.itemsHeld[buttonValue] != "")
        {
            GameMenu.instance.SelectItem(GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[buttonValue]));
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (GameManager.instance.itemsHeld[buttonValue] != "")
        {
            Item highlightedItem = GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[buttonValue]);
            GameMenu.instance.menuFeedText.text = highlightedItem.itemName + ": " + highlightedItem.description;
        }
        else
        {
            GameMenu.instance.menuFeedText.text = "";
        }
    }
}
