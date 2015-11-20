using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class CameraController : MonoBehaviour {

    public float shakeTime;
    public float shakeSpeed;
    public float shakeDistance;

    private UnityAction shakeCamera;
    private Camera m_Camera;

    public Transform target;


	// Use this for initialization
	void Start ()
    {
        m_Camera = GetComponent<Camera>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        if(!target)
        {
            Debug.Log("Camera target not set");
        }

        //Register Shake Camera, own function/ dd one if more message registers
        EventManager.RegisterListener(EventManager.MessageKey.ChargeHit, ShakeCamera);
	}
	
	// Update is called once per frame
	void Update ()
    {
        Follow();

        if(!target)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    public IEnumerator ShakeCamera(float shakeTime)
    {
        float endTime = Time.time + shakeTime;
        Vector3 startPos = m_Camera.transform.position;
        while(Time.time <= endTime)
        {
            Vector3 offset = new Vector3(shakeDistance * Mathf.Sin(shakeSpeed*Time.deltaTime), 
                                         shakeDistance * Mathf.Cos(shakeSpeed * Time.deltaTime), 0f);
            Vector3 newpos = startPos + offset;

            m_Camera.transform.position = newpos;
            yield return null;
        }

        m_Camera.transform.position = startPos;
        yield return null;
    }

    void ShakeCamera()
    {
        StartCoroutine(ShakeCamera(shakeTime));
    }

    public void Follow()
    {
        Vector3 camPos = new Vector3(target.position.x, target.position.y, m_Camera.transform.position.z);
        m_Camera.transform.position = camPos; 
    }
}
