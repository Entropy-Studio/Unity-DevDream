using UnityEngine;
using System.Collections;

[RequireComponent (typeof(SpriteRenderer))]

public class Tiling : MonoBehaviour {
	
	public int offsetX = 2;					//koliko pre poklapanja x koordinata kamere i ivice pozadinske slike da se stvori nova instanca slike za nastavak

	public bool hasARightBuddy = false;		//da li je postoji kopija desno
	public bool hasALeftBuddy = false;		//da li je postoji kopija levo

	public bool reverseScale = false;		// postaviti na true ako je potrebno obrnuti sprite da bi se nastavio neprimetno

	private float spriteWidth = 0f;			//sirina sprajta
	private Camera cam;
	private Transform myTransform;

	void Awake (){
		cam = Camera.main;
		myTransform = transform;
	}

	// Use this for initialization
	void Start () {
		SpriteRenderer sRenderer = GetComponent<SpriteRenderer> ();
		spriteWidth = sRenderer.sprite.bounds.size.x;
	}
	
	// Update is called once per frame
	void Update () {
		// provera da li je potrebno napraviti novu instancu? ako nije ne raditi nista
		if (hasALeftBuddy == false || hasARightBuddy == false)
		{
			// izracunavanje polovine sirine kamere u koordinatama sveta (umesto u pixelima)
			float camHorizontalExtent = cam.orthographicSize * Screen.width/Screen.height;

			// izracunavanje x pozicije gde kamera moze da vidi ivicu sprajta
			float edgeVisiblePositionRight = (myTransform.position.x + spriteWidth/2) - camHorizontalExtent;
			float edgeVisiblePositionLeft = (myTransform.position.x - spriteWidth/2) + camHorizontalExtent;

			//provera da li se vidi ivica elementa i pozivanje MakeNewBuddy() funkcije
			if (cam.transform.position.x >= edgeVisiblePositionRight - offsetX && hasARightBuddy == false)
			{
				MakeNewBuddy(1);
				hasARightBuddy = true;
			}
			else if (cam.transform.position.x <= edgeVisiblePositionLeft + offsetX && hasALeftBuddy == false)
			{
				MakeNewBuddy(-1);
				hasALeftBuddy = true;
			}
		}
	
	}

	// Funkcija koja pravi novu instancu pozadine na potrebnoj strani
	void MakeNewBuddy (int rightOrLeft) {
		//racunanje pozicije za novu instancu pozadinske slike
		Vector3 newPosition = new Vector3 (myTransform.position.x + spriteWidth * rightOrLeft, myTransform.position.y, myTransform.position.z);
		// instanciranje i smestanje u promenljivu
		Transform newBuddy = Instantiate (myTransform, newPosition, myTransform.rotation) as Transform;

		// ako je potrebno okrenuti sliku da bi se uklopila
		if (reverseScale == true) 
		{
			newBuddy.localScale = new Vector3 (newBuddy.localScale.x*-1, newBuddy.localScale.y, newBuddy.localScale.z);
		}
		// postavljanje roditelja nove instance na istog roditelja kao i originalne slike
		newBuddy.parent = myTransform.parent;

		if (rightOrLeft > 0) 
			{
				newBuddy.GetComponent<Tiling> ().hasALeftBuddy = true; // postavljanje hasALeftBuddy promenljive na true u skripti "Tiling" novokreirane instance
			} else 
				{
					newBuddy.GetComponent<Tiling>().hasARightBuddy = true;
				}
	}
}
