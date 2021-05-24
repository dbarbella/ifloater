using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour
{
    private Rigidbody rb;
    // The speed at which to blink down
    public float blinkDownSpeed;
    // The speed at which to blink up
    public float blinkUpSpeed;
    // How long to remain closed
    public float closedPeriod;
    // We should make sure that a blink has to complete before the next blink? Maybe?
    public float blinkDelay;
    // This is the time we will next be allowed to blink.
    private float nextBlink;
    private float topYPos;
    // This should be based on where the top of the bottom lid is.
    // We can calculate that.
    private float bottomYPos;
    // We may not need this; can use our own transform without it?
    public Transform lidTransform;

    // The current direction and speed of the top lid.
    private Vector3 currentDirection;
    private float currentSpeed;

    // Keep track of the game controller
    public GameController gameController;

    // If this is true, print debug statements about blinking.
    public bool debugBlink = false;

    // Blink needs to know who all of the floaters are, so it can notify them.
    // Actually it probably needs to know who the gameController is, so it
    // can notify that, and it can notify the floaters?

    // Start is called before the first frame update
    void Start()
    {
        //rb = GetComponent<Rigidbody>();
        // This should be automatically calculated; for now, we'll use hardcoded values.
        topYPos = 12.3f;
        bottomYPos = 4.2f;

        currentDirection = Vector3.down;
        currentSpeed = 0.0f;

        //gameController = GameObject.FindWithTag("GameController");

        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        if (gameController == null)
        {
            Debug.Log("Cannot find 'GameController' script.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        lidUpdate();
    }

    // This is responsible for updating the direction of the lid, and then
    // updating its position.
    void lidUpdate()
    {
        // Check to see if the blink button was pressed and it's been long
        // enough since the last blink.
        if (Input.GetButton("Blink") && Time.time > nextBlink)
        {
            // This should probably make a sound of some kind.
            CloseEye();
            gameController.receiveBlinkNotice();
        }

        // If we've reached the bottom of the blink, go back up again.
        // This needs to keep track of how long since the first bottom-touch
        // so that we can pause at the bottom, if we care about that.
        else if (transform.position.y <= bottomYPos)
        {
            OpenEye();
        }

        // If we've reached the top of the blink, stop.
        else if (transform.position.y >= topYPos)
        {
            StopEye();
        }

        // A piece of code that checks the current movement direction and alters the
        // transform's location.
        transform.Translate(currentDirection * Time.deltaTime * currentSpeed);
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, bottomYPos, topYPos), transform.position.z);
    }

    void StopEye()
    {
        if (debugBlink)
        {
            Debug.Log("Stopping eye.....");
        }

        currentDirection = Vector3.down;
        currentSpeed = 0.0f;

        // This requires it to be a physics object, which I do not think we want.
        // rb.velocity = new Vector3(0, -blinkDownSpeed, 0);
    }

    void OpenEye()
    {
        if (debugBlink)
        {
            Debug.Log("Opening eye.....");
        }

        currentDirection = Vector3.up;
        currentSpeed = blinkUpSpeed;
    }

    void CloseEye()
    {
        nextBlink = Time.time + blinkDelay;

        if (debugBlink)
        {
            Debug.Log("Closing eye.....");
        }
        // This should first go from current position to bottomYPos
        // or possibly some fraction of blinkDownSpeed, if we're already partially closed.

        // Instead of directly manipulating the transform, this should manipulate
        // a varaible that the Update method looks at.
        currentDirection = Vector3.down;
        currentSpeed = blinkDownSpeed;

        // This requires it to be a physics object, which I do not think we want.
        // rb.velocity = new Vector3(0, -blinkDownSpeed, 0);
    }
}
