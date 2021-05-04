using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ShopKeeper : MonoBehaviour
{
    [SerializeField] bool canOpen;

    public string[] itemsForSale = new string[44];
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canOpen && Input.GetButtonDown("Submit") && PlayerController2.instance.canMove && !Shop.instance.justClosed)
        {
            Shop.instance.itemsForSale = itemsForSale;
            
            Shop.instance.OpenShop();
        }

        if (Shop.instance.justClosed)
        {
            Shop.instance.justClosed = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            canOpen = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canOpen = false;
        }
    }
}
