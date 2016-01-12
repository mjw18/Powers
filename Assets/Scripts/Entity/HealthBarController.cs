using UnityEngine;
using System.Collections;

public class HealthBarController : MonoBehaviour
{
    private Enemy m_Enemy;
    private SpriteRenderer m_HealthSprite;
    private Transform m_Transform;

    public float reduceTime = 1.4f;
    public Easing.EasingFunction easeFunction = Easing.EaseOutFloat;
    public EasingType easeType = EasingType.Quadratic;

    private Vector3 tempScale;

    void Awake()
    {
        m_Enemy = GetComponentInParent<Enemy>();
        if(!m_Enemy)
        {
            Debug.Log("M_Enemy not set!");
        }
        
        m_HealthSprite = GetComponentInChildren<SpriteRenderer>();
        if(!m_HealthSprite)
        {
            Debug.Log("HealthSprite not set");
        }

        m_Transform = transform;
        tempScale = m_Transform.localScale;

        RegisterListeners();

    }

    void RegisterListeners()
    {
        UnityEngine.Events.UnityAction<ExtendedEvents.EnemyDamagedMessage> onTakeDamage = OnTakeDamage;
        ExtendedEvents.EventManager.RegisterListener<ExtendedEvents.EnemyDamagedMessage>(onTakeDamage, ExtendedEvents.MessageKey.EnemyDamaged);
    }
        
    void OnTakeDamage(ExtendedEvents.EnemyDamagedMessage damageMessage)
    {
        StartCoroutine(ScaleHealth());
    }

    IEnumerator ScaleHealth()
    {
        float frac = m_Enemy.refData.health / m_Enemy.enemyConfig.data.health;
        if(frac >= 1)
        {
            yield break;
        }

        float t = 0;
        float rate = 0.6f;
        float originalScale = m_Transform.localScale.x;
        Vector3 tScale = m_Transform.localScale;
        
        while( t <= 1)
        {
            tScale.x = Easing.InterpolateFloat(originalScale, frac, t, easeType, easeFunction);
            t += rate * Time.deltaTime;
            m_Transform.localScale = tScale;
            yield return tScale.x;
        }
        yield return null;
    }

}
