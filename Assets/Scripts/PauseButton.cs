using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour
{
    public GameObject menu;

    private bool menuVisible;
    private float transitionTime = 2f;
    private float elapsedTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        menuVisible = false;
        menu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseDown()
    {
        menuVisible = !menuVisible;
        menu.SetActive(menuVisible);
    }
}
