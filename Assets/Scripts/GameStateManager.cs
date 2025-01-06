using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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
    public GameObject filesWave0;
    public GameObject filesWave1;
    public GameObject filesWave2;
    public Timer timer;
    public Alert alert;

    private List<Vector3> originalPositions;
    private List<Vector3> leftPositions;

    private GameObject firewallMinigamePrefab;
    private GameObject firewallInstance;
    private GameObject popupMinigamePrefab;
    private AudioSource audioSource;

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
        popupMinigamePrefab = Resources.Load<GameObject>("Prefabs/MinigameHandlerPopup");
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

        filesWave0.SetActive(true);
        filesWave1.SetActive(false);
        filesWave2.SetActive(false);

        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.5f;
    }

    public void Update()
    {
        if (!gameOverTriggered)
        {
            if (timer.isGameOver())
            {
                // Trigger game over (out of time)
                Debug.Log("GameOver: Out of time");
                StartCoroutine(GameOver("GameOver", 0.5f));      
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
        int filesLeft = -1;

        // This is processed before the file object is destroyed and thus never reaches 0
        if (1 >= filesWave0.transform.childCount && currentPhase == GamePhase.BEGINNING)
        {
            Debug.Log("Entering POPUP Phase");
            currentPhase = GamePhase.POPUP;
            alert.TriggerAlert("popup");
        }
        else if (1 >= filesWave1.transform.childCount && currentPhase == GamePhase.POPUP)
        {
            Debug.Log("Entering FIREWALL Phase");
            currentPhase = GamePhase.FIREWALL;
            alert.TriggerAlert("firewall");
        }
        else if (1 >= filesWave2.transform.childCount && currentPhase == GamePhase.FIREWALL)
        {
            // trigger win condition
            Debug.Log("You win!");
            timer.SetWon(); // so it doesn't trigger a lose. too much effort to do the same on the firewall
            Invoke("Win", 1f);
        }
        
        switch (currentPhase)
        {
            case GamePhase.BEGINNING:
                filesLeft = filesWave0.transform.childCount - 1;
                break;
            case GamePhase.POPUP:
                filesLeft = filesWave1.transform.childCount - 1;
                break;
            case GamePhase.FIREWALL:
                filesLeft = filesWave2.transform.childCount - 1;
                break;
            default:
                filesLeft = -1;
                break;
        }
        filesLeftText.GetComponent<TMP_Text>().text = "Files Left: " + filesLeft.ToString();
    }

    public void AddMinigame() {
        if (currentPhase == GamePhase.POPUP) {
            AddPopupMinigame();
            filesWave1.SetActive(true);
        }
        else if (currentPhase == GamePhase.FIREWALL) {
            StartCoroutine(InterpolateOverTime(folder1, FOLDER_1_START_POSITION, FOLDER_1_LEFT_POSITION, .5f));
            StartCoroutine(InterpolateOverTime(folder2, FOLDER_2_START_POSITION, FOLDER_2_LEFT_POSITION, .5f));
            StartCoroutine(InterpolateOverTime(folder3, FOLDER_3_START_POSITION, FOLDER_3_LEFT_POSITION, .5f));
            StartCoroutine(InterpolateOverTime(folder4, FOLDER_4_START_POSITION, FOLDER_4_LEFT_POSITION, .5f));
            Invoke("AddFirewallMinigame", .5f);
            filesWave2.SetActive(true);
        }
    }

    void AddPopupMinigame() {
        if (popupMinigamePrefab != null) {
            Instantiate(popupMinigamePrefab);
        }
    }

    void AddFirewallMinigame() {
        if (firewallMinigamePrefab != null) {
            firewallInstance = Instantiate(firewallMinigamePrefab, new Vector3(5.3f, -1.6f, -1f), Quaternion.identity);
        }
    }

    void Win() {
        SceneManager.LoadScene("Credits");
    }

    public void PlayErrorSound() {
        audioSource.Play();
    }

}
