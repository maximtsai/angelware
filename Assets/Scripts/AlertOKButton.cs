using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertOKButton : MonoBehaviour
{
    public Alert alert;
    public GameStateManager gameStateManager;

    public void OKButtonClicked () {
        alert.BackToIdle();
        gameStateManager.AddMinigame();
    }
}
