using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.SearchService;
using UnityEditorInternal;
using UnityEngine.SceneManagement;
using System.Diagnostics.Tracing;

public class GameStateManager : MonoBehaviour
{
    public const int POPUP_THRESHOLD = 3;
    public const int FIREWALL_THRESHOLD = 6;
    public const int WIN_THRESHOLD = 10;

    public int fileDroppedCount = 0;

    public GameObject filesLeftText;
    public GameObject folder1;
    public GameObject folder2;
    public GameObject folder3;
    public GameObject folder4;
    public List<GameObject> files;
    public Timer timer;

    private List<Vector3> originalPositions;
    private List<Vector3> leftPositions;

    private GameObject firewallMinigamePrefab;
    private GameObject firewallInstance;
    private GameObject popupMinigamePrefab;

    enum GamePhase { BEGINNING, POPUP, FIREWALL }
    private GamePhase currentPhase = GamePhase.BEGINNING;
    

    private Vector3 FOLDER_1_START_POSITION = new Vector3(-2f, 1.5f, -0.5f);
    private Vector3 FOLDER_2_START_POSITION = new Vector3(2f, 1.5f, -0.5f);
    private Vector3 FOLDER_3_START_POSITION = new Vector3(-2f, -1.5f, -0.5f);
    private Vector3 FOLDER_4_START_POSITION = new Vector3(2f, -1.5f, -0.5f);
    
    private Vector3 FOLDER_1_LEFT_POSITION = new Vector3(-4.4f, 1.5f, -0.5f);
    private Vector3 FOLDER_2_LEFT_POSITION = new Vector3(-2f, 1.5f, -0.5f);
    private Vector3 FOLDER_3_LEFT_POSITION = new Vector3(-4.4f, -1.5f, -0.5f);
    private Vector3 FOLDER_4_LEFT_POSITION = new Vector3(-2f, -1.5f, -0.5f);

    private bool movingLeft = false;
    private bool gameOverTriggered = false;

    void Start()
    {
        firewallMinigamePrefab = Resources.Load<GameObject>("Prefabs/FirewallMinigame");
        firewallInstance = null;
        int filesLeft = WIN_THRESHOLD - fileDroppedCount;
        filesLeftText.GetComponent<TMP_Text>().text = "Files Left: " + filesLeft.ToString();

        if (folder1 != null) {
            folder1.transform.position = FOLDER_1_START_POSITION;
        }
        if (folder2 != null) {
            folder2.transform.position = FOLDER_2_START_POSITION;
        }
        if (folder3 != null) {
            folder3.transform.position = FOLDER_3_START_POSITION;
        }
        if (folder4 != null) {
            folder4.transform.position = FOLDER_4_START_POSITION;
        }

        originalPositions = new List<Vector3>();
        leftPositions = new List<Vector3>();
        
        foreach (var aFile in files) 
        {
            GameObject file = (GameObject) aFile;

            originalPositions.Add(file.transform.position);

            var oldX = file.transform.position.x;
            var newX = (oldX - -8f) * 0.6f - 8f;
            leftPositions.Add(new Vector3(newX, file.transform.position.y, file.transform.position.z));
        }
    }

    public void Update()
    {
        if (!gameOverTriggered)
        {
            if (timer.isGameOver())
            {
                // Trigger game over (out of time)
                Debug.Log("GameOver: Out of time");
                StartCoroutine(GameOver("GameOver", 5f));      
                gameOverTriggered = true;    
            }
            else if (firewallInstance && firewallInstance.GetComponentInChildren<FirewallMinigame>().isGameOver())
            {
                // Trigger game over (destroyed firewall)
                Debug.Log("GameOver: Firewall destroyed");
                StartCoroutine(GameOver("GameOver", 5f));
                gameOverTriggered = true;
            }
        }
    }

    private IEnumerator GameOver(string scene, float transitionDelay)
    {
        float elapsedTime = 0f;

        while (elapsedTime < transitionDelay)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        SceneManager.LoadScene(scene);
    }

    private IEnumerator InterpolateOverTime(GameObject obj, Vector3 from, Vector3 to, float time)
    {
        float elapsedTime = 0f;

        while (elapsedTime < time)
        {
            float t = elapsedTime / time; // Normalized time (0 to 1)
            t = 1 - Mathf.Pow(1 - t, 3);

            obj.transform.position = Vector3.Lerp(from, to, t);
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Ensure the final position is set exactly
        obj.transform.position = to;
    }

    public void IncreaseFileDropped() {
        fileDroppedCount += 1;
        int filesLeft = WIN_THRESHOLD - fileDroppedCount;
        filesLeftText.GetComponent<TMP_Text>().text = "Files Left: " + filesLeft.ToString();
        if (fileDroppedCount >= WIN_THRESHOLD) {
            // trigger win state
        }
        else if (fileDroppedCount >= FIREWALL_THRESHOLD) {
            if (currentPhase != GamePhase.FIREWALL) {
                currentPhase = GamePhase.FIREWALL;

                for (int i = 0; i < files.Count; i++) {
                    if (files[i] == null) {
                        // some files are destroyed, continue
                        continue;
                    }
                    GameObject file = files[i];

                    Vector3 originalPosition = originalPositions[i];
                    Vector3 leftPosition = leftPositions[i];

                    StartCoroutine(InterpolateOverTime(file, originalPosition, leftPosition, .5f));
                }
                StartCoroutine(InterpolateOverTime(folder1, FOLDER_1_START_POSITION, FOLDER_1_LEFT_POSITION, .5f));
                StartCoroutine(InterpolateOverTime(folder2, FOLDER_2_START_POSITION, FOLDER_2_LEFT_POSITION, .5f));
                StartCoroutine(InterpolateOverTime(folder3, FOLDER_3_START_POSITION, FOLDER_3_LEFT_POSITION, .5f));
                StartCoroutine(InterpolateOverTime(folder4, FOLDER_4_START_POSITION, FOLDER_4_LEFT_POSITION, .5f));
                Invoke("AddFirewallMinigame", 1f);

            }
        }
        else if (fileDroppedCount >= POPUP_THRESHOLD) {
            if (currentPhase != GamePhase.POPUP) {
                currentPhase = GamePhase.POPUP;
                AddPopupMinigame();
            }
        }
    }

    void AddPopupMinigame() {
        if (popupMinigamePrefab != null) {
            Instantiate(popupMinigamePrefab);
        }
    }

    void AddFirewallMinigame() {
        if (firewallMinigamePrefab != null) {
            firewallInstance = Instantiate(firewallMinigamePrefab, new Vector3(5.5f, -2f, -1f), Quaternion.identity);
        }
    }

}
