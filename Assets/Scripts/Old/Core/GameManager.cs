using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public CharStats[] playerStats;

    public bool gameMenuOpen, dialogActive, fadingBetweenScenes, shopActive;

    public string[] itemsHeld, equipmentHeld;
    public int[] numberOfItems, numberOfEquipment;
    public Item[] referenceItems;

    public SpriteRenderer weapon1Sprite, weapon2Sprite;
    public GameObject weapon1, weapon2;

    public float timePlayed;

    public int currentGold;

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

        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        SortItems();
        SortEquip();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameMenuOpen || dialogActive || fadingBetweenScenes || shopActive)
        {
            PlayerController2.instance.canMove = false;
        }
        else
        {
            PlayerController2.instance.canMove = true;
        }


#if UNITY_EDITOR   
        if (Input.GetKeyDown(KeyCode.J))
        {
            AddItem("Apple");
            RemoveItem("Pear");
        }
#endif

        UpdateWeapons();

        timePlayed += Time.deltaTime;
    }

    private void UpdateWeapons()
    {
        if (playerStats[0].equipWeapon1 != "")
        {
            weapon1Sprite.sprite = GetItemDetails(playerStats[0].equipWeapon1).itemSprite;
            weapon1.SetActive(true);
        }
        else
        {
            weapon1.SetActive(false);
        }

        if (playerStats[0].equipWeapon2 != "")
        {
            weapon2Sprite.sprite = GetItemDetails(playerStats[0].equipWeapon2).itemSprite;
            weapon2.SetActive(true);
        }
        else
        {
            weapon2.SetActive(false);
        }
    }

    public Item GetItemDetails(string itemToGrab)
    {
        for(int i = 0; i < referenceItems.Length; i++)
        {
            if (referenceItems[i].itemName == itemToGrab)
            {
                return referenceItems[i];
            }
        }
        return null;
    }

    public void SortItems()
    {
        bool itemAfterSpace = true;

        while (itemAfterSpace)
        {
            itemAfterSpace = false;
            
            for (int i = 0; i < itemsHeld.Length - 1; i++)
            {
                if (itemsHeld[i] == "")
                {
                    itemsHeld[i] = itemsHeld[i + 1];
                    itemsHeld[i + 1] = "";

                    numberOfItems[i] = numberOfItems[i + 1];
                    numberOfItems[i + 1] = 0;

                    if(itemsHeld[i] != "")
                    {
                        itemAfterSpace = true;
                    }
                }
            }
        }
    }

    public void SortEquip()
    {
        bool itemAfterSpace = true;

        while (itemAfterSpace)
        {
            itemAfterSpace = false;

            for (int i = 0; i < equipmentHeld.Length - 1; i++)
            {
                if (equipmentHeld[i] == "")
                {
                    equipmentHeld[i] = equipmentHeld[i + 1];
                    equipmentHeld[i + 1] = "";

                    numberOfEquipment[i] = numberOfEquipment[i + 1];
                    numberOfEquipment[i + 1] = 0;

                    if (equipmentHeld[i] != "")
                    {
                        itemAfterSpace = true;
                    }
                }
            }
        }
    }

    public void AddItem(string itemToAdd)
    {
        int newItemPosition = 0;
        bool foundSpace = false;

        for (int i = 0; i < itemsHeld.Length; i++)
        {
            if(itemsHeld[i] == "" | itemsHeld[i] == itemToAdd)
            {
                newItemPosition = i;
                i = itemsHeld.Length;
                foundSpace = true;
            }
        }

        if (foundSpace)
        {
            bool itemExists = false;
            for (int i = 0; i < referenceItems.Length; i++)
            {
                if(referenceItems[i].itemName == itemToAdd)
                {
                    itemExists = true;
                    i = referenceItems.Length;
                }
            }

            if (itemExists)
            {
                itemsHeld[newItemPosition] = itemToAdd;
                numberOfItems[newItemPosition]++;
            }
            else
            {
                Debug.LogError(itemToAdd + " does not exist.");
            }
        }

        GameMenu.instance.ShowSupplies();
    }

    public void AddEquipment(string equipToAdd)
    {
        int newItemPosition = 0;
        bool foundSpace = false;

        for (int i = 0; i < equipmentHeld.Length; i++)
        {
            if (equipmentHeld[i] == "" | equipmentHeld[i] == equipToAdd)
            {
                newItemPosition = i;
                i = equipmentHeld.Length;
                foundSpace = true;
            }
        }

        if (foundSpace)
        {
            bool itemExists = false;
            for (int i = 0; i < referenceItems.Length; i++)
            {
                if (referenceItems[i].itemName == equipToAdd)
                {
                    itemExists = true;
                    i = referenceItems.Length;
                }
            }

            if (itemExists)
            {
                equipmentHeld[newItemPosition] = equipToAdd;
                numberOfEquipment[newItemPosition]++;
            }
            else
            {
                Debug.LogError(equipToAdd + " does not exist.");
            }
        }

        GameMenu.instance.ShowEquipment();
    }

    public void RemoveItem(string itemToRemove)
    {
        bool foundItem = false;
        int itemPosition = 0;

        for (int i = 0; i < itemsHeld.Length; i++)
        {
            if (itemsHeld[i] == itemToRemove)
            {
                foundItem = true;
                itemPosition = i;

                i = itemsHeld.Length;
            }
        }

        if (foundItem)
        {
            numberOfItems[itemPosition]--;

            if(numberOfItems[itemPosition] <= 0)
            {
                itemsHeld[itemPosition] = "";
            }

            GameMenu.instance.ShowSupplies();
        }
        else
        {
            Debug.LogError("Couldn't find " + itemToRemove);
        }
    }

    public void RemoveEquipment(string equipToRemove)
    {
        bool foundItem = false;
        int itemPosition = 0;

        for (int i = 0; i < equipmentHeld.Length; i++)
        {
            if (equipmentHeld[i] == equipToRemove)
            {
                foundItem = true;
                itemPosition = i;

                i = equipmentHeld.Length;
            }
        }

        if (foundItem)
        {
            numberOfEquipment[itemPosition]--;

            if (numberOfEquipment[itemPosition] <= 0)
            {
                equipmentHeld[itemPosition] = "";
            }

            GameMenu.instance.ShowEquipment();
        }
        else
        {
            Debug.LogError("Couldn't find " + equipToRemove);
        }
    }
}
