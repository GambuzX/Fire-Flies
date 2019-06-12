using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    private Animator outerRingAnimator;
    private bool flashed;

    // Start is called before the first frame update
    void Start()
    {
        outerRingAnimator = GameObject.FindGameObjectWithTag("OuterRing").GetComponent<Animator>();
        flashed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!flashed && this.transform.localScale.x == 0)
        {
            outerRingAnimator.SetTrigger("flash_red");
            flashed = true;
        }
    }
}
