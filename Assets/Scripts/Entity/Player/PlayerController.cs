﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour
{ 
    public GameObject particles;
    public ParticleSystem activeParticles;
    private Player m_Player;

    public Text chargeTimeText;     //> Delete?
    private Color defaultTextColor; //> Delete?

    //References
    private Rigidbody2D m_rigidbody;
    private Collider2D m_Collider;
    private Regulator m_LaserRegulator;
    private Transform m_ShootPosition;
    private SpriteRenderer m_SpriteRenderer;
    private PowerManager m_PowerManager;

	void Awake ()
    {
        //Error Checking would be nice?
        m_Player = GetComponent<Player>();
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_Collider = GetComponent<Collider2D>();
        m_LaserRegulator = GetComponent<Regulator>();
        //Change this, put in laser anymway?
        m_ShootPosition = GameObject.Find("ShootPosition").transform;
        m_SpriteRenderer = GetComponent<SpriteRenderer>();

        //Make Component
        m_PowerManager = GetComponentInChildren<PowerManager>();

        //Fuck using strings though
        chargeTimeText = GameObject.Find("EnergyText").GetComponent<Text>();
        if(!chargeTimeText)
        {
            Debug.Log("Charge text not set. CHANGE THIS THIS SUCKS");
        }
        //Move this to Player
        chargeTimeText.text = "Energy: " + m_Player.energy;
	}
	
	void Update ()
    {
        //Update text
        chargeTimeText.text = string.Format("Energy: {0}", (int)m_Player.energy ); 

        if(Input.GetButtonDown("Jump") && m_Player.canMove)
        {
            Jump();
        }
        
        //Use Power if it is assigned and key pressed
        if (m_PowerManager.primaryPower && Input.GetKeyDown(m_PowerManager.primaryPowerKey))
        {
            //Is the power already in use? Is there enough energy?
            if (m_PowerManager.primaryPower.canUsePower && m_Player.UseEnergy(m_PowerManager.primaryPower.powerConfig.energyCost))
            {
                m_PowerManager.primaryPower.ExecutePower();
            }
        }
        else if (m_PowerManager.secondaryPower && Input.GetKeyDown(m_PowerManager.secondaryPowerKey))
        {
            if (m_Player.UseEnergy(m_PowerManager.secondaryPower.powerConfig.energyCost) && m_PowerManager.secondaryPower.canUsePower)
            {
                m_PowerManager.secondaryPower.ExecutePower();
            }
        }
        else if (m_PowerManager.movementPower && Input.GetKeyDown(m_PowerManager.movementPowerKey))
        {
            if (m_Player.UseEnergy(m_PowerManager.movementPower.powerConfig.energyCost) && m_PowerManager.movementPower.canUsePower)
            {
                m_PowerManager.movementPower.ExecutePower();
            }
        }
        else if (m_PowerManager.defensivePower && Input.GetKeyDown(m_PowerManager.defensivePowerKey))
        {
            if (m_Player.UseEnergy(m_PowerManager.defensivePower.powerConfig.energyCost) && m_PowerManager.defensivePower.canUsePower)
            {
                m_PowerManager.defensivePower.ExecutePower();
            }
        }
    }

    void FixedUpdate()
    {
        float horz = Input.GetAxis("Horizontal");

        if (m_Player.canMove)
        {
            MoveManager(horz);
        }
    }

    //Flips player facing if neccessary
    void Flip(float horz)
    {
        if(Mathf.Sign(horz) != Mathf.Sign(m_Player.facing) && Mathf.Abs(horz) > float.Epsilon)
        {
            //Change facing direction to opposite
            m_Player.facing *= -1;
            //Flip sprite
            m_Player.transform.localScale = new Vector3(m_Player.facing, 1f, 1f);
        }
    }

    void MoveManager(float horz)
    {
        Flip(horz);
        Vector2 playerVelocity = Vector2.right * m_Player.maxSpeed * horz;
        Vector2 currentVel = new Vector2(playerVelocity.x, m_rigidbody.velocity.y);
        m_rigidbody.velocity = currentVel;

        //Try working with forces to get friction?
    /*
        Vector2 velocity = m_rigidbody.velocity;

        //No input, slowdown
        if (horz == 0.0f && Mathf.Abs(velocity.x) > float.Epsilon)
        {
            m_rigidbody.AddForce(new Vector2(-Mathf.Sign(m_rigidbody.velocity.x) * 10f, 0f));
           // m_rigidbody.velocity = new Vector2(0f, velocity.y);
        }
        if (m_Player.grounded)
        {
            m_rigidbody.AddForce(new Vector2(horz * 12f, 0));
        }
        else
        {
            m_rigidbody.AddForce(new Vector2(horz * 1f, 0));
        }

        //Clamp player speed in x direction (if diagonal surface, must clamp x and y
        m_rigidbody.velocity = new Vector2(Mathf.Clamp(velocity.x, -m_Player.maxSpeed, m_Player.maxSpeed), velocity.y);
    
    */
    }

    void Jump()
    {
        if(m_Player.grounded)
            m_rigidbody.AddForce(m_Player.jumpForce * Vector2.up);
    }
}
