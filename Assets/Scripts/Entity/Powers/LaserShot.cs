using UnityEngine;
using System.Collections;

public class LaserShot : MonoBehaviour {

    public Transform laserShooter;
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
        m_Transform.position = laserShooter.position;
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
        if(other.tag != "Player")
        {
            Debug.Log(other);
            m_LifeTimer.StopTimer();
            DestroyLaserShot();
            Debug.Log("LaserDestroyed!");
        }
    }

    public void DestroyLaserShot()
    {
        gameObject.SetActive(false);
    }
}
