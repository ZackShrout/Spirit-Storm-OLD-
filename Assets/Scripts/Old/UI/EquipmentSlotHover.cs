using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EquipmentSlotHover : MonoBehaviour, ISelectHandler
{
    public string description;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (GameMenu.instance.theMenu.activeInHierarchy)
        {
            GameMenu.instance.menuFeedText.text = description;
        }
    }
}
