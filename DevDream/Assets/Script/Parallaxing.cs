using UnityEngine;
using System.Collections;

public class Parallaxing : MonoBehaviour {

	public Transform[] backgrounds;			// niz svih pozadinskih slika koje treba da se paralaxuju
	private float[] parallaxScales;			// proporcija pomeraja kamere za pomeranje pozadinskih slika
	public float smoothing = 1f;			// koliko je mekan paralax. obavezno > 0

	private Transform cam;					// referenca na transformaciju glavne kamere
	private Vector3 previousCamPosition;	// cuva poziciju kamere u prethodnom frejmu (Vector3 je tip promenljive (trodimenzionalni vektor))


	// poziva se pre Start()
	void Awake () {
		cam = Camera.main.transform;
	}

	// Use this for initialization
	void Start () {
		// prethodni frejm 
		previousCamPosition = cam.position;


		parallaxScales = new float[backgrounds.Length];
		for (int i=0; i < backgrounds.Length; i++)
		{
			parallaxScales[i] = backgrounds[i].position.z*-1;
		}
	}
	
	// Update is called once per frame
	void Update () {

		// za svaku pozadinu
		for (int i=0; i < backgrounds.Length; i++)
		{
			// paralax je razlika izmedju prethodnog i novog polozaja kamere na x osi pomnopzen sa parallaxScales za tu pozadinu
			float parallax = (previousCamPosition.x - cam.position.x) * parallaxScales[i];

			// podesavanje x pozicije
			float backgroundTargetPosX = backgrounds[i].position.x + parallax;

			Vector3 backgroundTargetPos = new Vector3 (backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);

			//lerp izmedju trenutne i ciljane pozicije
			backgrounds[i].position = Vector3.Lerp (backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
		}

		//postavljanje previsousCamPos na poziciju kamere na kraju frejma
		previousCamPosition = cam.position;
	
	}
}
