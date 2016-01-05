using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using ExtendedEvents;

public class HandLaser : Power {

    public Vector3 shootDirection = Vector3.right;

    //Object refernce for pool lookup
    public GameObject Laser;

    void Awake()
    {
        base.Init();
        m_ShootPosition = player.shootPosition;

        //Register on hit event
        UnityAction<LaserHitMessage> onLaserHitAction = OnLaserHit;
        EventManager.RegisterListener<LaserHitMessage>(onLaserHitAction);
    }

    public override void Execute()
    {
        Vector3 rayToTarget = GetShootDirection();

        //Get next laser
        GameObject laser = GameManager.instance.GetObjectPool(Laser).NextPooledObject(false);

        //Place laser at shootposition
        laser.transform.position = m_ShootPosition.position;

        //Rotate laser in travel direction
        laser.transform.LookAt(m_Player.transform.position + rayToTarget);

        //Make laser move in correct direction
        laser.GetComponent<ShotMover>().shotDirection = Vector3.Normalize(rayToTarget);

        //Activate laser
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

    public void OnLaserHit(LaserHitMessage hit)
    {
        //Get gameobject by ID
        GameObject hitObj = GameManager.instance.entityTable.GetEntityFromID(hit.ID);

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
    }
}
