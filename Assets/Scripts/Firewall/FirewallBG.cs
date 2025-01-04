using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirewallBG : MonoBehaviour
{
    public Sprite healthySprite;
    public Sprite warningSprite;
    public Sprite destroyedSprite;

    
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = healthySprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Healthy() {
        GetComponent<SpriteRenderer>().sprite = healthySprite;
    }

    public void Warning() {
        GetComponent<SpriteRenderer>().sprite = warningSprite;
    }
    
    public void Destroyed() {
        GetComponent<SpriteRenderer>().sprite = destroyedSprite;
    }
}
