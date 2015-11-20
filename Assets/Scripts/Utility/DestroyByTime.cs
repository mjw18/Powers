using UnityEngine;
using System.Collections;

public class DestroyByTime : MonoBehaviour {

    public float lifetime;

	// Destroy parent object after given amount of time
	void Start ()
    {
        Destroy(gameObject, lifetime);
	}

}
