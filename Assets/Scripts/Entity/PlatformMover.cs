using UnityEngine;
using System.Collections;

public class PlatformMover : MonoBehaviour {

    bool playerOnPlatform = false;

    public Vector3 direction = Vector3.up;
    public float speed = 0.5f;
    public float waitTime = 1f;
    public Transform stopPos;

    private Rigidbody2D m_Rigidbody;
    private Vector3 startPos;

    Easing.VectorEasingFunction easeFunction = Easing.VectorEaseInOut;
    public EasingType easeType = EasingType.Quadratic;
    bool moveUp = false;

	// Use this for initialization
	void Start ()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        startPos = transform.position;
	}

    void FixedUpdate()
    {
        if(moveUp)
        {
            Debug.Log("moving");
            m_Rigidbody.AddForce(Vector2.up * 10f);
        }
        if(transform.position.y >= stopPos.position.y)
        {
            moveUp = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        playerOnPlatform = true;
        if (other.CompareTag("Player")) moveUp = true; 
            //StartCoroutine(MovePlatform());
    }

    void OnTriggerExit2D()
    {
        playerOnPlatform = false;
    }

    IEnumerator MovePlatform()
    {
        Vector3 tempPos = transform.position;
        float dist = Vector3.Magnitude(stopPos.position - tempPos);
        float rate = speed / dist;
        float t = 0f;

        yield return new WaitForSeconds(waitTime);

        if (!playerOnPlatform) yield break;

        while(t <= 1)
        {
            m_Rigidbody.MovePosition(Easing.InterpolateVector(tempPos, stopPos.position, t, easeType, easeFunction));

            t += rate;
            yield return null;
        }

        if(playerOnPlatform)
        {
            yield return new WaitForSeconds(waitTime);
        }

        yield return StartCoroutine( MovePlatformBack() );
    }

    IEnumerator MovePlatformBack()
    {
        Vector3 tempPos = transform.position;
        float dist = Vector3.Magnitude(tempPos - startPos);
        float rate = speed / dist;
        float t = 0f;
        
        while (t <= 1)
        {
            m_Rigidbody.MovePosition(Easing.InterpolateVector(tempPos, startPos, t, easeType, easeFunction));

            t += rate;
            yield return null;
        }
        yield return null;
    }
}
