// This is responsible for zapping any floaters that collide with the zapper box.
// We may want to give floaters a smaller collider in the middle that we check.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZapFloater : MonoBehaviour
{
    // We'll probably need this to update the score.
    private GameController gameController;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FloaterCollider"))
        {
            // Don't destroy here - instead, tell the GameController to do this, so that it
            // can also unenroll the floater from its list.
            gameController.DestroyFloater(other.transform.parent.gameObject, "zapped");
            // Destroy(other.transform.parent.gameObject);
            /*
            if (explosion != null)
            {
                Instantiate(explosion, transform.position, transform.rotation);
            }
            gameController.AddScore(scoreValue);
            if (other.CompareTag("Player"))
            {
                Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
                gameController.GameOver();
            }
            */
        }
    }

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
    }
}
