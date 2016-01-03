using UnityEngine;
using ExtendedEvents;
using UnityEngine.UI;
using System.Collections;

public class Enemy : MonoBehaviour {

    public EnemyConfig config;
    public EnemyData refData; //> CHange this, its kinda dumb

    public bool targeted;

    private HealthBarController m_HealthBar;
    public Slider m_Slider;
    public Image m_FillImage;
    public Color healthBarColor = Color.red;
    public ParticleSystem DeathParticles;

	void Awake ()
    {
        refData = new EnemyData(config.data);
        m_HealthBar = GetComponentInChildren<HealthBarController>();
        m_FillImage.color = healthBarColor;
	}

    //Deal specified damage to enemy, kill if health below 0
    public void ApplyDamage(float damage)
    {
        StartCoroutine(MoveHealthBar(damage));

        refData.health -= damage;
        //EventManager.PostMessage(MessageKey.EnemyDamaged);

        if (refData.health <= float.Epsilon)
        {
            Kill();
        }
        else if (targeted)
        {
            //m_HealthBar.gameObject.SetActive(true);
        }
    }

    //Deactivate enemy
    public void Kill()
    {
        EventManager.PostMessage<EnemyDiedMessage>(new EnemyDiedMessage());
        Instantiate(DeathParticles, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }

    //Reuse inactive pooled enemy blob. Reset health value and health bar, then set active
    public void Respawn()
    {
        refData.health = config.data.health;
        m_Slider.value = refData.health;
        gameObject.SetActive(true);
    }

    IEnumerator MoveHealthBar(float damage, float duration = 0.6f)
    {
        float rate = 1f / duration;
        float t = 0f;
        float tempHealth = refData.health;

        //Not dependent on real Time
        while( t <= 1)
        {
            m_Slider.value = Easing.InterpolateFloat(tempHealth, tempHealth - damage, t, EasingType.Quartic, Easing.EaseIn);
            t += rate* Time.deltaTime;
            yield return null;
        }

        yield return null;
    }
}
