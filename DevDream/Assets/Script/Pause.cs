using UnityEngine;
using System.Collections;

public class Pause : MonoBehaviour {

    bool paused = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetButtonDown("Esc") && paused == false)
        {
            paused = true;
            Time.timeScale = 0;
        }
        else
        {
            if (Input.GetButtonDown("Esc") && paused == true)
            {
                paused = false;
                Time.timeScale = 1;
            }
        }
	}
}
