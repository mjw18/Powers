using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public float maxSpeed = 10f;
    public float jumpForce = 100f;
    public int facing = 1;

    public PlayerStateMachine m_StateMachine;

    private bool m_grounded = false;

    public bool grounded
    {
        get { return m_grounded; }
        set { m_grounded = value; }
    }

    private CircleCollider2D m_groundCheck;
    private BoxCollider2D m_FrontCheck;

    private BoxCollider2D m_collider;
    private Animator m_animController;

    // Use this for initialization
    void Awake ()
    {
       // m_StateMachine = GetComponent<PlayerStateMachine>();
        //m_StateMachine.Init();

        m_groundCheck = GameObject.Find("GroundCheck").GetComponent<CircleCollider2D>();
        m_collider = GetComponent<BoxCollider2D>();
        m_animController = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update ()
    {

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
