using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameStateManager : MonoBehaviour
{
    public const int POPUP_THRESHOLD = 3;
    public const int FIREWALL_THRESHOLD = 6;
    public const int WIN_THRESHOLD = 10;

    public int fileDroppedCount = 0;

    public GameObject filesLeftText;

    private GameObject firewallMinigamePrefab;
    private GameObject popupMinigamePrefab;

    enum GamePhase { BEGINNING, POPUP, FIREWALL }
    private GamePhase currentPhase = GamePhase.BEGINNING;

    void Start()
    {
        firewallMinigamePrefab = Resources.Load<GameObject>("Prefabs/FirewallMinigame");
        int filesLeft = WIN_THRESHOLD - fileDroppedCount;
        filesLeftText.GetComponent<TMP_Text>().text = "Files Left: " + filesLeft.ToString();
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
                AddFirewallMinigame();
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
            Instantiate(firewallMinigamePrefab, new Vector3(5.5f, -2f, -1f), Quaternion.identity);
        }
    }
}
