using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemButton : MonoBehaviour, ISelectHandler
{
    public int buttonValue;
    public Image buttonImage;
    public Text amountText;
    
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
        if (Shop.instance.buyMenu.activeInHierarchy)
        {
            if (Shop.instance.itemsForSale[buttonValue] != "")
            {
                Shop.instance.SelectBuyItem(GameManager.instance.GetItemDetails(Shop.instance.itemsForSale[buttonValue]));
            }
        }

        if (Shop.instance.sellMenu.activeInHierarchy)
        {
            if (Shop.instance.itemsToSell[buttonValue] != "")
            {
                Shop.instance.SelectSellItem(GameManager.instance.GetItemDetails(Shop.instance.itemsToSell[buttonValue]));
            }
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (Shop.instance.buyMenu.activeInHierarchy)
        {
            if (Shop.instance.itemsForSale[buttonValue] != "")
            {
                Item highlightedItem = GameManager.instance.GetItemDetails(Shop.instance.itemsForSale[buttonValue]);
                Shop.instance.itemDescription.text = highlightedItem.itemName + ": " + highlightedItem.description;
            }
            else
            {
                Shop.instance.itemDescription.text = "";
            }
        }

        if (Shop.instance.sellMenu.activeInHierarchy)
        {
            if (!string.IsNullOrEmpty(Shop.instance.itemsToSell[buttonValue]))
            {
                Item highlightedItem = GameManager.instance.GetItemDetails(Shop.instance.itemsToSell[buttonValue]);
                Shop.instance.itemDescription.text = highlightedItem.itemName + ": " + highlightedItem.description;
            }
            else
            {
                Shop.instance.itemDescription.text = "";
            }
        }
    }
}
