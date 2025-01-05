using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float totalTimeSeconds = 300f;
    [SerializeField]
    private float timeRemaining;
    // Start is called before the first frame update
    void Start()
    {
        timeRemaining = totalTimeSeconds;
    }

    // Update is called once per frame
    void Update()
    {
        timeRemaining = Mathf.Max(timeRemaining - Time.deltaTime, 0f);

        string minutes = ((int) Mathf.Floor(timeRemaining / 60f)).ToString();
        string seconds = (((int) Mathf.Floor(timeRemaining)) % 60).ToString("00");
        this.GetComponentInChildren<TextMeshPro>().text = minutes + ":" + seconds;
    }

    public float getTimeRemaining()
    {
        return this.timeRemaining;
    }

    public bool isGameOver()
    {
        return (0f >= this.timeRemaining);
    }
}
