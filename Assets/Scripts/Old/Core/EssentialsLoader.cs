using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssentialsLoader : MonoBehaviour
{
    public GameObject UIScreen, player, gameMan;

    
    // Start is called before the first frame update
    void Start()
    {
        if(UIFade.instance == null)
        {
            UIFade.instance = Instantiate(UIScreen).GetComponent<UIFade>();
        }

        if(PlayerController2.instance == null)
        {
            PlayerController2 clone = Instantiate(player).GetComponent<PlayerController2>();
            PlayerController2.instance = clone;
        }
        if(GameManager.instance == null)
        {
            Instantiate(gameMan);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
