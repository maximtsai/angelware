using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirewallBG : MonoBehaviour
{
    public enum FirewallHealth {Healthy, Warning, Destroyed};

    public Sprite healthySprite;
    public Sprite warningSprite;
    public Sprite destroyedSprite;

    private FirewallHealth health;
    // Start is called before the first frame update
    void Start()
    {
        health = FirewallHealth.Healthy;
        GetComponent<SpriteRenderer>().sprite = healthySprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Healthy() {
        health = FirewallHealth.Healthy;
        GetComponent<SpriteRenderer>().sprite = healthySprite;
    }

    public void Warning() {
        health = FirewallHealth.Warning;
        GetComponent<SpriteRenderer>().sprite = warningSprite;
    }
    
    public void Destroyed() {
        health = FirewallHealth.Destroyed;
        GetComponent<SpriteRenderer>().sprite = destroyedSprite;
    }
}
