using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; 

[RequireComponent(typeof(Image))]
public class Alert : MonoBehaviour
    , IPointerClickHandler
{
    public Sprite idleSprite;
    public AnimationClip alertEnter;
    public Sprite windowFirewallSprite;
    public Sprite windowMalwareSprite;
    public GameObject window;
    public GameObject okButton;
    public GameObject exclamation;

    private Image image;
    private UISpritesAnimation uiSpritesAnimation;

    private enum Phase { IDLE, WAITING, WINDOW }
    private Phase phase = Phase.IDLE;

    private float timer = 0.0f;

    void Start()
    {
        image = GetComponent<Image>();
        if (idleSprite != null) {
            image.sprite = idleSprite;
        }
        
        uiSpritesAnimation = GetComponent<UISpritesAnimation>();

        window.SetActive(false);

        okButton.SetActive(false);
        okButton.GetComponent<AlertOKButton>().enabled = false;

        exclamation.SetActive(false);

        phase = Phase.IDLE;
    }

    void Update() 
    {
        if (phase == Phase.WAITING) {
            timer += Time.deltaTime;
            if (timer > 0.3f) {
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
        uiSpritesAnimation.duration = 0.6f;
        uiSpritesAnimation.Play();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (phase == Phase.WAITING) {
            phase = Phase.WINDOW;

            uiSpritesAnimation.Stop();
            image.sprite = idleSprite;
            
            exclamation.SetActive(false);
            window.SetActive(true);
            okButton.SetActive(true);
            okButton.GetComponent<AlertOKButton>().enabled = true;
        }
    }

    public void BackToIdle() {
        phase = Phase.IDLE;

        uiSpritesAnimation.Stop();
        image.sprite = idleSprite;

        window.SetActive(false);
        okButton.SetActive(false);
        okButton.GetComponent<AlertOKButton>().enabled = false;
        exclamation.SetActive(false);
    }
}
