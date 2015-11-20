using UnityEngine;
using System.Collections;

public class KillBox : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Enemy")
        {
            Debug.Log(other + " has fallen off map!");
            other.GetComponent<Enemy>().Kill();
        }
        else if ( other.tag == "Player")
        {
            other.gameObject.SetActive(false);
        }
    }
}
