using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; 

[RequireComponent(typeof(Image))]
public class InGameStartButton : MonoBehaviour
    , IPointerEnterHandler
    , IPointerExitHandler
{
    public Sprite startButtonDefaultSprite;
    private Image image;
    private UISpritesAnimation uiSpritesAnimation;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        uiSpritesAnimation = GetComponent<UISpritesAnimation>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        uiSpritesAnimation.currentSpriteSequence = 0;
        uiSpritesAnimation.loop = true;
        uiSpritesAnimation.duration = 0.5f;
        uiSpritesAnimation.Play();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        uiSpritesAnimation.Stop();
        image.sprite = startButtonDefaultSprite;
    }
}
