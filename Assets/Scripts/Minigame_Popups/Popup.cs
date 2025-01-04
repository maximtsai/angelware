using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.EventSystems;

public class Popup : MonoBehaviour, IPointerDownHandler
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        UnityEngine.Vector2 p = eventData.position;
        Debug.Log("Mouse down: " + p);

        // Calculate the size of the popup on the screen
        Camera c = Camera.main;
        Renderer r = GetComponent<Renderer>();
        UnityEngine.Vector2 min = c.WorldToScreenPoint(r.bounds.min);
        UnityEngine.Vector2 max = c.WorldToScreenPoint(r.bounds.max);
        UnityEngine.Vector2 size = max - min;
        Debug.Log("Size: " + size);

        // Get object location in screen units (px)
        UnityEngine.Vector2 objPos = c.WorldToScreenPoint(transform.position);

        // Determine top right corner
        UnityEngine.Vector2 topRight = objPos + (size / 2);

        // Check if the hitbox is the top right corner square and destroy object
        const float HITBOX_WIDTH = 14.8225f;
        UnityEngine.Vector2 HITBOX_VECTOR = topRight - new UnityEngine.Vector2(HITBOX_WIDTH, HITBOX_WIDTH);

        if (p.x > HITBOX_VECTOR.x && p.y > HITBOX_VECTOR.y)
        {
            Debug.Log("X pressed");
            Destroy(this.gameObject);
        }
    }
}
