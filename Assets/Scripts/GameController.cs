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
    public bool debugIrisMessaging;

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
        {
            Debug.Log("Game controller received a blink message.");
        }
        // Notify each floater of the blink.
        for (int i = 0; i < livingFloaters.Count; i++)
        {
            //gameController = gameControllerObject.GetComponent<GameController>();
            if (debugBlinkMessaging)
            {
                Debug.Log("Notifying a floater of the blink.");
            }
            livingFloaters[i].GetComponent<Floater>().reactToBlink();
        }
    }

    public void receiveIrisMovement(float xMovement, float yMovement)
    {
        if (debugIrisMessaging)
        {
            Debug.Log("Received message from MoveIris: " + xMovement + ", " + yMovement);
        }

        // The values that are sent to each floater to have them adjust their movement.
        float xForce;
        float yForce;

        // This is clearly not the formula we want; this is just to see if the communication
        // chain is working.
        xForce = convertIrisMovementForce(xMovement);
        yForce = convertIrisMovementForce(yMovement);

        // If we want this to affect all floaters the same way, which I think we do,
        // we should do any calculations here, and then tell them what to do,
        // rather than having each floater do the calculations.
        for (int i = 0; i < livingFloaters.Count; i++)
        {
            livingFloaters[i].GetComponent<Floater>().reactToIrisMovement(xForce, yForce);
        }
    }

    // Convert a force
    float convertIrisMovementForce(float inputForce)
    {
        // These values determine how the floaters react to iris movement.
        float irisJerkMultiplier = 13.0f;
        float irisJerkExponent = 2.0f;
        
        float returnForce;
        int negativeForceValue = 1;

        if (inputForce < 0)
        {
            negativeForceValue = -1;
        }
        returnForce = Mathf.Pow((Mathf.Abs(inputForce) * irisJerkMultiplier), irisJerkExponent) * negativeForceValue;
        print(inputForce + " " + returnForce);
        return returnForce;
    }
}
