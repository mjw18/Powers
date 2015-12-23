using UnityEngine;
using System.Collections;

public class HandLaser : Power {

    private Transform m_ShootPosition;

    public Vector3 shootDirection = Vector3.right;

    void Awake()
    {
        base.Init();
        m_ShootPosition = player.shootPosition;
    }

    public override void Execute()
    {
        base.Execute();

        //Default charge direction is forward
        Vector3 rayToTarget = shootDirection * player.facing;

        //Get ray to chosen target, change to use DamageType
        if (targetSelector.targets.Count > 0)
        {
            //Store target in power
            target = targetSelector.targets[0].GetComponent<Transform>();

            //Get ray from player to taget
            rayToTarget = target.position - m_Player.transform.position;
        }

        GameObject laser = GameManager.instance.laserShotPool.NextPooledObject(false);

        laser.transform.position = m_ShootPosition.position;

        laser.transform.LookAt(m_Player.transform.position + rayToTarget);
        Debug.Log(rayToTarget);
        Debug.Log(Vector3.Normalize(rayToTarget) * player.facing);
        laser.GetComponent<ShotMover>().shotDirection = Vector3.Normalize(rayToTarget);
        laser.SetActive(true);
    }

    public void OnLaserHit(Collider2D col)
    {
        foreach (var effect in powerConfig.visualEffects)
        {
            if(effect.placement == VisualEffectPlacement.CenteredAtTarget)
            {
                effect.visualEffect.SetPosition(col.gameObject.transform.position);
            }
        }

        if (col.gameObject.tag == "Enemy")
        {
            col.GetComponent<Enemy>().ApplyDamage(powerConfig.damage);
        }
    }
}
