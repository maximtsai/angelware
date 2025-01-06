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
    private Vector3 goalPosition;

    public Animator myAnim;
    bool animationLocked = false; // playing animation and can't move
    bool flyingLocked = false; // started flying so won't change animation for a moment
    string lastState = "idle";

    public delegate void OnClose(GameObject obj);
    public static event OnClose onClose;

    void Start()
    {
    	myAnim = GetComponent<Animator>();
    	lastFewVelocity = new Vector3(0, 0, 0);
        goalPosition = transform.position;
        fileOffset = new Vector3(0, 0, 0);
        startScale = transform.localScale.x;
        DraggableItem.onPickUp += PickUpFile;
        DraggableItem.onDrop += DropFile;

        Popup.onClose += ClosePopup;
    }

    void FixedUpdate()
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


        goalPosition = new Vector3(mouseWorldPositionOffset.x * 0.15f + transform.position.x * 0.85f, mouseWorldPositionOffset.y * 0.15f + transform.position.y * 0.85f, -5f);

    }

    private void Update()
    {
        Vector3 oldPos = transform.position;

        if (!animationLocked)
        {
            transform.position = new Vector3(goalPosition.x * 0.5f + transform.position.x * 0.5f, goalPosition.y * 0.5f + transform.position.y * 0.5f, -5f);
            if (file != null)
            {
                Vector3 newFilePos = transform.position + fileOffset;
                newFilePos.z = -6f;
                file.transform.position = newFilePos;
                file.transform.localScale = file.transform.localScale * 0.9f + new Vector3(1, 1, 1) * 0.1f;
            }
        } else
        {
            transform.position = new Vector3(goalPosition.x * 0.013f + transform.position.x * 0.987f, goalPosition.y * 0.013f + transform.position.y * 0.987f, -5f);
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
        animationLocked = false;
        SwitchState("idle");
        file.transform.position = transform.position + fileOffset;
        file.transform.localScale = new Vector3(1, 1, 1);
        // file.DropFile();
        file = null;
    }

    void ClosePopup(GameObject obj)
    {
        Vector3 closePos = obj.transform.position;
        closePos.x += obj.GetComponent<BoxCollider2D>().offset.x * 0.85f * obj.transform.parent.transform.localScale.x;
        closePos.y += obj.GetComponent<BoxCollider2D>().offset.y * 0.85f * obj.transform.parent.transform.localScale.y;
        Debug.Log(obj.GetComponent<BoxCollider2D>().offset);
        Vector3 moveToPos = new Vector3(closePos.x, closePos.y, -5f);
        transform.position = moveToPos;
        SwitchState("slap");

    }

    private void updateVelocityAndAnim(Vector3 diff) {
        lastFewVelocity = lastFewVelocity * 0.96f + diff * 0.04f;
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
        if (!flyingLocked && lastFewVelocity.magnitude < 0.02f && diff.magnitude < 0.025f) {
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
        } else if (lastFewVelocity.y < 0) {
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
        if (flyingLocked && nextState == "bubble")
        {
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
                if (lastState != nextState)
                {
                    flyingLocked = true;
                    Invoke("unlockFlying", 0.6f);
                }
                break;
            case "flydown":
    			myAnim.Play("ang_fly_down");
                if (lastState != nextState)
                {
                    flyingLocked = true;
                    Invoke("unlockFlying", 0.6f);
                }
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
        lastState = nextState;

    }

    private void unlockFlying()
    {
        flyingLocked = false;

    }
    public void unlockAnimation()
    {
        animationLocked = false;
    }
}