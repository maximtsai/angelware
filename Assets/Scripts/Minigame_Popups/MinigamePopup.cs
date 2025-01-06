using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UIElements;

public class MinigamePopup : MonoBehaviour
{
    public List<GameObject> malwarePopupPrefabs;
    public int numMaxPopups = 10;
    public float intervalBetweenSpawns = 5.0f;
    public float chanceToSpawn = 0.50f;

    private float timeElapsed;
    private int numPopupsSpawned;
    private int numPopupsAlive;

    // Start is called before the first frame update
    void Start()
    {
        timeElapsed = 0f;
        numPopupsSpawned = 0;
        numPopupsAlive = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;
        while (timeElapsed > intervalBetweenSpawns)
        {
            // do the spawning logic in here
            float rng = UnityEngine.Random.Range(0f, 1f);
            if (rng <= chanceToSpawn && numPopupsAlive < numMaxPopups)
            {
                // Randomly select a prefab
                // TODO: Update the random selection to match the final list of malware sprites
                // int randomPopupIndex = 8;
                // while (0 == randomPopupIndex % 8 || 6 == randomPopupIndex)
                // {
                //     randomPopupIndex = Mathf.RoundToInt(UnityEngine.Random.Range(2, 17));
                // }
                // string resourceName = "Prefabs/Minigame_Popups/Malware-" + randomPopupIndex.ToString("00");
                
                int randomPopupIndex = UnityEngine.Random.Range(0, malwarePopupPrefabs.Count);
                try
                {
                    // Spawn the prefab into the scene and set its properties
                    Camera c = Camera.main;
                    // GameObject popup = (GameObject) Instantiate(Resources.Load(resourceName));
                    GameObject popup = (GameObject) Instantiate(malwarePopupPrefabs[randomPopupIndex]);
                    Renderer r = popup.GetComponent<Renderer>();
                    popup.gameObject.SetActive(false);
                    popup.GetComponentInChildren<Popup>().minigameController = this;
                    // Bound the popup to within the viewport
                    float x, y;
                    UnityEngine.Vector2 pos;
                    bool valid_spawn_pos = true;
                    do
                    {
                        x = UnityEngine.Random.Range(0f, 1f);
                        y = UnityEngine.Random.Range(0f, 1f);
                        pos = c.ViewportToWorldPoint(new UnityEngine.Vector3(x, y, 0));
                        popup.transform.position = new UnityEngine.Vector3(pos.x, pos.y, -3f - ((float) numPopupsSpawned) / 100f);

                        UnityEngine.Vector2 button_pos = c.WorldToViewportPoint(new UnityEngine.Vector2(r.bounds.max.x, r.bounds.max.y));
                        // Debug.Log(button_pos.x + ", " + button_pos.y);
                        valid_spawn_pos = button_pos.x <= 1.0 && button_pos.y <= 1.0;
                    } while (!valid_spawn_pos);
                    
                    popup.GetComponentInChildren<Popup>().orderHitboxes();

                    popup.gameObject.SetActive(true);
                    numPopupsSpawned = (numPopupsSpawned + 1) % 100; // Clamps the Z-index to the range [-0.99, 0]
                    numPopupsAlive++;
                    // Debug.Log("Total number of popups spawned: " + numPopupsSpawned);
                    // Debug.Log("Number of alive popups: " + numPopupsAlive);
                }
                catch(Exception e)
                {
                    Debug.Log("Failed to instantiate prefab");
                    Debug.Log("Attempted to spawn: " + malwarePopupPrefabs[randomPopupIndex].name);
                    Debug.Log(e);
                }
            }
            timeElapsed -= intervalBetweenSpawns;
        }
    }

    public void KillPopup()
    {
        numPopupsAlive--;
        Debug.Log("Killed active popup. Remaining: " + numPopupsAlive);
        // Reset the number of popups spawned if all popups closed to reset z-indexing
        if (0 == numPopupsAlive)
        {
            numPopupsSpawned = 0;
        }
    }

}
