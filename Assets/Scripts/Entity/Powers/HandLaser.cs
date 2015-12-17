﻿using UnityEngine;
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

        //Get ray to chosen target
        if (targetSelector.targets.Count > 0)
        {
            Debug.Log(targetSelector.targets[0]);
            rayToTarget = targetSelector.targets[0].transform.position - m_Player.transform.position;
        }

        GameObject laser = GameManager.instance.laserShotPool.NextPooledObject(false);
        laser.transform.position = m_ShootPosition.position;
        laser.transform.LookAt(target);
        Debug.Log("Laser Shot Rotation: " + laser.transform.rotation);
        laser.GetComponent<ShotMover>().shotDirection = Vector3.Normalize(rayToTarget) * player.facing;
        laser.SetActive(true);

        Ray laserRay = new Ray();
        laserRay.origin = m_ShootPosition.position;
        //laserRay.direction = Vector3.right * m_Player.facing;

        //Store raycast hit
        RaycastHit2D laserHit = Physics2D.Raycast(laserRay.origin, laserRay.direction, powerConfig.range);

        if (!laserHit)
        {
            Debug.Log("Raycast returns null");
            //Draw a big line (FIX THIS I ADDED A RANDOM 5 FOR GRINS)
           // shootDirection = m_ShootPosition.position + powerConfig.range * laserRay.direction * 5f;
        }
        else if (laserHit.collider.tag == "Player")
        {
            Debug.Log("You are hitting the player with this linecast");
        }
        else if (laserHit.collider.tag == "Enemy")
        {
            Debug.Log("You are hitting the enemy with this linecast");
            EventManager.PostMessage(EventManager.MessageKey.LaserHit);
            laserHit.transform.gameObject.GetComponent<Enemy>().ApplyDamage(powerConfig.damage); ;

            //End Laser shot at enemy
          // hitPosition = laserHit.transform.position;
        }
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
