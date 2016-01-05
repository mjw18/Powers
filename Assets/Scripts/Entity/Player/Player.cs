using UnityEngine;
using ExtendedEvents;
using System.Collections;

public class Player : MonoBehaviour
{
    private float health = 100f;

    public float maxSpeed = 10f;
    public float jumpForce = 100f;
    public int facing = 1;

    public bool canMove = true;

    public float maxEnergy = 30.0f;
    public float energyRechargeRate = 3f;
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

    // Use this for initialization
    void Awake ()
    {

        shootPosition = GameObject.Find("ShootPosition").GetComponent<Transform>();
        m_GroundCheck = GameObject.Find("GroundCheck").GetComponent<Transform>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_collider = GetComponent<BoxCollider2D>();
        m_animController = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Handle player energy for powers
        energy = Mathf.Clamp(energy + energyRechargeRate * Time.deltaTime, 0.0f, maxEnergy);
    }

    public bool CheckGrounded()
    {
        //Move all this to separate script
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
        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.

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

    public void ApplyDamage(float damage)
    {
        health -= damage;
        if(health <= float.Epsilon)
        {
            OnPlayerDied();
        }
    }

    public void OnPlayerDied()
    {
        //EventManager.PostMessage(MessageKey.PlayerDied);
        gameObject.SetActive(false);
    }
}
