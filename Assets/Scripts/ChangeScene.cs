using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour { 
public KeyCode key;


    public void changeToScene(int changeTheScene)
    {
        SceneManager.LoadScene(changeTheScene);
          
    }

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            SceneManager.LoadScene("2ndScreen");

        }
    }

}
   