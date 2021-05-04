using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public string[] questMarkerNames;
    public bool[] questMarkersComplete;

    public static QuestManager instance;

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
        questMarkersComplete = new bool[questMarkerNames.Length];
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Q))
        {
            print(CheckIfComplete("quest test"));
            MarkQuestComplete("quest test");
            MarkQuestIncomplete("Find Kel'Vara");
        }
#endif
    }

    public bool CheckIfComplete(string questToCheck)
    {
        if(GetQuestNumber(questToCheck) != 0)
        {
            return questMarkersComplete[GetQuestNumber(questToCheck)];
        }
        return false;
    }

    public void MarkQuestComplete(string questToMark)
    {
        questMarkersComplete[GetQuestNumber(questToMark)] = true;

        UpdateLocalQuestObjects();
    }

    public void MarkQuestIncomplete(string questToMark)
    {
        questMarkersComplete[GetQuestNumber(questToMark)] = false;

        UpdateLocalQuestObjects();
    }

    public void UpdateLocalQuestObjects()
    {
        QuestObjectActivator[] questObjects = FindObjectsOfType<QuestObjectActivator>();

        if(questObjects.Length > 0)
        {
            for(int i = 0; i < questObjects.Length; i++)
            {
                questObjects[i].CheckCompletion();
            }
        }
    }

    public int GetQuestNumber(string questToFind)
    {
        for(int i = 0; i < questMarkerNames.Length; i++)
        {
            if(questMarkerNames[i] == questToFind)
            {
                return i;
            }
        }
        print("Quest " + questToFind + " does not exist");
        return 0;
    }
}
