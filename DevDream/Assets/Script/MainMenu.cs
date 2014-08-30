using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	void OnGUI()
	{
		if(GUI.Button(new Rect(Screen.width/2-100, Screen.height/3, 200, 50), "New Game"))
		{
			Application.LoadLevel("MainLevel");
		}
		if(GUI.Button(new Rect(Screen.width/2-100, Screen.height/2, 200, 50), "Exit Game"))
		{
			Application.Quit();
		}
	}
}
