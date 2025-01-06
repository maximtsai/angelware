using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngelwareLogo : MonoBehaviour
{
    public float scaleValue = 0.3f;
    private float timer = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        transform.localScale = new Vector3(Mathf.Sin(timer * 2) * scaleValue, scaleValue, 0f);
        GetComponent<RectTransform>().anchoredPosition = new Vector3(-301f, Mathf.Sin(timer * 4) * 5, -5f);
    }
}
