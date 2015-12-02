using UnityEngine;
using System.Collections;

public class ShotMover : MonoBehaviour {

    public float speed = 5.0f;
    public Vector3 shotDirection = Vector3.right;
    private Transform m_Transform;
	
	void Awake ()
    {
        m_Transform = transform;
        //gameObject.SetActive(false);
	}
	
	void Update ()
    {
        m_Transform.Translate(shotDirection * Time.deltaTime * speed);	
	}
}
