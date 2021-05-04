using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuButtonHover : MonoBehaviour, ISelectHandler
{
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
        if(Shop.instance.shopMenu.activeInHierarchy)
        {
            if (Shop.instance.firstButtonSelected)
            {
                Shop.instance.itemDescription.text = "Hello! What would you like to do?";
            }
        }
    }
}
