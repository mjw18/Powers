using UnityEngine;
using System.Collections;

public class HandLaser : Power {

    private Transform m_ShootPosition;

    public Vector3 hitPosition;

    public override void Execute()
    {
        base.Execute();

        LayerMask enemyLayerMask = LayerMask.GetMask("Enemy");
        Ray laserRay = new Ray();
        laserRay.origin = m_ShootPosition.position;
        //laserRay.direction = Vector3.right * m_Player.facing;

        //Store raycast hit
        RaycastHit2D laserHit = Physics2D.Raycast(laserRay.origin, laserRay.direction, powerConfig.range, enemyLayerMask);

        if (!laserHit)
        {
            Debug.Log("Raycast returns null");
            //Draw a big line (FIX THIS I ADDED A RANDOM 5 FOR GRINS)
            hitPosition = m_ShootPosition.position + powerConfig.range * laserRay.direction * 5f;
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
           hitPosition = laserHit.transform.position;
        }
    }
}
