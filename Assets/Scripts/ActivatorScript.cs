using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatorScript : MonoBehaviour
{
    public KeyCode key;
    bool active = false;
    GameObject note;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(key) && active)
            Destroy(note);
    }

    private void OnTriggerEnter2D(Collider2D col) { 
    
        active = true;
        if (col.gameObject.tag == "Note")
            note = col.gameObject;
        }
    private void OnTriggerExit2D(Collider2D col)
    {
        active = false;
    }

    


}


