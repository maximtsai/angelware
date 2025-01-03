using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropFolder : MonoBehaviour
{
    public int folderIdx;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Draggable"))
        {
            Debug.Log("Object entered the drop area: " + collision.name);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("exit");
    }

    public void OnObjectDropped(GameObject droppedObject)
    {
        Debug.Log("Object was dropped in area");
        // DraggableItem draggableItem = droppedObject.GetComponent<DraggableItem>();

    }


    // Update is called once per frame
    void Update()
    {
        
    }

}
