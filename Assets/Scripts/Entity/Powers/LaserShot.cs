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

        //IF not player, target selector, or laser object
        if (other.gameObject != gameObject && !other.gameObject.CompareTag(Tags.player) && !other.gameObject.CompareTag(Tags.targetSelector))
        {
            //Resets the lifetime timer
            m_LifeTimer.StopTimer();
            DestroyLaserShot();
            EventManager.PostMessage<LaserHitMessage>(new LaserHitMessage(other.gameObject.GetInstanceID(), collision));
            Debug.Log("LaserDestroyed!" + other);
        }
    }

    //Deactivate laser object, send messages (?)
    public void DestroyLaserShot()
    {
        gameObject.SetActive(false);
    }
}