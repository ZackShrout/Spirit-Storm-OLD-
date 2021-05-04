using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpiritStorm.Saving;

public class CharStats : MonoBehaviour, ISaveable
{
    public string charName;

    public int playerLevel, currentEXP;
    public int[] expToNextLevel;
    public int maxLevel = 100;
    public int baseEXP = 1000;

    //Main stats
    public int currentHP, maxHP, currentFocus, maxFocus;
    
    //Secondary stats
    public int currentStrength, currentSpeed, currentAgility, currentDefense, currentCrit;
    public int maxStrength, maxSpeed, maxAgility, maxDefense, maxCrit;

    //Stats from equipment
    public int attackPower, magicPower, magicDefense;
    public string equipWeapon1, equipWeapon2, equipArmor, equipArtifact1, equipArtifact2;

    public Sprite charImage;

    // Start is called before the first frame update
    void Start()
    {
        expToNextLevel = new int[maxLevel];
        expToNextLevel[1] = baseEXP;

        for(int i = 2; i < expToNextLevel.Length; i++)
        {
            expToNextLevel[i] = Mathf.FloorToInt(expToNextLevel[i - 1] * 1.05f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.K))
        {
            AddExp(500);
        }
        
    }

    public void AddExp(int expToAdd)
    {
        currentEXP += expToAdd;

        if(playerLevel < maxLevel)
        {
            if (currentEXP >= expToNextLevel[playerLevel])
            {
                currentEXP -= expToNextLevel[playerLevel];
                playerLevel++;

                //determine which stats to increase based on the character's new level:
                //Strength and Speed on even levels; Defense, Agility, and Crit on odd levels
                if (playerLevel % 2 == 0)
                {
                    if (currentStrength < maxStrength)
                    {
                        currentStrength++;
                    }
                    if (currentSpeed < maxSpeed)
                    {
                        currentSpeed++;
                    }
                }
                else
                {
                    if (currentDefense < maxDefense)
                    {
                        currentDefense++;
                    }
                    if (currentAgility < maxAgility)
                    {
                        currentAgility++;
                    }
                    if (currentCrit < maxCrit)
                    {
                        currentCrit++;
                    }
                }
                maxHP = Mathf.FloorToInt(Mathf.MoveTowards(maxHP, 999f, 10f));
                currentHP = maxHP;
                maxFocus = Mathf.FloorToInt(Mathf.MoveTowards(maxFocus, 99f, 1f));
                currentFocus = maxFocus;
            }
        }
        if (playerLevel >= maxLevel)
        {
            currentEXP = 0;
        }
    }

    public object CaptureState()
    {
        //Need to save:
        //int playerLevel
        //int currentEXP
        //int currentFocus
        //int currentStrength, currentSpeed, currentAgility, currentDefense, currentCrit;

        return currentHP;
    }

    public void RestoreState(object state)
    {
        currentHP = (int)state;
    }
}
