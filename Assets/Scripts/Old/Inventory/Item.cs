using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] CharStats[] playerStats;
    
    [Header("Item Type")]
    public bool isItem;
    public bool isWeapon;
    public bool isArmor, isArtifact;

    [Header("Item Description")]
    public string itemName;
    public string description;
    public int value;
    public Sprite itemSprite;

    [Header("Item Properties")]
    public int amountToChange;
    public bool isPartyWide, isMainHand, isOffHand;
    public bool affectHP, affectFocus, affectStrength, affectDefense, affectAgility, affectSpeed, affectCrit;
    public int weaponAttackPower, weaponMagicPower, armorDefense, armorMagicDefense;
    public string[] canUse;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Use(int charToUseOn)
    {
        if (isPartyWide && isItem)
        {
            playerStats = GameMenu.instance.playerStats;
            
            if (affectHP)
            {
                for(int i = 0; i < playerStats.Length; i++)
                {
                    if(playerStats[i].gameObject.activeInHierarchy)
                    {
                        playerStats[i].currentHP += amountToChange;
                        if(playerStats[i].currentHP > playerStats[i].maxHP)
                        {
                            playerStats[i].currentHP = playerStats[i].maxHP;
                        }
                    }
                }
            }
            if (affectFocus)
            {
                for (int i = 0; i < playerStats.Length; i++)
                {
                    if (playerStats[i].gameObject.activeInHierarchy)
                    {
                        playerStats[i].currentFocus += amountToChange;
                        if (playerStats[i].currentFocus > playerStats[i].maxFocus)
                        {
                            playerStats[i].currentFocus = playerStats[i].maxFocus;
                        }
                    }
                }
            }
            GameManager.instance.RemoveItem(itemName);
            return;
        }

        CharStats selectedChar = GameManager.instance.playerStats[charToUseOn];

        if (isItem)
        {
            if (affectHP)
            {
                selectedChar.currentHP += amountToChange;

                if(selectedChar.currentHP > selectedChar.maxHP)
                {
                    selectedChar.currentHP = selectedChar.maxHP;
                }
            }
            if(affectFocus)
            {
                selectedChar.currentFocus += amountToChange;

                if (selectedChar.currentFocus > selectedChar.maxFocus)
                {
                    selectedChar.currentFocus = selectedChar.maxFocus;
                }
            }

            GameManager.instance.RemoveItem(itemName);
        }
    }

    public void Equip(int charToUseOn, int equipSlot)
    {
        CharStats selectedChar = GameManager.instance.playerStats[charToUseOn];

        if (isWeapon)
        {
            if(equipSlot == 3)
            {
                if (selectedChar.equipWeapon1 != "")
                {
                    GameManager.instance.AddEquipment(selectedChar.equipWeapon1);
                }

                selectedChar.equipWeapon1 = itemName;
                selectedChar.attackPower = weaponAttackPower;

                GameManager.instance.RemoveEquipment(itemName);
            }
            
            if(equipSlot == 4)
            {
                if (selectedChar.equipWeapon2 != "")
                {
                    GameManager.instance.AddEquipment(selectedChar.equipWeapon2);
                }

                selectedChar.equipWeapon2 = itemName;
                selectedChar.attackPower = weaponAttackPower;

                GameManager.instance.RemoveEquipment(itemName);
            }
        }

        if (isArmor)
        {
            //Needs updating

            if (selectedChar.equipArmor != "")
            {
                GameManager.instance.AddEquipment(selectedChar.equipArmor);
            }

            selectedChar.equipArmor = itemName;
            selectedChar.currentDefense += armorDefense;

            GameManager.instance.RemoveEquipment(itemName);
        }
    }
}
