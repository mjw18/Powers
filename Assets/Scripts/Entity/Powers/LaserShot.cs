using UnityEngine;
using System.Collections;

public class LaserShot : MonoBehaviour {

    public float lifetime = 10f;

    private Transform m_Transform;
    private Regulator m_LifeTimer;

    void Awake()
    {
        m_LifeTimer = GetComponent<Regulator>();
        m_LifeTimer.waitTime = lifetime;
        m_Transform = transform;
    }

    void OnEnable()
    {
        //Use regulator? Gives option for backing out of double setactive call
        m_LifeTimer.StartTimer();
    }

    void Update()
    {
        if(m_LifeTimer.CheckTimer())
        {
            DestroyLaserShot();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject != gameObject && !other.gameObject.CompareTag(Tags.player) && !other.gameObject.CompareTag(Tags.targetSelector))
        {
            Debug.Log(transform.position);
            //Resets the lifetime timer
            m_LifeTimer.StopTimer();
            DestroyLaserShot();
            ExtendedEvents.EventManager.PostMessage(ExtendedEvents.MessageKey.LaserHit, other.gameObject.GetInstanceID());
            Debug.Log("LaserDestroyed!" + other);
        }
    }

    public void DestroyLaserShot()
    {
        gameObject.SetActive(false);
    }
}
