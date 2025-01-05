using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour
{
    public GameObject gameStateManager;
    // Vector3 mousePositionOffset;
    public int folderIdx;
    private DropFolder currentDropArea;
    private Vector3 startPos;
    public delegate void OnPickUp(DraggableItem file);
    public static event OnPickUp onPickUp;
    public delegate void OnDrop(DraggableItem file);
    public static event OnDrop onDrop;


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
        // Fire event to angel to have her pick up file
        onPickUp?.Invoke(this);
    }

    private void OnMouseUp()
    {
        onDrop?.Invoke(this);

        DropFile();
    }

    public void DropFile()
    {
        transform.position = GetMouseWorldPosition(0);
        if (currentDropArea != null)
        {
            if (folderIdx == currentDropArea.folderIdx)
            {
                if (gameStateManager != null)
                {
                    gameStateManager.GetComponent<GameStateManager>().IncreaseFileDropped();
                }
                else
                {
                    Debug.Log("ERROR: Set Game State Manager on file! File: " + name);
                }
                Destroy(gameObject);
            }
            else
            {
                StartCoroutine(InterpolateOverTime(transform.position, startPos, 0.36f));
            }
        } else
        {
            StartCoroutine(InterpolateOverTime(transform.position, startPos, 0.36f));
        }
    }


    private void OnMouseDrag()
    {
        // transform.position = GetMouseWorldPosition();
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
        from.z = -6;

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
