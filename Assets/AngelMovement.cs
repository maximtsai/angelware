using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngelMovement : MonoBehaviour
{
    public Camera mainCamera;
    public float borderBuffer = 50f; // Buffer in pixels from the edges of the screen
    private Vector3 lastFewVelocity;

    public Animator myAnim;
    bool hasFile = false;
    void Start()
    {
    	myAnim = GetComponent<Animator>();
    	lastFewVelocity = new Vector3(0, 0, 0);
    }

    void Update()
    {
    	// Get the mouse position in screen coordinates
    	Vector3 mouseScreenPosition = Input.mousePosition;

    	// Clamp mouse pos to stay within screen bounds
        float clampedX = Mathf.Clamp(mouseScreenPosition.x, borderBuffer, Screen.width - borderBuffer);
        float clampedY = Mathf.Clamp(mouseScreenPosition.y, borderBuffer, Screen.height - borderBuffer);
        mouseScreenPosition = new Vector3(clampedX, clampedY, 0f);

    	// convert to world pos
    	Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);

    	Vector3 oldPos = transform.position;

    	transform.position = new Vector3(mouseWorldPosition.x * 0.1f + transform.position.x * 0.9f, mouseWorldPosition.y * 0.1f + transform.position.y * 0.9f, -1f);

    	Vector3 diff = transform.position - oldPos;
    	
    	updateVelocityAndAnim(diff);
    }

  private void updateVelocityAndAnim(Vector3 diff) {
    lastFewVelocity = lastFewVelocity * 0.5f + diff * 0.5f;
    if (lastFewVelocity.magnitude < 0.01f) {
    	SwitchState("idle");
    } else if (lastFewVelocity.y < 0) {
    	// flying up (I think)
    	 SwitchState("flyup");
    } else if (lastFewVelocity.y > 0) {
    	// flying up (I think)
    	 SwitchState("flydown");
    }

    }

    private void SwitchState(string nextState) {

        switch (nextState)
        {
            case "idle":
            	if (hasFile) {
    			myAnim.Play("ang_idle_bubble");
            	} else {
    			myAnim.Play("ang_idle");
            	}
                break;
            case "flyup":
    			myAnim.Play("ang_fly1");
                break;
            case "flydown":
    			myAnim.Play("ang_fly2");
                break;
            default:
    			myAnim.Play("ang_idle");
                break;
        }

    }
}