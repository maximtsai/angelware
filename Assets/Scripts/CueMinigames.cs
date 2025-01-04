using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CueMinigames : MonoBehaviour
{
    public GameObject firewallMinigamePrefab;
    public GameObject popupMinigamePrefab;

    // Start is called before the first frame update
    void Start()
    {
        firewallMinigamePrefab = Resources.Load<GameObject>("Prefabs/FirewallMinigame");
        StartCoroutine(GameFlow());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Do not know if this is the best way to do it!! Here goes nothing!!
    IEnumerator GameFlow() {
        
        yield return new WaitForSeconds(1);

        AddFirewallMinigame();
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
