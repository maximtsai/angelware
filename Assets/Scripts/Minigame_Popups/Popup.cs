using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;

public class Popup : MonoBehaviour
{
    public GameObject parent;
    public MinigamePopup minigameController;

    // Start is called before the first frame update
    void Start()
    {
        this.orderHitboxes();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseDown()
    {
        minigameController.KillPopup();
        Destroy(parent);
    }

    public void orderHitboxes()
    {
        UnityEngine.Vector3 pos = this.gameObject.transform.position;
        this.gameObject.transform.position = new UnityEngine.Vector3(
            pos.x,
            pos.y,
            parent.gameObject.transform.position.z - 0.0001f
        );
    }
}
