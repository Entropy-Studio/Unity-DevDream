using UnityEngine;
using System.Collections;

public class GUI_Button : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI()
	{
		if(GUI.Button(new Rect(15, 15, 100, 50), "New game"))
		{

		}
	}
}
