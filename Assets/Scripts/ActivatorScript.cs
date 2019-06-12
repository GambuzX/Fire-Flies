using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatorScript : MonoBehaviour
{
    //bool active = false;
    //GameObject note;

    private CircleCollider2D outerRing, middleRing, innerRing;
    public float outerRingRadius, middleRingRadius, innerRingRadius;

    private List<GameObject> caughtNotes = new List<GameObject>();

    public GameObject note;

    // Start is called before the first frame update
    void Start()
    {
        outerRing = this.transform.GetChild(0).GetComponent<CircleCollider2D>();
        middleRing = this.transform.GetChild(1).GetComponent<CircleCollider2D>();
        innerRing = this.transform.GetChild(2).GetComponent<CircleCollider2D>();
        outerRingRadius = outerRing.radius*2;
        middleRingRadius = middleRing.radius*2;
        innerRingRadius = innerRing.radius*2;

        caughtNotes.Add(Instantiate(note, note.transform.position, Quaternion.identity));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            handleKeyNotes();
            Debug.Log(caughtNotes.Count);
        }
    }
    
    /*private void OnTriggerEnter2D(Collider2D col) {

        //active = true;
        if (col.gameObject.tag == "Note")
            caughtNotes.Add(col.gameObject);
        Debug.Log("hello");

    }*/

    /*private void OnTriggerExit2D(Collider2D col)
    {
        caughtNotes.Remove(col.gameObject);
    }*/

    private void handleKeyNotes()
    {
        for (int i = caughtNotes.Count-1; i >= 0; i--)
        {
            if (caughtNotes[i].transform.localScale.x == 0)
            {
                Destroy(caughtNotes[i].gameObject);
                caughtNotes.RemoveAt(i);
            }
            else if (inOuterZone(caughtNotes[i])) Debug.Log("outer zone");
            else if (inInnerZone(caughtNotes[i])) Debug.Log("inner zone");
        }
    }

    private bool inOuterZone(GameObject obj)
    {
        return (obj.transform.localScale.x < outerRingRadius && obj.transform.localScale.x > middleRingRadius);
    }

    private bool inInnerZone(GameObject obj)
    {
        return (obj.transform.localScale.x < middleRingRadius && obj.transform.localScale.x > innerRingRadius);
    }



}


