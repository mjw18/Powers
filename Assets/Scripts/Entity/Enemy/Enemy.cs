using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public EnemyConfig config;
    public EnemyData refData;

    public bool targeted;

    private HealthBarController m_HealthBar;
    public ParticleSystem DeathParticles;

	// Use this for initialization
	void Awake ()
    {
        refData = new EnemyData(config.data);
        m_HealthBar = GetComponentInChildren<HealthBarController>();
	}
	
	// Update is called once per frame
	void LateUpdate ()
    {
        if (refData.health <= float.Epsilon)
        {
            Kill();
        }
        else if(targeted)
        {
            m_HealthBar.gameObject.SetActive(true);
        }
    }

    //Deal specified damage to enemy, kill if health below 0
    public void ApplyDamage(float damage)
    {
        Debug.Log(string.Format("Dealt {0} damage to enemy", damage));
        refData.health -= damage;
        EventManager.PostMessage(EventManager.MessageKey.EnemyDamaged);
    }

    public void Kill()
    {
        EventManager.PostMessage(EventManager.MessageKey.EnemyDied);
        Instantiate(DeathParticles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
