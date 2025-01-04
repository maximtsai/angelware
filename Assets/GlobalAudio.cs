using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalAudio : MonoBehaviour
{
    public AudioClip clickSound;
    public AudioClip clickDownSound;
    public AudioClip clickUpSound;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            audioSource.clip = clickDownSound;
            audioSource.Play();
        }

        if (Input.GetMouseButtonUp(0)) {
            audioSource.clip = clickUpSound;
            audioSource.Play();
        }
    }
}
