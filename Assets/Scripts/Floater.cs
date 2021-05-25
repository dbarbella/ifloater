using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    public float minFloaterLen = 0.5f;
    public float maxFloaterLen = 1.5f;

    private Rigidbody rb;

    // Print debug information about floater if true
    public bool debugFloater;

    // Amount to bump up on a blink;
    public float blinkThrust = 750f;

    // variation factor in how big the bump is.
    // Randomly increase or decrease by up to this percent.
    public float blinkThrustChaos = 0.5f;
    public float irisJerkChaos = 0.5f;

    // We may eventually want this to depend on the floater's y value,
    // but a constant is fine for now.
    public float blinkReactionDelay = 0.1f;


    // Start is called before the first frame update
    void Start()
    {
        // When a floater is spawned, randomly alter the:
        // y of its scale, between .5 and 1.5 (or whatever)
        // z of its rotation, between -180 and 180
        transform.localScale = new Vector3(transform.localScale.x, UnityEngine.Random.Range(minFloaterLen, maxFloaterLen), transform.localScale.z);
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, UnityEngine.Random.Range(-180.0f, 180.0f));

        rb = GetComponent<Rigidbody>();
    }


    // Update is called once per frame
    void Update()
    {
        // If we collide with bottom of top lid, and it's going up, we want to go up.
    }

    public void reactToBlink()
    {
        if (debugFloater)
        {
            print("Floater reacting to blink...");
        }
        Invoke("addUpBlinkForce", blinkReactionDelay);
    }
    
    // We need to be able to apply a force to all of the floaters. Let's start by seeing if we can "bump" the floater
    // We do want this to use real physics.
    void addUpBlinkForce()
    {
        // This needs to be "absolute up" rather than relative to the object.
        // Hence Vector3.up.
        // This is what we want to do, but we want it to happen kind of
        // along with the blink up.
        // Give this a smidge of randomness, maybe.
        if (debugFloater)
        {
            print("Adding up blink force...");
        }
        rb.AddForce(Vector3.up * blinkThrust * (1+(Random.Range(-blinkThrustChaos, blinkThrustChaos))));
    }

    // Don't like that this is public and the other add i
    public void reactToIrisMovement(float xForce, float yForce)
    {
        if (debugFloater)
        {
            print("Applying iris movement force to floater...");
        }
        // It's possible that this Vector3 should be built in
        // GameController.
        float randomIrisThrustfactor;
        randomIrisThrustfactor = (1 + (Random.Range(-irisJerkChaos, irisJerkChaos)));
        rb.AddForce(new Vector3(xForce * randomIrisThrustfactor, yForce * randomIrisThrustfactor, 0.0f));
    }

    // We need to destroy it when it goes out of bounds, or respawn it or something

    // We need to destroy it when it goes in the crosshair and award points.
}
