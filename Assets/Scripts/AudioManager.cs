using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource backgroundMusic;
    private bool musicStarted = false;

    void Awake() // Awake is called when the script instance is being loaded
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this; // 'this' refers to the current instance of the AudioManager class 
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (!musicStarted)
        {
            backgroundMusic.Play();
            musicStarted = true;
        }
    }
}
