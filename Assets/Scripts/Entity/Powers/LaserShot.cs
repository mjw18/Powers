using UnityEngine;
using System.Collections;
using ExtendedEvents;

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

    //Destroy laser after given lifetime
    void Update()
    {
        if (m_LifeTimer.CheckTimer())
        {
            DestroyLaserShot();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //Shorthand for collider, work to fit copy pasted code, CHANGE
        Collider2D other = collision.collider;

        CollisionMessage colMes = new CollisionMessage(collision);
        IDMessage idMes = new IDMessage(other.gameObject.GetInstanceID());
        //IF not player, target selector, or laser object
        if (other.gameObject != gameObject && !other.gameObject.CompareTag(Tags.player) && !other.gameObject.CompareTag(Tags.targetSelector))
        {
            //Resets the lifetime timer
            m_LifeTimer.StopTimer();
            DestroyLaserShot();
            ExtendedEvents.EventManager.PostMessage<CollisionMessage>(colMes);
            ExtendedEvents.EventManager.PostMessage<IDMessage>(idMes);
            Debug.Log("LaserDestroyed!" + other);
        }
    }

    void OnTreiggerEnter2D(Collider2D other)
    {
        if (other.gameObject != gameObject && !other.gameObject.CompareTag(Tags.player) && !other.gameObject.CompareTag(Tags.targetSelector))
        {
            //Resets the lifetime timer
            m_LifeTimer.StopTimer();
            DestroyLaserShot();
            ExtendedEvents.EventManager.PostMessage(ExtendedEvents.MessageKey.LaserHit, other.gameObject.GetInstanceID());
            Debug.Log("LaserDestroyed!" + other);
        }
    }

    //Deactivate laser object, send messages (?)
    public void DestroyLaserShot()
    {
        gameObject.SetActive(false);
    }
}