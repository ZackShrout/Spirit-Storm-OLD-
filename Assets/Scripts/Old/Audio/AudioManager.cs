using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource[] sfx;
    [SerializeField] AudioSource[] bgm;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySFX(int soundToPlay)
    {
        sfx[soundToPlay].Play();
    }

    internal void PlayMusic(int musicToPlay)
    {
        bgm[musicToPlay].Play();
    }

    internal void StopMusic(int musicToStop)
    {
        bgm[musicToStop].Stop();
    }
}
