using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngelMovement : MonoBehaviour
{
    public Camera mainCamera;
    public float borderBuffer = 50f; // Buffer in pixels from the edges of the screen
    private Vector3 lastFewVelocity;
    private float startScale = 1f;
    private DraggableItem file;
    private Vector3 fileOffset;

    public Animator myAnim;
    bool animationLocked = false;
    void Start()
    {
    	myAnim = GetComponent<Animator>();
    	lastFewVelocity = new Vector3(0, 0, 0);
        fileOffset = new Vector3(0, 0, 0);
        startScale = transform.localScale.x;
        DraggableItem.onPickUp += PickUpFile;
        DraggableItem.onDrop += DropFile;
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
        Vector3 mouseWorldPositionOffset = mouseWorldPosition + new Vector3(-0.1f, -0.3f, 0.0f);

    	Vector3 oldPos = transform.position;

        if (!animationLocked)
        {
            transform.position = new Vector3(mouseWorldPositionOffset.x * 0.03f + transform.position.x * 0.97f, mouseWorldPositionOffset.y * 0.03f + transform.position.y * 0.97f, -5f);
            if (file != null)
            {
                Vector3 newFilePos = transform.position + fileOffset;
                newFilePos.z = -6f;
                file.transform.position = newFilePos;
                file.transform.localScale = file.transform.localScale * 0.95f + new Vector3(1, 1, 1) * 0.05f;
            }
        }


        Vector3 diff = transform.position - oldPos;
    	
    	updateVelocityAndAnim(diff);
    }

    void PickUpFile(DraggableItem newFile)
    {
        file = newFile;
        file.transform.position = new Vector3(0, 0, -99f); // temporarily hide this
        file.transform.localScale = new Vector3(0.3f, 0.3f, 1);
        SwitchState("pickup");
    }

    public void DropFile(DraggableItem newFile)
    {
        file.transform.position = transform.position + fileOffset;
        file.transform.localScale = new Vector3(1, 1, 1);
        // file.DropFile();
        file = null;
    }

    private void updateVelocityAndAnim(Vector3 diff) {
        lastFewVelocity = lastFewVelocity * 0.95f + diff * 0.05f;
        float xVel = lastFewVelocity.x;
        float flyAngle;
        if (Mathf.Abs(xVel) < 0.0001f)
        {
            flyAngle = 0;
        } else
        {
            flyAngle = Mathf.Atan2(lastFewVelocity.y, xVel);
        }
        float flyAngleLeftRight = flyAngle - 1.571f;
        if (flyAngleLeftRight < -3.1415f)
        {
            flyAngleLeftRight += 6.283f;
        }

        Vector3 currentRotation = transform.eulerAngles;
        currentRotation.z = currentRotation.z * 0.9f;
        if (lastFewVelocity.magnitude < 0.01f) {
            // Add 10 degrees to the Z-axis for clockwise rotation
            currentRotation.z = 0;
            transform.localScale = new Vector3(startScale, startScale, 1);
            if (file != null)
            {
                fileOffset = new Vector3(0.15f, 0.4f, 0);
                SwitchState("bubble");
            }
            else
            {
                SwitchState("idle");
            }
        } else if (file == null) {
            SwitchState("idle");
        }
        else if (lastFewVelocity.y < 0) {
            // flying down (I think)
            if (flyAngleLeftRight < 0)
            {
                // Flying right
                transform.localScale = new Vector3(startScale, startScale, 1);
                currentRotation.z = (flyAngleLeftRight + 1.57f) * 25f;
                fileOffset = new Vector3(0.05f, 0, 0);
            }
            else
            {
                transform.localScale = new Vector3(-startScale, startScale, 1);
                currentRotation.z = (flyAngleLeftRight - 1.57f) * 25f;
                fileOffset = new Vector3(-0.05f, 0, 0);
            }
            SwitchState("flyup");
        } else if (lastFewVelocity.y > 0) {
            // flying up (I think)
            if (flyAngleLeftRight < 0)
            {
                transform.localScale = new Vector3(startScale, startScale, 1);
                currentRotation.z = (flyAngleLeftRight + 1.57f) * 25f;
                fileOffset = new Vector3(0.065f, 0.066f, 0);
            }
            else
            {
                transform.localScale = new Vector3(-startScale, startScale, 1);
                currentRotation.z = (flyAngleLeftRight - 1.57f) * 25f;
                fileOffset = new Vector3(-0.065f, 0.066f, 0);
            }
            SwitchState("flydown");
        }
        transform.eulerAngles = currentRotation;

    }

    private void SwitchState(string nextState) {
        if (animationLocked)
        {
            // We're playing an uninterruptible animation
            return;
        }

        switch (nextState)
        {
            case "bubble":
                myAnim.Play("ang_idle_bubble");
                break;
            case "idle":
                myAnim.Play("ang_idle");
                break;
            case "flyup":
    			myAnim.Play("ang_fly_up");
                break;
            case "flydown":
    			myAnim.Play("ang_fly_down");
                break;
            case "slap":
                myAnim.Play("ang_slap");
                animationLocked = true;
                break;
            case "pickup":
                myAnim.Play("ang_pickup");
                animationLocked = true;
                break;
            case "dunk":
                myAnim.Play("ang_dunk");
                animationLocked = true;
                break;
            default:
    			myAnim.Play("ang_idle");
                break;
        }

    }

    public void unlockAnimation()
    {
        animationLocked = false;
    }
}