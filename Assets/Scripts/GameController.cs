using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // This needs to spawn floaters, and it needs
    // to get messages from Blink that it then conveys to the floaters.

    public GameObject floater;
    public float minFloaterSpawnX;
    public float maxFloaterSpawnX;
    public float floaterSpawnY;
    public float floaterSpawnZ;
    //public Vector2 spawnAngles;
    public int hazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;

    public bool gameOver;
    public bool restart;

    public bool debugFloaterSpawn;
    public bool debugBlinkMessaging;

    private GameObject newFloater;
    private List<GameObject> livingFloaters = new List<GameObject>();

    IEnumerator SpawnFloaters()
    {
        if (debugFloaterSpawn)
        { print("Starting spawning."); }
        
        yield return new WaitForSeconds(startWait);
        while (true)
        {
            for (int i = 0; i < hazardCount; i++)
            {
                if (debugFloaterSpawn)
                { print("Making a floater."); }

                // Choose spawn position randomly
                // The size and rotation are chosen randomly by the floater itself, not here.
                Vector3 spawnPosition = new Vector3(Random.Range(minFloaterSpawnX, maxFloaterSpawnX), floaterSpawnY, floaterSpawnZ);
                // Create the floater
                newFloater = Instantiate(floater, spawnPosition, Quaternion.identity);
                // Add the floater to livingFloaters
                livingFloaters.Add(newFloater);
                // Wait to spawn the next one
                yield return new WaitForSeconds(spawnWait);
            }
            yield return new WaitForSeconds(waveWait);

            if (gameOver)
            {
                //restartText.text = "Press 'r' to restart";
                restart = true;
                break;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnFloaters());
    }

    // Update is called once per frame
    void Update()
    {
        // This needs to periodically spawn floaters
        // Check how we spawn asteroids
        // This maybe needs to be a separate spawner script?
    }

    public void receiveBlinkNotice()
    {
        if (debugBlinkMessaging)
        { print("Game controller received a blink message."); }
        // Notify each floater of the blink.
    }
}
