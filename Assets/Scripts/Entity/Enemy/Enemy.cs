using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public EnemyConfig config;
    private EnemyData refData;

    public bool targeted;

    public ParticleSystem DeathParticles;

	// Use this for initialization
	void Awake ()
    {
        refData = new EnemyData(config.data);
	}
	
	// Update is called once per frame
	void LateUpdate ()
    {
        if (refData.health <= -float.Epsilon)
        {
            Kill();
        }
        else if(targeted)
        {
            Debug.Log("This enemy has been targeted");
        }
    }

    //Deal specified damage to enemy, kill if health below 0
    public void ApplyDamage(float damage)
    {
        refData.health -= damage;
    }

    public void Kill()
    {
        EventManager.PostMessage(EventManager.MessageKey.EnemyDied);
        Instantiate(DeathParticles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
