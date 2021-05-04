﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEntrance : MonoBehaviour
{
    public string transitionName;
    
    // Start is called before the first frame update
    void Start()
    {
      if(transitionName == PlayerController2.instance.areaTransitionName)
      {
            PlayerController2.instance.transform.position = transform.position;
      }
        UIFade.instance.FadeFromBlack();

        GameManager.instance.fadingBetweenScenes = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}