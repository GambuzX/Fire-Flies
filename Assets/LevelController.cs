using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{

    private float startTime;
    private float endTime = 200f;
    private bool ended = false;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - startTime > 230) SceneManager.LoadScene(0);

        if (!ended && Time.time - startTime > endTime)
        {

            foreach (Note obj in GameObject.FindObjectsOfType<Note>()) Destroy(obj.gameObject);
            GameObject.Find("outer_ring").GetComponent<SpriteRenderer>().enabled = false;

            ActivatorScript activator = FindObjectOfType<ActivatorScript>().GetComponent<ActivatorScript>();

            GameObject.Find("Score").SetActive(false);

            int finalscore = activator.getScore();

            GameObject.Find("Score Label").GetComponent<Text>().enabled = true;
            GameObject.Find("Finalscore").GetComponent<Text>().enabled = true;
            GameObject.Find("Finalscore").GetComponent<Text>().text = "" + finalscore;

            GameObject.Find("HighscoreLabel").GetComponent<Text>().enabled = true;
            GameObject.Find("Highscore").GetComponent<Text>().enabled = true;

            if (finalscore > PlayerPrefs.GetInt("highscore", 0))
            {
                PlayerPrefs.SetInt("highscore", finalscore);
            } else
            {
                finalscore = PlayerPrefs.GetInt("highscore", 0);
            }
            GameObject.Find("Highscore").GetComponent<Text>().text = "" + finalscore;


            ended = true;
            activator.enabled = false;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(2);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
