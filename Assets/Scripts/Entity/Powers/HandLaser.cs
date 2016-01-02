using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class HandLaser : Power {

    private Transform m_ShootPosition;

    public Vector3 shootDirection = Vector3.right;

    void Awake()
    {
        base.Init();
        m_ShootPosition = player.shootPosition;

        //Register on hit event
        UnityAction<int> onLaserHitAction = OnLaserHit;
        ExtendedEvents.EventManager.RegisterListener(ExtendedEvents.MessageKey.LaserHit, onLaserHitAction);
    }

    public override void Execute()
    {
        Vector3 rayToTarget = GetShootDirection();

        GameObject laser = GameManager.instance.m_LaserShotPool.NextPooledObject(false);

        laser.transform.position = m_ShootPosition.position;
        //GameObject.Find("Targeted").GetComponent<Transform>().position = m_ShootPosition.position;
        laser.transform.LookAt(m_Player.transform.position + rayToTarget);
        laser.GetComponent<ShotMover>().shotDirection = Vector3.Normalize(rayToTarget);
        laser.SetActive(true);
        base.Execute();
    }

    Vector3 GetShootDirection()
    {
        //Default charge direction is forward
        Vector3 rayToTarget = Vector3.right * player.facing;

        Debug.Log((m_Hit.point.y - m_ShootPosition.position.y)/(m_Hit.point.x - m_ShootPosition.position.x));

        //Get ray to chosen target, change to use DamageType
        if (targetSelector.targets.Count == 0) return m_Hit.point - (Vector2)m_ShootPosition.position;

        //Store target in power, why, I don't know
        target = targetSelector.targets[0].GetComponent<Transform>();

        return m_Hit.point - (Vector2)m_ShootPosition.position;

    }

    public void OnLaserHit(int hitID)
    {
        GameObject hitObj = GameManager.instance.entityTable.GetEntityFromID(hitID);

        if(!hitObj)
        {
            Debug.Log("Entity ID not registered");
            return;
        }

        foreach (var effect in powerConfig.visualEffects)
        {
            if(effect.placement == VisualEffectPlacement.CenteredAtTarget)
            {
                effect.visualEffect.SetPosition(hitObj.transform.position);
            }
        }

        if (hitObj.CompareTag(Tags.enemy) )
        {
            hitObj.GetComponent<Enemy>().ApplyDamage(powerConfig.damage);
        }

        //Fire Play particles event?
        //ExtendedEvents.EventManager.PostMessage(ExtendedEvents.MessageKey.LaserHit, m_Hit);
    }
}
