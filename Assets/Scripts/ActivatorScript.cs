using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivatorScript : MonoBehaviour
{
    //bool active = false;
    //GameObject note;

    private CircleCollider2D outerRing, middleRing, innerRing;
    private float outerRingRadius, middleRingRadius, innerRingRadius;

    private List<GameObject> caughtNotes = new List<GameObject>();

    public GameObject note;

    private int score = 0;

    private Text scoreText;
    private Animator outerRingAnimator;

    // Start is called before the first frame update
    void Start()
    {
        outerRing = this.transform.GetChild(0).GetComponent<CircleCollider2D>();
        middleRing = this.transform.GetChild(1).GetComponent<CircleCollider2D>();
        innerRing = this.transform.GetChild(2).GetComponent<CircleCollider2D>();
        outerRingRadius = outerRing.radius*2;
        middleRingRadius = middleRing.radius*2;
        innerRingRadius = innerRing.radius*2;

        this.scoreText = GameObject.Find("Score").GetComponent<Text>();
        scoreText.text = "0";

        InvokeRepeating("spawnNote", 0, 2);

        outerRingAnimator = GameObject.FindGameObjectWithTag("OuterRing").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            handleKeyNotes();
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

    private void spawnNote()
    {
        Color background = new Color(
            Random.Range(0f, 1f),
            Random.Range(0f, 1f),
            Random.Range(0f, 1f),
            0.2f
        );
        GameObject newNote = Instantiate(note, note.transform.position, Quaternion.identity);
        newNote.GetComponent<SpriteRenderer>().color = background;
        caughtNotes.Add(newNote);
    }

    private void handleKeyNotes()
    {
        for (int i = caughtNotes.Count-1; i >= 0; i--)
        {
            if (caughtNotes[i].transform.localScale.x == 0)
            {
                removeNoteAt(i);
            }
            else if (inOuterZone(caughtNotes[i]))
            {
                outerRingAnimator.SetTrigger("flash_white");
                increaseScore(50);
                removeNoteAt(i);
            }
            else if (inInnerZone(caughtNotes[i]))
            {
                outerRingAnimator.SetTrigger("flash_yellow");
                increaseScore(100);
                removeNoteAt(i);
            }
        }
    }

    private void increaseScore(int val)
    {
        score += val;
        scoreText.text = "" + score;
    }

    private void removeNoteAt(int index)
    {
        Destroy(caughtNotes[index].gameObject);
        caughtNotes.RemoveAt(index);
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


