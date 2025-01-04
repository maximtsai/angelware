using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeScreen : MonoBehaviour
{
    private bool fading = false;
    private float fadeAmount = 0.0f;
    // Start is called before the first fr\ame update
    void Start()
    {
        GetComponent<Image>().enabled = false;
        GetComponent<Image>().color = new Color(1,1,1,0);
    }

    // Update is called once per frame
    void Update()
    {
        Color screenColor = GetComponent<Image>().color;
        if (fading) {
            fadeAmount += Time.deltaTime / 0.5f;
            GetComponent<Image>().color = new Color(screenColor.r, screenColor.g, screenColor.b, fadeAmount);
        }
    }

    public void StartFadeScreen()
    {
        fading = true;
        GetComponent<Image>().enabled = true;
    }
}
