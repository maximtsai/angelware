using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    public GameObject blackScreen;
    private bool clicked = false;

    public void StartButtonClicked()
    {
        if (!clicked) {
            clicked = true;
            blackScreen.SetActive(true);
            blackScreen.GetComponent<FadeScreen>().StartFadeScreen();
            Invoke("LoadMainScene", 0.5f);
        }
    }

    void LoadMainScene() {
        SceneManager.LoadScene("CutsceneIntro");
    }
}
