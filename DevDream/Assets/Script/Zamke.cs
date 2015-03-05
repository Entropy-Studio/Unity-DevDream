using UnityEngine;
using System.Collections;

//-chainsaw
//-laserska kapija
//-homing raketa
//-projektil raketa, linearno kretanje (malo brze od homing)
//-hidraulicna presa
//-propadajuci pod
public class Zamke : MonoBehaviour 
{
    public float damage = 0f;
    public bool instantKill = false;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == Tags.damageColliderTag)
        {
            if (instantKill)
            {
                other.transform.root.gameObject.GetComponent<HealthController>().health = 0f;
            }
            else
            {
                other.transform.root.gameObject.GetComponent<HealthController>().Damage(damage);
            }
        }
    }
}