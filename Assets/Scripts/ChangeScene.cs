﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {


    public void changeToScene(int changeTheScene)
    {
        SceneManager.LoadScene(changeTheScene);
          
    }

    void Start()
    {

    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene("2ndScreen");

        }
    }

}
   