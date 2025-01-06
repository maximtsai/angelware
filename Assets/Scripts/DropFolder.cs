using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropFolder : MonoBehaviour
{
    public int folderIdx;
    Animator myAnim;
    private bool justDropped = false;

    // Start is called before the first frame update
    void Start()
    {
        myAnim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        myAnim.Play("hover");

        /*
        if (collision.CompareTag("Draggable"))
        {
            Debug.Log("Object entered the drop area: " + collision.name);
        }
        */
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // If we just dropped off file, don't make it play idle animation right away
        if (justDropped)
        {
            justDropped = false;
        } else
        {
            myAnim.Play("idle");
        }
    }

    public void OnObjectDropped(GameObject droppedObject)
    {
        justDropped = true;
        DraggableItem draggableItem = droppedObject.GetComponent<DraggableItem>();
        if (draggableItem.folderIdx == folderIdx)
        {
            myAnim.Play("catch");
        }
        else
        {
            myAnim.Play("reject");
        }

    }


    // Update is called once per frame
    void Update()
    {
        
    }

}
