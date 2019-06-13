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

    private float[] spawnTimes = { 1.5f, 3.5f, 5f, 7f, 8.5f, 10.5f, 13f, 14.4f, 17f, 19.7f, 20.6f, 21.5f, 22.8f, 25.5f, 27.3f, 28.2f, 29f, 31.2f, 33.8f, 34.8f, 35.7f, 37f, 39.7f, 41.5f, 42.5f, 44.1f, 46.8f, 48.2f, 49.5f, 50.4f, 51.3f, 52.6f, 53.9f, 55.7f, 56.6f, 58.3f, 59.7f, 61f, 62.5f, 63.3f, 64.2f, 65.5f, 66.8f, 68.8f, 85f, 85.5f, 86.7f, 88.1f, 89f, 90f, 91.2f, 92.1f, 93f, 93.8f, 94.5f, 95.2f, 96.5f, 97.9f, 98.7f, 99.65f, 101f, 102.3f, 103.65f, 105.4f, 106.7f, 108.1f, 109.4f, 111.7f, 112.1f, 114f, 115.2f, 116.5f, 117.8f, 119.6f, 121f, 122.3f, 123.550f, 140.5f, 141f, 142f, 143f, 143.8f, 144.8f, 145.5f, 146.3f, 147.5f, 148.5f, 149.4f, 150f, 150.7f, 151.5f, 152f, 152.7f, 153.4f, 154.2f, 154.8f, 155.2f, 155.6f, 156f, 156.5f, 157f, 158f, 159f, 159.8f, 160.5f, 161.2f, 161.9f, 162.3f, 163f, 164f, 164.9f, 165.5f, 166.3f, 167f, 167.6f, 169f, 169.8f, 170.3f, 170.8f, 171.6f, 172f, 172.5f, 173.4f, 174.5f, 175.1f, 176f, 176.5f, 177.4f, 178f, 178.8f, 179.6f, 180.4f, 181.1f, 181.75f, 183.1f, 184f, 184.5f, 185f, 186f, 186.4f, 187.3f, 188f, 188.9f, 190.25f, 190.7f, 191f, 191.6f, 192f, 193f, 193.8f, 194.2f, 195.4f, 196f, 196.7f, 197.4f, 198.6f, 199.2f };
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

        playTransition(69);
        playTransition(124);

    }

    // Update is called once per frame
    void Update()
    {
        if (!notesLock && Input.GetKeyDown(KeyCode.Space))
        {
            handleKeyNotes();
        }

        if (currentNote < spawnTimes.Length && Time.time - initialTime > spawnTimes[currentNote] - 2.5)
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
        Invoke("endTransition", time+17);
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
                outerRingAnimator.SetTrigger("flash_yellow");
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

}


