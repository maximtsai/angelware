using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirewallMinigame : MonoBehaviour
{
    private bool active = false;
    private float currScale = 0.0f;
    private Coroutine enterCoroutine = null;

    private Grid brickGrid;
    private GameObject brickPrefab;

    // Start is called before the first frame update
    void Start()
    {
        // transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);

        brickGrid = GameObject.Find("BrickGrid").GetComponent<Grid>();
        brickPrefab = Resources.Load<GameObject>("Prefabs/FirewallBrick");
        InstantiateBricks();

        // enterCoroutine = StartCoroutine(IncreaseSize());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InstantiateBricks() {
        float horizontalOffset = (brickGrid.cellSize.x + brickGrid.cellGap.x) / 2f;

        for (int i = 0; i < 5; i++) {
            // 5 tiles per even row
            if (i % 2 == 0) {
                for (int j = 0; j < 5; j++) {
                    Vector3 brickPosition = brickGrid.CellToWorld(new Vector3Int(j, i));
                    Instantiate(brickPrefab, brickPosition, Quaternion.identity);
                }
            }
            // 4 tiles per odd row
            else {
                for (int j = 0; j < 4; j++) {
                    Vector3 brickPosition = brickGrid.CellToWorld(new Vector3Int(j, i));
                    Instantiate(brickPrefab, new Vector3(brickPosition.x + horizontalOffset, brickPosition.y, 0.0f), Quaternion.identity);
                }
            }
        }
    }


    IEnumerator IncreaseSize()
    {
        while (currScale < 1.0f) {
            currScale += 0.1f;
            transform.localScale = new Vector3(currScale, currScale, 0.0f);
            yield return new WaitForSeconds(0.05f);
        }
        active = true;
        enterCoroutine = null;
    }
}
