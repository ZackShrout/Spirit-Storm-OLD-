using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public static Shop instance;

    public bool justClosed = false;

    public GameObject shopMenu, buyMenu, sellMenu;
    public GameObject shopFirstButton, buyFirstButton, sellFirstButton, sellSelectFirstButton, buySelectFirstButton;

    public string[] itemsForSale, itemsToSell;
    public int[] numberItemsToSell;
    public ItemButton[] buyItemButtons, sellItemButtons;

    public Text goldText, numberOfItemsOwned;

    public bool firstButtonSelected = false;

    public Item selectedItem;
    public Text itemDescription, buyItemValue;
    public Text sellItemValue;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OpenShop()
    {
        shopMenu.SetActive(true);

        GameManager.instance.shopActive = true;

        goldText.text = GameManager.instance.currentGold.ToString() + " G";

        if (!firstButtonSelected)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(shopFirstButton);

            firstButtonSelected = true;
        }
    }

    public void CloseShop()
    {
        buyMenu.SetActive(false);
        sellMenu.SetActive(false);
        shopMenu.SetActive(false);

        GameManager.instance.shopActive = false;

        justClosed = true;

        firstButtonSelected = false;
    }

    public void OpenBuyMenu()
    {
        buyMenu.SetActive(true);
        sellMenu.SetActive(false);

        for (int i = 0; i < buyItemButtons.Length; i++)
        {
            buyItemButtons[i].buttonValue = i;

            if (itemsForSale[i] != "")
            {
                buyItemButtons[i].buttonImage.gameObject.SetActive(true);
                buyItemButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(itemsForSale[i]).itemSprite;
            }
            else
            {
                buyItemButtons[i].buttonImage.gameObject.SetActive(false);
            }
        }

        goldText.text = GameManager.instance.currentGold.ToString() + " G";

        firstButtonSelected = false;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(buyFirstButton);
        firstButtonSelected = true;
    }

    public void OpenSellMenu()
    {
        buyMenu.SetActive(false);
        sellMenu.SetActive(true);

        itemsToSell = new string[sellItemButtons.Length];
        numberItemsToSell = new int[sellItemButtons.Length];

        CombineSuppliesAndEquip();
        SortItemsToSell();

        for (int i = 0; i < itemsToSell.Length; i++)
        {
            sellItemButtons[i].buttonValue = i;

            if (!string.IsNullOrEmpty(itemsToSell[i]))
            {
                sellItemButtons[i].buttonImage.gameObject.SetActive(true);
                sellItemButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(itemsToSell[i]).itemSprite;
                sellItemButtons[i].amountText.text = numberItemsToSell[i].ToString();
            }
            else
            {
                sellItemButtons[i].buttonImage.gameObject.SetActive(false);
                sellItemButtons[i].amountText.text = "";
            }
        }

        goldText.text = GameManager.instance.currentGold.ToString() + " G";

        firstButtonSelected = false;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(sellFirstButton);
        firstButtonSelected = true;
    }

    private void SortItemsToSell()
    {
        bool itemAfterSpace = true;

        while (itemAfterSpace)
        {
            itemAfterSpace = false;

            for (int i = 0; i < itemsToSell.Length - 1; i++)
            {
                if (itemsToSell[i] == "")
                {
                    itemsToSell[i] = itemsToSell[i + 1];
                    itemsToSell[i + 1] = "";

                    numberItemsToSell[i] = numberItemsToSell[i + 1];
                    numberItemsToSell[i + 1] = 0;

                    if (itemsToSell[i] != "")
                    {
                        itemAfterSpace = true;
                    }
                }
            }
        }
    }

    private void CombineSuppliesAndEquip()
    {
        for (int i = 0; i < GameManager.instance.itemsHeld.Length; i++)
        {
            itemsToSell[i] = GameManager.instance.itemsHeld[i];
            numberItemsToSell[i] = GameManager.instance.numberOfItems[i];
        }
        for (int i = GameManager.instance.itemsHeld.Length; i < GameManager.instance.itemsHeld.Length + GameManager.instance.equipmentHeld.Length; i++)
        {
            itemsToSell[i] = GameManager.instance.equipmentHeld[i - GameManager.instance.itemsHeld.Length];
            numberItemsToSell[i] = GameManager.instance.numberOfEquipment[i - GameManager.instance.itemsHeld.Length];
        }
    }

    public void SelectBuyItem(Item buyItem)
    {
        selectedItem = buyItem;
        itemDescription.text = selectedItem.itemName + ": " + selectedItem.description;
        buyItemValue.text = selectedItem.value.ToString() + " G";
        numberOfItemsOwned.text = GetNumberOfItemsOwned(buyItem).ToString();

        firstButtonSelected = false;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(buySelectFirstButton);
        firstButtonSelected = true;
    }

    private int GetNumberOfItemsOwned(Item buyItem)
    {
        for(int i = 0; i < GameManager.instance.itemsHeld.Length; i++)
        {
            if(GameManager.instance.itemsHeld[i] == buyItem.name)
            {
                return GameManager.instance.numberOfItems[i];
            }
        }
        for(int i = 0; i < GameManager.instance.equipmentHeld.Length; i++)
        {
            if (GameManager.instance.equipmentHeld[i] == buyItem.name)
            {
                return GameManager.instance.numberOfEquipment[i];
            }
        }
        return 0;
    }

    public void SelectSellItem(Item sellItem)
    {
        selectedItem = sellItem;
        itemDescription.text = selectedItem.itemName + ": " + selectedItem.description;
        sellItemValue.text = Mathf.FloorToInt(selectedItem.value * .5f).ToString() + " G";

        firstButtonSelected = false;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(sellSelectFirstButton);
        firstButtonSelected = true;
    }

    public void BuyItem()
    {
        if(GameManager.instance.currentGold >= selectedItem.value)
        {
            GameManager.instance.currentGold -= selectedItem.value;

            if (selectedItem.isItem)
            {
                GameManager.instance.AddItem(selectedItem.itemName);
                OpenBuyMenu();
            }
            else
            {
                GameManager.instance.AddEquipment(selectedItem.itemName);
                OpenBuyMenu();
            }
        }
    }

    public void SellItem()
    {
        GameManager.instance.currentGold += Mathf.FloorToInt(selectedItem.value * .5f);

        if (selectedItem.isItem)
        {
            GameManager.instance.RemoveItem(selectedItem.itemName);
            OpenSellMenu();
        }
        else
        {
            GameManager.instance.RemoveEquipment(selectedItem.itemName);
            OpenSellMenu();
        }
    }
}
