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

    void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(MoveLaser());
    }

    IEnumerator MoveLaser()
    {
        float t = 0.0f;
        Vector3 tempPos = m_Transform.position;
        //The rate to add to the t value (1/s)
        float rate = speed / 7f;
        Vector3 offset = Easing.InterpolateVector(tempPos, shotDirection * 17f, 0.1f, e, easeFunction);

        while ( t <= 1 )
        {
            m_Transform.position = Easing.InterpolateVector(tempPos, shotDirection * 17f , t, e, easeFunction);
            t += rate * Time.deltaTime;
            yield return null;
        }
        yield return null;
    }
}
