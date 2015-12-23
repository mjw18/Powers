using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Enemy : MonoBehaviour {

    public EnemyConfig config;
    public EnemyData refData;

    public bool targeted;

    private HealthBarController m_HealthBar;
    public Slider m_Slider;
    public Image m_FillImage;
    public Color healthBarColor = Color.red;
    public ParticleSystem DeathParticles;

	// Use this for initialization
	void Awake ()
    {
        refData = new EnemyData(config.data);
        m_HealthBar = GetComponentInChildren<HealthBarController>();
        m_FillImage.color = healthBarColor;
	}
	
	// Update is called once per frame
	void LateUpdate ()
    {

    }

    //Deal specified damage to enemy, kill if health below 0
    public void ApplyDamage(float damage)
    {
        StartCoroutine(MoveHealthBar(damage));

        refData.health -= damage;
        EventManager.PostMessage(EventManager.MessageKey.EnemyDamaged);

        if (refData.health <= float.Epsilon)
        {
            Kill();
        }
        else if (targeted)
        {
            //m_HealthBar.gameObject.SetActive(true);
        }
    }

    public void Kill()
    {
        EventManager.PostMessage(EventManager.MessageKey.EnemyDied);
        Instantiate(DeathParticles, transform.position, Quaternion.identity);
        Destroy(gameObject);
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
