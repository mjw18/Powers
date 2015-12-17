using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public KeyCode primaryPowerKey = KeyCode.E;
    public KeyCode secondaryPowerKey = KeyCode.R;
    public Power primaryPower;
    public Power secondaryPower; 

    //Remove this!!! Use beefed up Coroutine to add Stop or checks for already running
    private bool m_SlowingTime = false;

    public GameObject particles;

    public ParticleSystem activeParticles;
    private Player m_Player;

    public Text chargeTimeText;     //> Delete?
    private Color defaultTextColor; //> Delete?

    private Rigidbody2D m_rigidbody;
    private Collider2D m_Collider;
    private Regulator m_LaserRegulator;
    private Transform m_ShootPosition;
    private SpriteRenderer m_SpriteRenderer;

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

        primaryPower = GetComponent<HandLaser>();
        secondaryPower = GetComponent<Charge>();

        if(!m_ShootPosition)
        {
            Debug.Log("ShootPosition not found");
        }

        //Fuck using strings though
        chargeTimeText = GameObject.Find("EnergyText").GetComponent<Text>();
        if(chargeTimeText == null)
        {
            Debug.Log("Charge text not set. CHANGE THIS THIS SUCKS");
        }
        //Move this to Player
        chargeTimeText.text = "Energy: " + m_Player.energy;
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Update text
        chargeTimeText.text = string.Format("Energy: {0}", (int)m_Player.energy ); 

        if(Input.GetKeyDown(KeyCode.LeftControl) && !m_SlowingTime)
        {
            Debug.Log(string.Format("My delta time: {0} \n Unity delta time: {1}", m_LaserRegulator.realDeltaTime, Time.deltaTime));

            m_SlowingTime = true;
            //StartCoroutine(SlowTimeScale(0.3f));
        }
        /*  if (!Input.GetKey(KeyCode.LeftControl) && Time.timeScale <= 0.98)
          {
              StartCoroutine(ResetTimeScale());
          }*/
        if(Input.GetButtonDown("Jump"))
        {
            Jump();
        }
        if (Input.GetKeyDown(primaryPowerKey))
        {
            if (m_Player.UseEnergy(primaryPower.powerConfig.energyCost))
            {
                if (primaryPower != null) StartCoroutine(primaryPower.UsePower());
                    //primaryPower.Execute();
            }
        }
        else if (Input.GetKeyDown(secondaryPowerKey))
        {
            if (m_Player.UseEnergy(secondaryPower.powerConfig.energyCost))
            {
                if (secondaryPower != null) StartCoroutine(secondaryPower.UsePower());
            }
        }
    }

    void FixedUpdate()
    {
        float horz = Input.GetAxis("Horizontal");

        bool m_CanMove = true;
        if (m_CanMove)
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
    }

    void Jump()
    {
        if(m_Player.grounded)
            m_rigidbody.AddForce(m_Player.jumpForce * Vector2.up);
    }

    IEnumerator SmoothMove(RaycastHit2D target)
    {
        GameObject enemy = target.collider.gameObject;
        Vector3 originalPos = enemy.transform.position;

        while((Vector3.Distance(originalPos, transform.position)) > 0.5f)
        { 
            transform.position = Vector3.Lerp(transform.position, originalPos, 8f * Time.deltaTime);

            yield return null;
        }
    }
}
