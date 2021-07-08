using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    public Text restartText;

    public bool debugFloaterSpawn;
    public bool debugBlinkMessaging;
    public bool debugIrisMessaging;
    public bool debugIrritation;

    private GameObject newFloater;
    private List<GameObject> livingFloaters = new List<GameObject>();

    private float irritation;
    public float maxIrritation;

    public float irritationReductionOnZap;

    // These values determine how the floaters react to iris movement.
    public float irisJerkMultiplier;
    public float irisJerkExponent;

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
                restartText.text = "Press 'r' to restart";
                restart = true;
                break;
            }
        }
    }

    // Irritation needs to go up over time
    // For now, it just goes up a little at a time, linearly
    // Something also needs to care about maxIrritation.
    IEnumerator AdjustIrritation()
    {
        while (true)
        {
            irritation = irritation + 8.0f;
            if (debugIrritation)
            {
                Debug.Log("Irritation: " + irritation);
            }
            yield return new WaitForSeconds(1);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        irritation = 0;
        restartText.text = "";

        StartCoroutine(SpawnFloaters());
        StartCoroutine(AdjustIrritation());
    }

    // Update is called once per frame
    void Update()
    {
        if (GetIrritation() >= maxIrritation)
        {
            gameOver = true;
            // End the game, report score
        }

        if (restart)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                // For some reason this isn't turning on the lights.
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    public float GetIrritation()
    {
        return irritation;
    }

    public void ReceiveBlinkNotice()
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

    public void ReceiveIrisMovement(float xMovement, float yMovement)
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
        xForce = ConvertIrisMovementForce(xMovement);
        yForce = ConvertIrisMovementForce(yMovement);

        // If we want this to affect all floaters the same way, which I think we do,
        // we should do any calculations here, and then tell them what to do,
        // rather than having each floater do the calculations.
        for (int i = 0; i < livingFloaters.Count; i++)
        {
            livingFloaters[i].GetComponent<Floater>().reactToIrisMovement(xForce, yForce);
        }
    }

    // Convert a force
    float ConvertIrisMovementForce(float inputForce)
    {
        float returnForce;
        int negativeForceValue = 1;

        if (inputForce < 0)
        {
            negativeForceValue = -1;
        }
        returnForce = Mathf.Pow((Mathf.Abs(inputForce) * irisJerkMultiplier), irisJerkExponent) * negativeForceValue;
        return returnForce;
    }

    public void DestroyFloater(GameObject floaterObject, string causeOfDeath)
    {
        // Remove it from our list.
        livingFloaters.Remove(floaterObject);
        // Destroy it.
        Destroy(floaterObject);

        // Adjust the eye if appropriate.
        if (causeOfDeath.Equals("zapped"))
        {
            print("Getting in here - Zapped!");
            irritation = (Mathf.Max(0, irritation - irritationReductionOnZap));

            // Flash the reticle.

            // Increment the score if appropriate.
        }
    }
}
