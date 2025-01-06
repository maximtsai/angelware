using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuAudio : MonoBehaviour
{
    public AudioClip startupSound;
    public AudioClip startMenuBgm;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.clip = startupSound;
        audioSource.loop = false;

        Invoke("PlayBGM", 4.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PlayBGM() {
        audioSource.clip = startMenuBgm;
        audioSource.loop = true;
        audioSource.volume = 0.3f;
        audioSource.Play();
    }
}
