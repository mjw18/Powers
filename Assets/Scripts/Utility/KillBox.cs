using UnityEngine;
using System.Collections;

public class KillBox : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Enemy")
        {
            other.GetComponent<Enemy>().Kill();
        }
        else if ( other.tag == "Player")
        {
            //Change This is stupid
            //GroundCheck hits first so we have to look in parent
            other.gameObject.GetComponentInParent<Player>().ApplyDamage(100f, DamageType.Physical);
        }
    }
}
