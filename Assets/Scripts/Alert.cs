using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Alert : MonoBehaviour
{
    public Sprite idleSprite;
    public AnimationClip alertEnter;
    public Sprite windowSprite;

    private Image image;
    private Animation animationComponent;
    private UISpritesAnimation uiSpritesAnimation;
    private GameObject okButton;
    private GameObject exclamation;

    private enum Phase { IDLE, WAITING, WINDOW }
    private Phase phase = Phase.IDLE;

    private float timer = 0.0f;

    void Start()
    {
        image = GetComponent<Image>();
        if (idleSprite != null) {
            image.sprite = idleSprite;
        }
        // image.color = new Color(1, 1, 1, 0);
        
        animationComponent = GetComponent<Animation>();
        
        uiSpritesAnimation = GetComponent<UISpritesAnimation>();

        okButton = GameObject.Find("OKButton");
        okButton.SetActive(false);
        okButton.GetComponent<AlertOKButton>().enabled = false;

        exclamation = GameObject.Find("Exclamation");
        exclamation.SetActive(false);

        phase = Phase.IDLE;
    }

    void Update() 
    {
        if (phase == Phase.WAITING) {
            timer += Time.deltaTime;
            if (timer > 1.0f) {
                timer = 0f;
                if (exclamation.activeSelf) {
                    exclamation.SetActive(false);
                }
                else {
                    exclamation.SetActive(true);
                }
            }
        }
    }

    public void TriggerAlert() {
        phase = Phase.WAITING;
        uiSpritesAnimation.currentSpriteSequence = 0;
        uiSpritesAnimation.Play();
    }

    void OnMouseDown() {
        if (phase == Phase.WAITING) {
            uiSpritesAnimation.Stop();
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
