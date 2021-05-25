// Contacts GameController periodically to adjust the eye color.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustWhiteColor : MonoBehaviour
{
    //float adjustmentFrequency = 0.1;

    float irritation;
    private Renderer myRenderer;
    public GameController gameController;

    public bool debugWhiteColor;

    // Start is called before the first frame update
    void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        if (gameController == null)
        {
            Debug.Log("Cannot find 'GameController' script.");
        }

        myRenderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateWhiteColor();
    }

    void UpdateWhiteColor()
    {
        irritation = gameController.GetIrritation();
        if (debugWhiteColor)
        {
            Debug.Log("Irritation: " + irritation);
        }
        //myRenderer.material.SetColor("_Color", new Color(irritation / 100, 1.0f, 1.0f));
        float greenAndBlue = (1.0f - (irritation / 100f));
        myRenderer.material.SetColor("_Color", new Color(1.0f, greenAndBlue, greenAndBlue));
    }
}
