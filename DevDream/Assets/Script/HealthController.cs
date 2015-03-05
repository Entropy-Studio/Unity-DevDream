using UnityEngine;
using System.Collections;

public class HealthController : MonoBehaviour {
    
    public float health = 100f;

	void Update () 
    {
	    if (health < 0)
        {
            health = 0f;
        }
	}

    public void Damage(float damage)
    {
       // Debug.Log("damage = " + damage);
        health -= damage;
    }
}
