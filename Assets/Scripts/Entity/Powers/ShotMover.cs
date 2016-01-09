using UnityEngine;
using System.Collections;

public class ShotMover : MonoBehaviour {

    public float speed = 9.0f;
    public Vector3 shotDirection;// = Vector3.right;
    private Transform m_Transform;
    public float laserRange = 17f;
    private int i = 0;
    public Easing.VectorEasingFunction easeFunction = Easing.VectorEaseIn;
    public EasingType e = EasingType.Quintic;
	
	void Awake ()
    {
        m_Transform = transform;
	}

    void OnEnable()
    {
        //StopAllCoroutines();
        //StartCoroutine(MoveLaser());
    }

    void Update()
    {
        GetComponent<Rigidbody2D>().velocity = speed * shotDirection;
    }

    IEnumerator MoveLaser()
    {
        
        float t = 0.0f;
        Vector3 tempPos = m_Transform.position;
        //The rate to add to the t value (1/s)
        float rate = speed / 7f;
        Vector3 offset = Easing.InterpolateVector(tempPos, shotDirection * laserRange, 0.1f, e, easeFunction);
        Debug.Log(shotDirection.y / shotDirection.x);
        while ( t <= 1 )
        {
            m_Transform.position = Easing.InterpolateVector(tempPos, shotDirection * laserRange , t, e, easeFunction);
            t += rate * Time.deltaTime;
            yield return null;
        }

        //Destroy when max range is reached. Use message?
        gameObject.GetComponentInParent<LaserShot>().DestroyLaserShot();

        yield return null;
    }
}
