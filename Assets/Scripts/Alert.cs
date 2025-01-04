using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Alert : MonoBehaviour
{
    public AnimationClip alertEnter;
    public Sprite windowSprite;

    private Image image;
    private Animation animationComponent;
    private UISpritesAnimation uiSpritesAnimation;
    private GameObject okButton;

    private enum Phase { WAITING, WINDOW }
    private Phase phase = Phase.WAITING;

    void Start()
    {
        image = GetComponent<Image>();
        // image.color = new Color(1, 1, 1, 0);
        
        animationComponent = GetComponent<Animation>();
        
        uiSpritesAnimation = GetComponent<UISpritesAnimation>();
        uiSpritesAnimation.currentSpriteSequence = 0;
        uiSpritesAnimation.playOnAwake = true;
        uiSpritesAnimation.loop = true;

        okButton = GameObject.Find("OKButton");
        okButton.SetActive(false);
        okButton.GetComponent<AlertOKButton>().enabled = false;

        phase = Phase.WAITING;
        // Invoke("Appear", 1f);
    }

    // public void Appear() {
    //     animationComponent.clip = alertEnter;
    //     animationComponent.Play();
    // }

    void OnMouseDown() {
        if (phase == Phase.WAITING) {
            phase = Phase.WINDOW;
            image.sprite = windowSprite;
            okButton.SetActive(true);
            okButton.GetComponent<AlertOKButton>().enabled = true;
        }
        else if (phase == Phase.WINDOW) {
            // do nothing
        }
    }
}
