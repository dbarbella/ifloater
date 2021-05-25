using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveIris : MonoBehaviour
{
    public bool debugLook = false;

    private Vector3 rawMousePos;
    private Vector3 scaledMousePos;

    // We need to define boundaries for the maximum and minimum x and y positions of
    // the iris.
    public float irisXMin;
    public float irisXMax;
    public float irisYMin;
    public float irisYMax;

    private float irisXGap;
    private float irisYGap;

    private float newIrisX;
    private float newIrisY;

    // Used to report movement to gameController.
    private float oldIrisX;
    private float oldIrisY;

    // Keep track of the game controller
    public GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        irisXGap = irisXMax - irisXMin;
        irisYGap = irisYMax - irisYMin;

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
        lookUpdate();
    }

    // This is responsible for making the eye look at the cursor.
    // The eye needs to move to a point proportionate to where in the camera the cursor is.
    // This probably wants the iris-pupil construct to be a separate object.
    // We will need to experiment with how jerky this should be.
    // The bottom-left of the screen or window is at (0, 0).
    // The top-right of the screen or window is at (Screen.width, Screen.height).
    // This needs to probably be able to report changes to the iris position.
    // What we'll do: Tell GameController about the delta?
    void lookUpdate()
    {
        rawMousePos = Input.mousePosition;
        scaledMousePos = scaleMousePos(rawMousePos);

        oldIrisX = transform.position.x;
        oldIrisY = transform.position.y;

        newIrisX = irisXMin + (irisXGap * scaledMousePos.x);
        newIrisY = irisYMin + (irisYGap * scaledMousePos.y);

        transform.position = new Vector3(newIrisX, newIrisY, transform.position.z);

        if (debugLook)
        {
            Debug.Log(rawMousePos);
            Debug.Log(scaledMousePos);
        }

        gameController.receiveIrisMovement(newIrisX - oldIrisX, newIrisY - oldIrisY);
    }

    // We may want to revisit this to make it "strain" more at the edges, but this
    // works for now.
    Vector3 scaleMousePos(Vector3 unscaledMousePos)
    {
        return new Vector3(Mathf.Clamp(unscaledMousePos.x / Screen.width, 0.0f, 1.0f), Mathf.Clamp(unscaledMousePos.y / Screen.height, 0.0f, 1.0f), 0.0f);
    }
}
