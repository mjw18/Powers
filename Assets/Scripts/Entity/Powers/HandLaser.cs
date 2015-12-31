using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class HandLaser : Power {

    private Transform m_ShootPosition;

    public Vector3 shootDirection = Vector3.right;

    //Move to laser shot class?
    private RaycastHit2D m_Hit;

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
        //Default charge direction is forward
        Vector3 rayToTarget = shootDirection * player.facing;

        //Get ray to chosen target, change to use DamageType
        if (targetSelector.targets.Count > 0)
        {
            //Store target in power
            target = targetSelector.targets[0].GetComponent<Transform>();

            //Get ray from player to taget
            RaycastHit2D hit = Physics2D.Raycast(m_ShootPosition.position, 
                                                 target.position - m_Player.transform.position, 
                                                 powerConfig.range);

            rayToTarget = hit.point - (Vector2)m_ShootPosition.position;
            m_Hit = hit;
        }

        GameObject laser = GameManager.instance.m_LaserShotPool.NextPooledObject(false);

        laser.transform.position = m_ShootPosition.position;

        laser.transform.LookAt(m_Player.transform.position + rayToTarget);
        laser.GetComponent<ShotMover>().shotDirection = Vector3.Normalize(rayToTarget);
        laser.SetActive(true);
        Debug.DrawLine(m_ShootPosition.position, m_Hit.point, Color.white);
        base.Execute();
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
