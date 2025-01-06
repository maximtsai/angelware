using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalAudio : MonoBehaviour
{
    public AudioClip mainBgm;
    public AudioClip clickSound;
    public AudioClip clickDownSound;
    public AudioClip clickUpSound;

    private AudioSource audioSourceLoop;
    private AudioSource audioSourceSingle;

    // Start is called before the first frame update
    void Start()
    {
        audioSourceLoop = GetComponents<AudioSource>()[0];
        audioSourceSingle = GetComponents<AudioSource>()[1];

        if (mainBgm != null) {
            audioSourceLoop.clip = mainBgm;
            audioSourceLoop.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            audioSourceSingle.clip = clickDownSound;
            audioSourceSingle.Play();
        }

        if (Input.GetMouseButtonUp(0)) {
            audioSourceSingle.clip = clickUpSound;
            audioSourceSingle.Play();
        }
    }
}
