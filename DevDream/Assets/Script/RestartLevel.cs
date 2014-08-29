using UnityEngine;
using System.Collections;

public class RestartLevel : MonoBehaviour {

	void RestartujNivo()
	{
		Application.LoadLevel(Application.loadedLevelName);
	}
}
