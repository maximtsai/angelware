using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Hide", 6f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hide() {
        GetComponent<Animator>().Play("TutorialTextLeave");
    }
}
