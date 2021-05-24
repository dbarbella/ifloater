using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{
    // This needs to spawn floaters, and it needs
    // to get messages from Blink that it then conveys to the floaters.
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       

    }

    // This should make the eyelids blink.
    // Ideally, this should not re-find the eyelids every time.
    // This involves closing the upper lid, and as it opens, applying
    // an upward force to all floaters.
    // Don't want to allow blinking again until after a blink is complete.
    void blink_eyes()
    {

    }
}
