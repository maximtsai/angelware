using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitButton : MonoBehaviour
{

    public void QuitButtonClicked()
    {
        Application.Quit();
    }

    public void OnMouseDown()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }
}
