using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

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
    private AudioSource musicAudio;

    private List<float> spawnTimes = new List<float>();
    private int currentNote = 0;
    private float initialTime;

    public Color[] noteColors;
    private GameObject outerRingObj;

    private bool notesLock = false;

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

        outerRingObj = GameObject.FindGameObjectWithTag("OuterRing");
        outerRingAnimator = GameObject.FindGameObjectWithTag("OuterRing").GetComponent<Animator>();
        musicAudio = GameObject.FindGameObjectWithTag("GameMusic").GetComponent<AudioSource>();

        initialTime = Time.time;
        readMusicTimes();

        playTransition(70);
        playTransition(125);

    }

    // Update is called once per frame
    void Update()
    {
        if (!notesLock && Input.GetKeyDown(KeyCode.Space))
        {
            handleKeyNotes();
        }

        if (currentNote < spawnTimes.Count && Time.time - initialTime > spawnTimes[currentNote] - 2.5)
        {
            spawnNote();
            currentNote++;
        }
    }

    private void spawnNote()
    {
        GameObject newNote = Instantiate(note, note.transform.position, Quaternion.identity);
        newNote.GetComponent<SpriteRenderer>().color = noteColors[Random.Range(0, noteColors.Length)];
        caughtNotes.Add(newNote);
    }

    private void playTransition(float time)
    {
        Invoke("startTransition", time);
        Invoke("endTransition", time+15);
    }

    private void startTransition()
    {
        outerRingObj.GetComponent<SpriteRenderer>().enabled = false;
        notesLock = true;
    }

    private void endTransition()
    {
        outerRingObj.GetComponent<SpriteRenderer>().enabled = true;
        notesLock = false;
    }

    private void handleKeyNotes()
    {
        bool caught = false;
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
                caught = true;
            }
            else if (inInnerZone(caughtNotes[i]))
            {
                outerRingAnimator.SetTrigger("flash_white");
                increaseScore(100);
                removeNoteAt(i);
                caught = true;
            }
        }
        if(!caught)
        {
            outerRingAnimator.SetTrigger("flash_red");
            increaseScore(-10);
        }
    }

    private void increaseScore(int val)
    {
        score += val;
        scoreText.text = "" + score;
    }

    public int getScore()
    {
        return score;
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

    void readMusicTimes()
    {
        StreamReader inp_stm = new StreamReader("Assets/music_times.txt");

        while (!inp_stm.EndOfStream)
        {
            string inp_ln = inp_stm.ReadLine();
            spawnTimes.Add(float.Parse(inp_ln));
        }

        inp_stm.Close();
    }

}


