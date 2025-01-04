using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FirewallMinigame : MonoBehaviour
{
    public enum FirewallHealth {Healthy, Warning, Destroyed};

    public AudioClip fireLoop;
    public AudioClip flameUp;
    public AudioClip burnUp;

    private const int TOTAL_BRICK_COUNT = 23;

    private bool active = false;
    private float currScale = 0.0f;
    private Coroutine enterCoroutine = null;

    private Grid brickGrid;
    private GameObject brickPrefab;
    private List<GameObject> bricks = new List<GameObject>();
    private FirewallBG firewallBG;
    private AudioSource audioSourceLoop;
    private AudioSource audioSourceSingle;

    private float timer;
    private float randomInterval;
    private float minTimeInterval = 0.8f;
    private float maxTimeInterval = 1.6f;
    private const float MIN_TIME_INTERVAL_LOWER_LIMIT = 0.2f;
    private int level = 0;
    private int levelThresholdCount = 0;
    private const int LEVEL_THRESHOLD = 4;

    private float health = 0.0f;

    private FirewallHealth firewallHealth;

    // Start is called before the first frame update
    void Start()
    {
        // transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);

        timer = 0.0f;
        randomInterval = Random.Range(minTimeInterval, maxTimeInterval);

        firewallHealth = FirewallHealth.Healthy;

        brickGrid = GameObject.Find("BrickGrid").GetComponent<Grid>();
        brickPrefab = Resources.Load<GameObject>("Prefabs/FirewallBrick");

        firewallBG = GameObject.Find("FirewallBG").GetComponent<FirewallBG>();

        audioSourceLoop = GetComponents<AudioSource>()[0];
        audioSourceSingle = GetComponents<AudioSource>()[1];

        InstantiateBricks();

        // enterCoroutine = StartCoroutine(IncreaseSize());
    }

    // Update is called once per frame
    void Update()
    {
        // periodically degrade state of a random brick
        if (timer >= randomInterval) {
            timer = 0.0f;
            randomInterval = Random.Range(minTimeInterval, maxTimeInterval);

            DegradeBrickAndCheckStatus();
            
            if (levelThresholdCount >= LEVEL_THRESHOLD) {
                IncreaseLevel();
            }
            else {
                levelThresholdCount += 1;
            }
        }
        else {
            timer += Time.deltaTime;
        }
    }

    void DegradeBrickAndCheckStatus() {
        int randomBrickIndex = Random.Range(0, TOTAL_BRICK_COUNT - 1);
        GameObject randomBrick = bricks[randomBrickIndex];
        randomBrick.GetComponent<FirewallBrick>().DegradeState();

        UpdateHealth();
    }

    public void UpdateHealth() {
        int state1Count = 0;
        int state2Count = 0;
        int destroyedBrickCount = 0;
        foreach(GameObject brick in bricks) {
            if (brick.GetComponent<FirewallBrick>().GetHealthState() == FirewallBrick.HealthState.State1) {
                state1Count += 1;
            }
            if (brick.GetComponent<FirewallBrick>().GetHealthState() == FirewallBrick.HealthState.State2) {
                state2Count += 1;
            }
            if (brick.GetComponent<FirewallBrick>().GetHealthState() == FirewallBrick.HealthState.Destroyed) {
                destroyedBrickCount += 1;
            }
        }
        // a function measuring the health https://www.desmos.com/calculator/02gyistj1v
        health = state1Count * 1f + state2Count * 2f + destroyedBrickCount * 4f;
        // Debug.Log("Firewall health: " + health.ToString());
        
        if (health < 15f) {
            audioSourceLoop.Stop();

            firewallHealth = FirewallHealth.Healthy;
            firewallBG.Healthy();
        }
        else if (health < 40f) {
            if (firewallHealth == FirewallHealth.Healthy) {
                audioSourceSingle.clip = flameUp;
                audioSourceSingle.Play();

                audioSourceLoop.clip = fireLoop;
                audioSourceLoop.Play();
            }
            
            firewallHealth = FirewallHealth.Warning;
            firewallBG.Warning();
        }
        else {
            if (firewallHealth == FirewallHealth.Healthy || firewallHealth == FirewallHealth.Warning) {
                audioSourceSingle.clip = burnUp;
                audioSourceSingle.Play();

                audioSourceLoop.Stop();
            }
            firewallHealth = FirewallHealth.Destroyed;
            firewallBG.Destroyed();
            foreach(GameObject brick in bricks) {
                brick.GetComponent<FirewallBrick>().SetHealthState(FirewallBrick.HealthState.Destroyed);
            }

            // Trigger game over
        }
    }

    void IncreaseLevel() {
        level += 1;
        levelThresholdCount = 0;

        // cap it at 0.2 seconds 
        if (minTimeInterval < MIN_TIME_INTERVAL_LOWER_LIMIT) {
            // Debug.Log("FIREWALL LEVEL CAPPED!");
            return;
        }
        
        // degrade bricks faster with each level
        minTimeInterval *= 0.9f;
        maxTimeInterval *= 0.9f;
    }

    void InstantiateBricks() {
        float horizontalOffset = (brickGrid.cellSize.x + brickGrid.cellGap.x) / 2f;

        for (int i = 0; i < 5; i++) {
            // 5 tiles per even row
            if (i % 2 == 0) {
                for (int j = 0; j < 5; j++) {
                    Vector3 brickPosition = brickGrid.CellToWorld(new Vector3Int(j, i));
                    GameObject brick = Instantiate(brickPrefab, brickPosition, Quaternion.identity);
                    brick.GetComponent<FirewallBrick>().firewallMinigame = GetComponent<FirewallMinigame>();
                    bricks.Add(brick);
                }
            }
            // 4 tiles per odd row
            else {
                for (int j = 0; j < 4; j++) {
                    Vector3 brickPosition = brickGrid.CellToWorld(new Vector3Int(j, i));
                    GameObject brick = Instantiate(brickPrefab, new Vector3(brickPosition.x + horizontalOffset, brickPosition.y, 0.0f), Quaternion.identity);
                    brick.GetComponent<FirewallBrick>().firewallMinigame = GetComponent<FirewallMinigame>();
                    bricks.Add(brick);
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
