using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.LowLevel;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public static GameMenu instance;

    [Header("Main Menu Variables")]
    public GameObject theMenu;

    public GameObject menuFirstButton, suppliesFirstButton, suppliesSelectFirstButton, suppliesSelectCharFirstButton, equipFirstButton, equipChoiceFirstButton, equipSelectionFirstButton;

    public GameObject[] windows;

    public CharStats[] playerStats;

    public Text[] nameText, hpText, focusText, levelText, xpText, statusAilment;
    public Slider[] xpSlider;
    public Image[] charImage;
    public GameObject[] charStatHolder;

    public Text menuFeedText;

    public Text timePlayedText;

    public bool firstButtonSelected = false;

    public Text goldText;

    [Header("Supplies Window Variables")]
    public SupplyButton[] supplyButtons;

    public string selectedItem;
    public Item activeItem;
    public Text itemDescription;

    public Text[] suppliesNameText, suppliesHPText, suppliesFocusText, suppliesStrengthText, suppliesSpeedText, suppliesAgilityText, suppliesDefenseText, suppliesCritText;
    public Image[] suppliesCharImage;
    public GameObject[] suppliesCharStatHolder;

    [Header("Equip Window Variables")]
    public EquipButton[] equipButtons;

    public string[] equipToShow;
    public int[] numberOfEquipToShow;

    public int charToEquip, equipSlot;

    public string selectedEquipment;
    public Text equipDescription;

    public Text[] equipNameText, equipAttackPowerText, equipSpellPowerText, equipDefenseText, equipMagicDefenseText, equipSpeedText, equipCritText, equipWeap1Text, equipWeap2Text, equipArmorText, equipArt1Text, equipArt2Text;
    public Image[] equipmentCharImage, equipWeap1Image, equipWeap2Image, equipArmorImage, equipArt1Image, equipArt2Image;
    public Sprite defaultWeap1Image, defaultWeap2Image, defaultArmorImage, defaultArtifact1Image, defaultArtifact2Image;
    public GameObject[] equipmentCharStatHolder;

    public GameObject equipChoice;

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
        if (!GameManager.instance.dialogActive && !GameManager.instance.shopActive && Input.GetButtonUp("Cancel"))
        {
            if (theMenu.activeInHierarchy)
            {
                CloseMenu();
            }
            else
            {
                theMenu.SetActive(true);

                if (!firstButtonSelected)
                {
                    EventSystem.current.SetSelectedGameObject(null);
                    EventSystem.current.SetSelectedGameObject(menuFirstButton);

                    firstButtonSelected = true;
                }

                UpdateMainStats();
                GameManager.instance.gameMenuOpen = true;
            }
        }

        if (theMenu.activeInHierarchy)
        {
            UpdateTimePlayed();
        }
    }

    public void UpdateTimePlayed()
    {
        float timePlayed = GameManager.instance.timePlayed;
        int hours = Mathf.FloorToInt(timePlayed / 3600);
        int minutes = Mathf.FloorToInt((timePlayed % 3600) / 60);
        int seconds = Mathf.FloorToInt((timePlayed % 3600) % 60);

        timePlayedText.text = hours.ToString("D2") + ":" + minutes.ToString("D2") + ":" + seconds.ToString("D2");
    }

    public void UpdateMainStats()
    {
        playerStats = GameManager.instance.playerStats;

        for (int i = 0; i < playerStats.Length; i++)
        {
            if (playerStats[i].gameObject.activeInHierarchy)
            {
                charStatHolder[i].SetActive(true);

                nameText[i].text = playerStats[i].charName;
                hpText[i].text = "HP: " + playerStats[i].currentHP + "/" + playerStats[i].maxHP;
                focusText[i].text = "Focus: " + playerStats[i].currentFocus + "/" + playerStats[i].maxFocus;
                levelText[i].text = "Level: " + playerStats[i].playerLevel;
                xpText[i].text = "" + playerStats[i].currentEXP + "/" + playerStats[i].expToNextLevel[playerStats[i].playerLevel];
                xpSlider[i].maxValue = playerStats[i].expToNextLevel[playerStats[i].playerLevel];
                xpSlider[i].value = playerStats[i].currentEXP;
                charImage[i].sprite = playerStats[i].charImage;
                statusAilment[i].text = "";
            }
            else
            {
                charStatHolder[i].SetActive(false);
            }
        }

        goldText.text = GameManager.instance.currentGold.ToString() + " G";
    }

    public void ToggleWindow(int windowNumber)
    { 
        for(int i = 0; i < windows.Length; i++)
        {
            if(i == windowNumber)
            {
                windows[i].SetActive(!windows[i].activeInHierarchy);
            }
            else
            {
                windows[i].SetActive(false);
            }
        }

        ClearEquipToShow();
        equipChoice.SetActive(false);
    }

    public void CloseMenu()
    {
        for (int i = 0; i < windows.Length; i++)
        {
            windows[i].SetActive(false);
        }

        theMenu.SetActive(false);
        GameManager.instance.gameMenuOpen = false;

        ClearEquipToShow();
        equipChoice.SetActive(false);

        firstButtonSelected = false;
    }

    public void ShowSupplies()
    {
        GameManager.instance.SortItems();
        UpdateSuppliesStats();
        
        for (int i = 0; i < supplyButtons.Length; i++)
        {
            supplyButtons[i].buttonValue = i;

            if(GameManager.instance.itemsHeld[i] != "")
            {
                supplyButtons[i].buttonImage.gameObject.SetActive(true);
                supplyButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[i]).itemSprite;
                supplyButtons[i].amountText.text = GameManager.instance.numberOfItems[i].ToString();
            }
            else
            {
                supplyButtons[i].buttonImage.gameObject.SetActive(false);
                supplyButtons[i].amountText.text = "";
            }
        }

        if (windows[0].activeInHierarchy)
        {
            firstButtonSelected = false;
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(suppliesFirstButton);
            firstButtonSelected = true;
        }
    }

    public void ShowEquipment()
    {
        GameManager.instance.SortEquip();
        UpdateEquipStats();

        for (int i = 0; i < equipButtons.Length; i++)
        {
            equipButtons[i].buttonValue = i;

            if (GameManager.instance.equipmentHeld[i] != "")
            {
                equipButtons[i].buttonImage.gameObject.SetActive(true);
                equipButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(GameManager.instance.equipmentHeld[i]).itemSprite;
                equipButtons[i].amountText.text = GameManager.instance.numberOfEquipment[i].ToString();
            }
            else
            {
                equipButtons[i].buttonImage.gameObject.SetActive(false);
                equipButtons[i].amountText.text = "";
            }
        }

        if (windows[1].activeInHierarchy)
        {
            firstButtonSelected = false;
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(equipFirstButton);
            firstButtonSelected = true;
        }
    }

    public void SelectItem(Item newItem)
    {
        activeItem = newItem;

        itemDescription.text = activeItem.itemName + ": " + activeItem.description;

        firstButtonSelected = false;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(suppliesSelectFirstButton);
        firstButtonSelected = true;
    }

    public void SelectEquip(Item newEquip)
    {
        activeItem = newEquip;

        equipDescription.text = activeItem.itemName + ": " + activeItem.description;

        firstButtonSelected = false;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(equipSelectionFirstButton);
        firstButtonSelected = true;
    }

    public void DiscardItem()
    {
        if(activeItem != null)
        {
            GameManager.instance.RemoveItem(activeItem.itemName);
        }
    }

    public void DiscardEquip()
    {
        if (activeItem != null)
        {
            GameManager.instance.RemoveItem(activeItem.itemName);
        }

        CloseEquipChoice();
    }

    public void SuppliesSelectChar()
    {
        firstButtonSelected = false;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(suppliesSelectCharFirstButton);
        firstButtonSelected = true;
    }

    public void UpdateSuppliesStats()
    {
        playerStats = GameManager.instance.playerStats;

        for (int i = 0; i < playerStats.Length; i++)
        {
            if (playerStats[i].gameObject.activeInHierarchy)
            {
                suppliesCharStatHolder[i].SetActive(true);

                suppliesNameText[i].text = playerStats[i].charName;
                suppliesHPText[i].text = "" + playerStats[i].currentHP + "/" + playerStats[i].maxHP;
                suppliesFocusText[i].text = "" + playerStats[i].currentFocus + "/" + playerStats[i].maxFocus;
                suppliesStrengthText[i].text = playerStats[i].currentStrength.ToString();
                suppliesSpeedText[i].text = playerStats[i].currentSpeed.ToString();
                suppliesAgilityText[i].text = playerStats[i].currentAgility.ToString();
                suppliesDefenseText[i].text = playerStats[i].currentDefense.ToString();
                suppliesCritText[i].text = playerStats[i].currentCrit.ToString();
                suppliesCharImage[i].sprite = playerStats[i].charImage;
            }
            else
            {
                suppliesCharStatHolder[i].SetActive(false);
            }
        }
    }

    public void UpdateEquipStats()
    {
        playerStats = GameManager.instance.playerStats;

        for (int i = 0; i < playerStats.Length; i++)
        {
            if (playerStats[i].gameObject.activeInHierarchy)
            {
                equipmentCharStatHolder[i].SetActive(true);

                equipNameText[i].text = playerStats[i].charName;
                equipmentCharImage[i].sprite = playerStats[i].charImage;

                if (playerStats[i].equipWeapon1 != "")
                {
                    equipWeap1Image[i].rectTransform.rotation = Quaternion.Euler(0, 0, -45);
                    equipWeap1Image[i].rectTransform.sizeDelta = new Vector2(81f, 122f);
                    equipWeap1Image[i].sprite = GameManager.instance.GetItemDetails(playerStats[i].equipWeapon1).itemSprite;
                    equipWeap1Text[i].text = playerStats[i].equipWeapon1;
                }
                else
                {
                    equipWeap1Image[i].rectTransform.rotation = Quaternion.Euler(0, 0, 0);
                    equipWeap1Image[i].rectTransform.sizeDelta = new Vector2(59f, 59f);
                    equipWeap1Image[i].sprite = defaultWeap1Image;
                    equipWeap1Text[i].text = "Main Hand";
                }

                if (playerStats[i].equipWeapon2 != "")
                {
                    equipWeap2Image[i].rectTransform.rotation = Quaternion.Euler(0, 0, -45);
                    equipWeap2Image[i].rectTransform.sizeDelta = new Vector2(81f, 122f);
                    equipWeap2Image[i].sprite = GameManager.instance.GetItemDetails(playerStats[i].equipWeapon2).itemSprite;
                    equipWeap2Text[i].text = playerStats[i].equipWeapon2;
                }
                else
                {
                    equipWeap2Image[i].rectTransform.rotation = Quaternion.Euler(0, 0, 0);
                    equipWeap2Image[i].rectTransform.sizeDelta = new Vector2(59f, 59f);
                    equipWeap2Image[i].sprite = defaultWeap2Image;
                    equipWeap2Text[i].text = "Off Hand";
                }

                if (playerStats[i].equipArmor != "")
                {
                    equipArmorImage[i].sprite = GameManager.instance.GetItemDetails(playerStats[i].equipArmor).itemSprite;
                    equipArmorText[i].text = playerStats[i].equipArmor;
                }
                else
                {
                    equipArmorImage[i].sprite = defaultArmorImage;
                    equipArmorText[i].text = "Armor";
                }

                if (playerStats[i].equipArtifact1 != "")
                {
                    equipArt1Image[i].sprite = GameManager.instance.GetItemDetails(playerStats[i].equipArtifact1).itemSprite;
                    equipArt1Text[i].text = playerStats[i].equipArtifact1;
                }
                else
                {
                    equipArt1Image[i].sprite = defaultArtifact1Image;
                    equipArt1Text[i].text = "Artifact";
                }

                if (playerStats[i].equipArtifact2 != "")
                {
                    equipArt2Image[i].sprite = GameManager.instance.GetItemDetails(playerStats[i].equipArtifact2).itemSprite;
                    equipArt2Text[i].text = playerStats[i].equipArtifact2;
                }
                else
                {
                    equipArt2Image[i].sprite = defaultArtifact2Image;
                    equipArt2Text[i].text = "Artifact";
                }

                equipAttackPowerText[i].text = playerStats[i].attackPower.ToString();
                equipSpellPowerText[i].text = playerStats[i].magicPower.ToString();
                equipDefenseText[i].text = playerStats[i].currentDefense.ToString();
                equipMagicDefenseText[i].text = playerStats[i].magicDefense.ToString();
                equipSpeedText[i].text = playerStats[i].currentSpeed.ToString();
                equipCritText[i].text = playerStats[i].currentCrit.ToString();
            }
            else
            {
                equipmentCharStatHolder[i].SetActive(false);
            }
        }
    }

    public void UseItem(int selectChar)
    {
        activeItem.Use(selectChar);

        firstButtonSelected = false;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(suppliesFirstButton);
        firstButtonSelected = true;
    }

    public void EquipItem()
    {
        activeItem.Equip(charToEquip, equipSlot);

        CloseEquipChoice();
    }

    public void OpenEquipChoice(int equipType)
    {
        equipSlot = equipType;
        equipChoice.SetActive(true);

        for (int i = 0; i < equipmentCharStatHolder.Length; i++)
        {
            equipmentCharStatHolder[i].SetActive(false);
        }

        switch (equipType)
        {
            case 0:  //is armor
                for (int i = 0; i < equipButtons.Length; i++)
                {
                    if (IsArmor(i))
                    {
                        FindEquippable(i);
                    }
                }
                SortEquip();

                DisplayEquippable();

                break;
            case 1: //is artifact 1
                for (int i = 0; i < equipButtons.Length; i++)
                {
                    if (IsArtifact(i))
                    {
                        FindEquippable(i);
                    }
                }
                SortEquip();

                DisplayEquippable();

                break;
            case 2: //is artifact 2
                for (int i = 0; i < equipButtons.Length; i++)
                {
                    if (IsArtifact(i))
                    {
                        FindEquippable(i);
                    }
                }
                SortEquip();

                DisplayEquippable();

                break;
            case 3: //is weapon 1
                for (int i = 0; i < equipButtons.Length; i++)
                {
                    if (IsWeaponMainHand(i))
                    {
                        FindEquippable(i);
                    }
                }
                SortEquip();

                DisplayEquippable();

                break;
            case 4: //is weapon 2
                for (int i = 0; i < equipButtons.Length; i++)
                {
                    if (IsWeaponOffHand(i))
                    {
                        FindEquippable(i);
                    }
                }
                SortEquip();

                DisplayEquippable();

                break;
        }

        firstButtonSelected = false;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(equipChoiceFirstButton);
        firstButtonSelected = true;
    }

    private void DisplayEquippable()
    {
        for (int i = 0; i < equipButtons.Length; i++)
        {
            equipButtons[i].buttonValue = i;

            if (equipToShow[i] != "")
            {
                equipButtons[i].buttonImage.gameObject.SetActive(true);
                equipButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(equipToShow[i]).itemSprite;
                equipButtons[i].amountText.text = numberOfEquipToShow[i].ToString();
            }
            else
            {
                equipButtons[i].buttonImage.gameObject.SetActive(false);
                equipButtons[i].amountText.text = "";
            }
        }
    }

    private void FindEquippable(int i)
    {
        for (int j = 0; j < GameManager.instance.GetItemDetails(GameManager.instance.equipmentHeld[i]).canUse.Length; j++)
        {
            if (playerStats[charToEquip].charName == GameManager.instance.GetItemDetails(GameManager.instance.equipmentHeld[i]).canUse[j])
            {
                equipToShow[i] = GameManager.instance.equipmentHeld[i];
                numberOfEquipToShow[i] = GameManager.instance.numberOfEquipment[i];
                break;
            }
        }
    }

    private bool IsWeaponOffHand(int i)
    {
        return GameManager.instance.equipmentHeld[i] != "" && GameManager.instance.GetItemDetails(GameManager.instance.equipmentHeld[i]).isWeapon && GameManager.instance.GetItemDetails(GameManager.instance.equipmentHeld[i]).isOffHand;
    }

    private bool IsWeaponMainHand(int i)
    {
        return GameManager.instance.equipmentHeld[i] != "" && GameManager.instance.GetItemDetails(GameManager.instance.equipmentHeld[i]).isWeapon && GameManager.instance.GetItemDetails(GameManager.instance.equipmentHeld[i]).isMainHand;
    }

    private bool IsArtifact(int i)
    {
        return GameManager.instance.equipmentHeld[i] != "" && GameManager.instance.GetItemDetails(GameManager.instance.equipmentHeld[i]).isArtifact;
    }

    private bool IsArmor(int i)
    {
        return GameManager.instance.equipmentHeld[i] != "" && GameManager.instance.GetItemDetails(GameManager.instance.equipmentHeld[i]).isArmor;
    }

    public void SortEquip()
    {
        bool itemAfterSpace = true;

        while (itemAfterSpace)
        {
            itemAfterSpace = false;

            for (int i = 0; i < equipToShow.Length - 1; i++)
            {
                if (equipToShow[i] == "")
                {
                    equipToShow[i] = equipToShow[i + 1];
                    equipToShow[i + 1] = "";

                    numberOfEquipToShow[i] = numberOfEquipToShow[i + 1];
                    numberOfEquipToShow[i + 1] = 0;

                    if (equipToShow[i] != "")
                    {
                        itemAfterSpace = true;
                    }
                }
            }
        }
    }

    private void ClearEquipToShow()
    {
        for (int i = 0; i < equipToShow.Length; i++)
        {
            equipToShow[i] = "";
        }
    }
    
    public void CloseEquipChoice()
    {
        equipChoice.SetActive(false);

        UpdateEquipStats();
        ClearEquipToShow();

        firstButtonSelected = false;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(equipFirstButton);
        firstButtonSelected = true;
    }

    public void QuitToMainMenu()
    {
        CloseMenu();
        SceneManager.LoadScene("MainMenu");
    }
}
