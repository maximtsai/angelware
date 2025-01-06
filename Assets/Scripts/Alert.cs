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
    public Sprite openSprite;
    public AnimationClip alertEnter;
    public Sprite windowFirewallSprite;
    public Sprite windowMalwareSprite;
    public GameObject window;
    public GameObject okButton;
    public GameObject exclamation;
    public GameObject glow;
    public AudioClip notificationSound;
    public AudioClip virusSound;

    private Image image;
    private UISpritesAnimation uiSpritesAnimation;
    private AudioSource audioSource;

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

        audioSource = GetComponent<AudioSource>();

        window.SetActive(false);

        okButton.SetActive(false);
        okButton.GetComponent<AlertOKButton>().enabled = false;

        exclamation.SetActive(false);
        exclamation.GetComponent<UISpritesAnimation>().duration = 1.0f;
        exclamation.GetComponent<UISpritesAnimation>().playOnAwake = true;
        exclamation.GetComponent<UISpritesAnimation>().loop = true;

        glow.SetActive(false);

        phase = Phase.IDLE;
    }

    void Update() 
    {
    }

    public void TriggerAlert(string type) {
        if (type == "popup") {
            window.GetComponent<Image>().sprite = windowMalwareSprite;
        }
        else if (type == "firewall") {
            window.GetComponent<Image>().sprite = windowFirewallSprite;
        }
        phase = Phase.WAITING;
        uiSpritesAnimation.currentSpriteSequence = 0;
        uiSpritesAnimation.duration = 0.6f;
        uiSpritesAnimation.Play();

        exclamation.SetActive(true);
        glow.SetActive(true);

        audioSource.volume = 1f;
        audioSource.clip = notificationSound;
        audioSource.loop = false;
        audioSource.Play();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (phase == Phase.WAITING) {
            phase = Phase.WINDOW;

            uiSpritesAnimation.Stop();
            image.sprite = openSprite;

            exclamation.SetActive(false);
            window.SetActive(true);
            okButton.SetActive(true);
            okButton.GetComponent<AlertOKButton>().enabled = true;

            glow.SetActive(false);

            audioSource.volume = 0.3f;
            audioSource.clip = virusSound;
            audioSource.loop = false;
            audioSource.Play();
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
        
        glow.SetActive(false);
    }
}
