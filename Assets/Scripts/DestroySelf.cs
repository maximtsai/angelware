using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    public float delayAmt = 0.35f;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyThisObj", delayAmt);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DestroyThisObj()
    {
        Destroy(gameObject);
    }
}
