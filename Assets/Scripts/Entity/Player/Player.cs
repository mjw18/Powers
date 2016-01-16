using UnityEngine;
using ExtendedEvents;
using System.Collections;

public class Player : Agent
{
    public float maxSpeed = 8f;
    public float jumpForce = 100f;

    //Player control state
    public int facing = 1;
    public bool canMove = true;
    public bool isAiming = false;

    new public float maxEnergy = 30.0f;
    new public float energyRechargeRate = 2.5f;
    public float energy;

    //Ground checking, move to separate script to attach to other entities
    private bool m_grounded = false;
    public float groundCheckRadius = 0.1f;
    public LayerMask whatIsGround;
    private Transform m_GroundCheck;

    public bool grounded
    {
        get { return m_grounded; }
        set { m_grounded = value; }
    }

    public Transform shootPosition;
    private SpriteRenderer m_SpriteRenderer;
    private BoxCollider2D m_collider;
    private Animator m_animController;

    void Awake ()
    {
        shootPosition = GameObject.Find("ShootPosition").GetComponent<Transform>();
        m_GroundCheck = GameObject.Find("GroundCheck").GetComponent<Transform>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_collider = GetComponent<BoxCollider2D>();
        m_animController = GetComponent<Animator>();
    }
	
	void Update ()
    {
        //Handle player energy for powers
        energy = Mathf.Clamp(energy + energyRechargeRate * Time.deltaTime, 0.0f, maxEnergy);
    }

    public bool CheckGrounded()
    {
        //Use Layers?
        Collider2D[] cols = Physics2D.OverlapCircleAll(m_GroundCheck.position, groundCheckRadius);
        foreach (Collider2D col in cols)
        {
            //Still don't like these strings
            if (col.gameObject.tag == "Ground")
            {
                return true;
            }            
        }
        return false;
    }

    void FixedUpdate()
    {
        m_grounded = false;

        m_grounded = CheckGround();
    }

    bool CheckGround()
    {
        //Circle cast at ground position
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, groundCheckRadius, whatIsGround);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                //So we don't have to check all circlecasted objects
                return true;
            }
        }
        return false;
    }

    //Usse player energy, return true if power can be used
    public bool UseEnergy(float requiredEnergy)
    {
        //Not enough energy to use given power
        if (energy - requiredEnergy < -float.Epsilon)
            return false;

        //Use required energy return can use
        energy -= requiredEnergy;
        return true;
        
    }

    public override void ApplyDamage(float damage, DamageType damageType)
    {
        base.ApplyDamage(damage, damageType);

        //Animation/messaging?
    }

    public override void OnDeath()
    {
        //EventManager.PostMessage(MessageKey.PlayerDied);
        gameObject.SetActive(false);
    }
}
