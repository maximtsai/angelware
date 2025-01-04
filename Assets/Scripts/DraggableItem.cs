using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour
{
    public GameStateManager gameStateManager;
    // Vector3 mousePositionOffset;
    public int folderIdx;
    private DropFolder currentDropArea;
    private Vector3 startPos;

    Transform parentAfterDrag;

    private Vector3 GetMouseWorldPosition(float zIdx = -1f)
    {
        // capture mouse position and return WorldPoint
        Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        newPos.z = zIdx;
        return newPos;
    }

    private void OnMouseDown()
    {
        // capture mouse offset pos
        // mousePositionOffset = gameObject.transform.position - GetMouseWorldPosition();
    }

    private void OnMouseUp()
    {
        transform.position = GetMouseWorldPosition(0);
        if (currentDropArea != null)
        {
            if (folderIdx == currentDropArea.folderIdx)
            {
                if (gameStateManager != null) {
                    gameStateManager.IncreaseFileDropped();
                }
                else {
                    Debug.Log("ERROR: Set Game State Manager on file! File: " + name);
                }
                Destroy(gameObject);
            }
            else
            {
                StartCoroutine(InterpolateOverTime(transform.position, startPos, 0.36f));
            }
        }

    }


    private void OnMouseDrag()
    {
        transform.position = GetMouseWorldPosition();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DropArea"))
        {
            currentDropArea = other.GetComponent<DropFolder>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("DropArea") && currentDropArea == other.GetComponent<DropFolder>())
        {
            currentDropArea = null;
        }
    }

    private IEnumerator InterpolateOverTime(Vector3 from, Vector3 to, float time)
    {
        float elapsedTime = 0f;

        while (elapsedTime < time)
        {
            float t = elapsedTime / time; // Normalized time (0 to 1)
            t = 1 - Mathf.Pow(1 - t, 3);

            transform.position = Vector3.Lerp(from, to, t);
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Ensure the final position is set exactly
        transform.position = to;
    }



    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
