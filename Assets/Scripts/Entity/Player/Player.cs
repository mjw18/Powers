using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public float maxSpeed = 10f;
    public float jumpForce = 100f;
    public int facing = 1;

    public float maxEnergy = 30.0f;
    public float energyRechargeRate = 3f;
    public float energy;

    public PlayerStateMachine m_StateMachine;

    private bool m_grounded = false;

    public bool grounded
    {
        get { return m_grounded; }
        set { m_grounded = value; }
    }

    private CircleCollider2D m_groundCheck;
    private SpriteRenderer m_SpriteRenderer;
    private BoxCollider2D m_collider;
    private Animator m_animController;

    // Use this for initialization
    void Awake ()
    {
       // m_StateMachine = GetComponent<PlayerStateMachine>();
        //m_StateMachine.Init();

        m_groundCheck = GameObject.Find("GroundCheck").GetComponent<CircleCollider2D>();
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

    //use physics checksphere?
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Ground")
        {
            m_grounded = true;
        }
    }

}
