using UnityEngine;
using System.Collections;

public class ShotMover : MonoBehaviour {

    public float speed = 10.0f;
    public Vector3 shotDirection = Vector3.right;
    private Transform m_Transform;
    public HandLaser handLaser;

    public Easing.VectorEasingFunction easeFunction = Easing.VectorEaseIn;
    public EasingType e = EasingType.Quintic;
	
	void Awake ()
    {
        m_Transform = transform;
        //gameObject.SetActive(false);
        handLaser = GetComponentInParent<HandLaser>();
        if(!handLaser)
        {
            //Debug.Log("This is so clearly wrong, fix it");
        }
	}
	
	void Update ()
    {
        //m_Transform.Translate(shotDirection * Time.deltaTime * speed);	
	}

    void OnEnable()
    {
        StartCoroutine(MoveLaser(shotDirection * 7f));
    }

    IEnumerator MoveLaser(Vector3 destination)
    {
        float t = 0.6f;
        Vector3 tempPos = m_Transform.position;
        //The rate to add to the t value (1/s)
        float rate = speed / 7f;
        Vector3 offset = Easing.InterpolateVector(tempPos, destination, 0.1f, e, easeFunction);

        while ( t <= 1)
        {
            m_Transform.position = Easing.InterpolateVector(offset, destination, t, e, easeFunction) - offset;
            t += rate * Time.deltaTime;
            yield return null;
        }
        yield return null;
    }
}
