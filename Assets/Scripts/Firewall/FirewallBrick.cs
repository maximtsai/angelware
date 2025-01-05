using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirewallBrick : MonoBehaviour
{
    public enum HealthState {State0, State1, State2, Destroyed};

    public Sprite state0Sprite;
    public Sprite state1Sprite;
    public Sprite state2Sprite;

    public Sprite brickAState0Sprite;
    public Sprite brickAState1Sprite;
    public Sprite brickAState2Sprite;
    public Sprite brickBState0Sprite;
    public Sprite brickBState1Sprite;
    public Sprite brickBState2Sprite;
    public Sprite brickCState0Sprite;
    public Sprite brickCState1Sprite;
    public Sprite brickCState2Sprite;
    public Sprite brickDState0Sprite;
    public Sprite brickDState1Sprite;
    public Sprite brickDState2Sprite;


    public FirewallMinigame firewallMinigame;
    

    private HealthState healthState;
    private int brickType;

    // Start is called before the first frame update
    void Start()
    {
        healthState = HealthState.State0;
        GetComponent<SpriteRenderer>().enabled = true;

        brickType = Random.Range(0,4);
        
        SetAppearance();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown() {
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
                switch (brickType) {
                    case 0:
                        GetComponent<SpriteRenderer>().sprite = brickAState0Sprite;
                        break;
                    case 1:
                        GetComponent<SpriteRenderer>().sprite = brickBState0Sprite;
                        break;
                    case 2:
                        GetComponent<SpriteRenderer>().sprite = brickCState0Sprite;
                        break;
                    case 3:
                        GetComponent<SpriteRenderer>().sprite = brickDState0Sprite;
                        break;
                }

                // GetComponent<SpriteRenderer>().sprite = state0Sprite;
                break;
            case HealthState.State1:
                switch (brickType) {
                    case 0:
                        GetComponent<SpriteRenderer>().sprite = brickAState1Sprite;
                        break;
                    case 1:
                        GetComponent<SpriteRenderer>().sprite = brickBState1Sprite;
                        break;
                    case 2:
                        GetComponent<SpriteRenderer>().sprite = brickCState1Sprite;
                        break;
                    case 3:
                        GetComponent<SpriteRenderer>().sprite = brickDState1Sprite;
                        break;
                }
                // GetComponent<SpriteRenderer>().sprite = state1Sprite;
                break;
            case HealthState.State2:
                switch (brickType) {
                    case 0:
                        GetComponent<SpriteRenderer>().sprite = brickAState2Sprite;
                        break;
                    case 1:
                        GetComponent<SpriteRenderer>().sprite = brickBState2Sprite;
                        break;
                    case 2:
                        GetComponent<SpriteRenderer>().sprite = brickCState2Sprite;
                        break;
                    case 3:
                        GetComponent<SpriteRenderer>().sprite = brickDState2Sprite;
                        break;
                }
                // GetComponent<SpriteRenderer>().sprite = state2Sprite;
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
