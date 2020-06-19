using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class soundManager : MonoBehaviour
{
    //public AudioClip sound;
    public AudioSource music;
    public AudioClip music1;
    public AudioClip music2;
    public AudioSource loseSound;
    public AudioSource winSound;
    public bool scene = true;

    public static soundManager insatnce;

    private void Awake()
    {
        if (insatnce == null)
        {
            insatnce = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        enableMusic();
    }

    public void enableMusic()
    {
        music.Play();
    }
    public void disableMusic()
    {
        music.Stop();
    }

    public void win()
    {
        winSound.Play();
        disableMusic();
    }
    public void lose()
    {
        loseSound.Play();
        disableMusic();
    }
}
