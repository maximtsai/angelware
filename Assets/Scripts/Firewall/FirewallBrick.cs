using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirewallBrick : MonoBehaviour
{
    public enum HealthState {State0, State1, State2, Destroyed};

    public Sprite state0Sprite;
    public Sprite state1Sprite;
    public Sprite state2Sprite;

    public FirewallMinigame firewallMinigame;
    

    private HealthState healthState;

    // Start is called before the first frame update
    void Start()
    {
        healthState = HealthState.State0;
        GetComponent<SpriteRenderer>().enabled = true;
        
        SetAppearance();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown() {
        Debug.Log("clicked!");
        Repair();
        firewallMinigame.UpdateHealth();
    }

    public void DegradeState() {
        switch (healthState) {
            case HealthState.State0:
                healthState = HealthState.State1;
                break;
            case HealthState.State1:
                healthState = HealthState.State2;
                break;
            case HealthState.State2:
                healthState = HealthState.Destroyed;
                break;
            case HealthState.Destroyed:
                break;
        }
        SetAppearance();
    }

    void Repair() {
        // TODO: need to propagate this up...
        switch (healthState) {
            case HealthState.State0:
                // do nothing
                break;
            case HealthState.State1:
                healthState = HealthState.State0;
                break;
            case HealthState.State2:
                healthState = HealthState.State1;
                break;
            case HealthState.Destroyed:
                // do nothing, can't be repaired
                break;
        }
        SetAppearance();
    }

    void SetAppearance() {
        switch (healthState) {
            case HealthState.State0:
                GetComponent<SpriteRenderer>().sprite = state0Sprite;
                break;
            case HealthState.State1:
                GetComponent<SpriteRenderer>().sprite = state1Sprite;
                break;
            case HealthState.State2:
                GetComponent<SpriteRenderer>().sprite = state2Sprite;
                break;
            case HealthState.Destroyed:
                GetComponent<SpriteRenderer>().enabled = false;
                break;
        }
    }

    public HealthState GetHealthState() {
        return healthState;
    }

    public void SetHealthState(HealthState hState) {
        healthState = hState;
        SetAppearance();
    }

}
