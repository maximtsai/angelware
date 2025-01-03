using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirewallSandboxScene : MonoBehaviour
{
    public GameObject firewallMinigamePrefab;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("AddFirewallMinigame", 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AddFirewallMinigame() {
        Instantiate(firewallMinigamePrefab);
    }
}
