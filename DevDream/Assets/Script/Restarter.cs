﻿//Ova skripta je pozvana na kraju Death animacije (3sec) objekta Player

using UnityEngine;
using System.Collections;

public class Restarter : MonoBehaviour {

	void OnTriggerEnter2D (Collider2D other)
	{
		if(other.tag == Tags.playerTag)
			Application.LoadLevel(Application.loadedLevelName);
	}
}
