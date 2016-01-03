using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using ExtendedEvents;

public class CameraController : MonoBehaviour {

    public float shakeTime;
    public float shakeSpeed;
    public float shakeDistance;

    public Easing.VectorEasingFunction easeFunction = Easing.VectorEaseInOut;
    public EasingType e = EasingType.Sin;

    private Camera m_Camera;

    private Transform m_CameraTransform;
    public Transform target;
    private bool m_DoFollow = false;


	// Use this for initialization
	void Start ()
    {
        m_Camera = GetComponent<Camera>();
        m_CameraTransform = m_Camera.transform;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        if(!target)
        {
            Debug.Log("Camera target not set");
        }

        m_DoFollow = true;
        //Necessary?
        UnityAction<ShakeCameraMessage> shakeCamera = ShakeCamera;

        //Make a generic "move to specific target" message
        UnityAction<PlayerRespawnedMessage> findPlayer = OnTargetLoss;

        //Register Shake Camera, own function/ dd one if more message registers
        ExtendedEvents.EventManager.RegisterListener<ShakeCameraMessage>(shakeCamera, MessageKey.ShakeCamera);
        ExtendedEvents.EventManager.RegisterListener<PlayerRespawnedMessage>(findPlayer, MessageKey.PlayerRespawned);
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(m_DoFollow) Follow();

        if(!target)
        {
            m_DoFollow = false;
            Invoke("OnTargetLoss", 2f);
        }
    }

    public IEnumerator ShakeCamera(float shakeTime)
    {
        Debug.Log("Shaking Camera");
        float endTime = Time.time + shakeTime;
        Vector3 startPos = m_Camera.transform.position;
        while(Time.time <= endTime)
        {
            Vector3 offset = new Vector3(shakeDistance * Mathf.Sin(shakeSpeed*Time.deltaTime), 
                                         shakeDistance * Mathf.Cos(shakeSpeed * Time.deltaTime), 0f);
            Vector3 newpos = startPos + offset;

            m_CameraTransform.position = newpos;
            yield return null;
        }

        m_CameraTransform.position = startPos;
        yield return null;
    }

    void ShakeCamera(ShakeCameraMessage shakeMessage)
    {
        StartCoroutine(ShakeCamera(shakeMessage.shakeTime));
    }

    //This is terrible, use easing functions
    public void Follow()
    {
        Vector3 camPos = new Vector3(target.position.x, target.position.y, m_CameraTransform.position.z);
        m_CameraTransform.position = camPos; 
    }

    //use return coroutine here (for completion bool? for I dont know what)
    void OnTargetLoss(PlayerRespawnedMessage respawn)
    {
        m_DoFollow = false;
        Time.timeScale = 0.0f;
        StartCoroutine(MoveCameraToTarget(m_CameraTransform, target, 2f));
    }

    public Vector3 GetMousePosition()
    {
        //Transfers camera's z position, we don't want this
        Vector3 uncorrectedPos = m_Camera.ScreenToWorldPoint(Input.mousePosition);
        //set z to 0 (gameplay plane)
        Vector3 correctedPos = new Vector3(uncorrectedPos.x, uncorrectedPos.y, 0f);
        return correctedPos;    
    }

    //Write Extension method if ever return values from Coroutine
    IEnumerator MoveCameraToTarget(Transform mod, Transform curTarget, float duration)
    {
        float t = 0f;
        Vector3 targPos = curTarget.position + Vector3.forward * m_CameraTransform.position.z;
        Vector3 tempPos = m_CameraTransform.position;
        //The rate to add to the t value (1/s)
        float rate = 1.0f / duration;

        while (t <= 1f)
        {
            mod.position = Easing.InterpolateVector(tempPos, targPos, t, e, easeFunction);
            t += rate * GameManager.instance.globalRegulator.realDeltaTime;
            yield return curTarget;
        }

        m_DoFollow = true;
        Time.timeScale = 1.0f;
        yield return null;
    }    
}
