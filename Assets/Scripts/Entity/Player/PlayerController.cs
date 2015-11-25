using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public KeyCode primaryPowerKey = KeyCode.E;
    public KeyCode secondaryPowerKey = KeyCode.R;
    public Power primaryPower;
    public Power secondaryPower; 

    //These Should probably be removed soon
    public float powerUpTime = 0.5f;
    public float powerUpRate = 0.05f;
    public float powerLossRate = 0.01f;
    public float chargeTime = 3f;

    public float charging = 0f;
    private bool m_PoweringUp = false;
    private bool m_CanMove = true;
    private bool m_UsingPower = false;

    //Charge Variables
    public static class ChargeData
    {
        public static float chargeAttackDistance;
        public static float damage = 75f;
        public static float chargeForce = 10f;
        public static float energyCost = 15f;
    }

    //Laser variables
    public class LaserData
    {
        public static float energyCost = 3f;
        public static float laserRange = 5.0f;
        public static float laserDamage = 4.0f;
        public static float laserFirerate = 1.0f;
        public static Color laserColor = new Color(80, 120, 120);
    }

    public GameObject particles;

    public ParticleSystem activeParticles;
    private Player m_Player;

    public Text chargeTimeText;     //> Delete?
    private Color defaultTextColor; //> Delete?

    private Rigidbody2D m_rigidbody;
    private Collider2D m_Collider;
    private LineRenderer m_LaserLine;
    private Regulator m_LaserRegulator;
    private Transform m_ShootPosition;

	void Awake ()
    {
        m_Player = GetComponent<Player>();
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_Collider = GetComponent<Collider2D>();
        m_LaserLine = GetComponent<LineRenderer>();
        m_LaserRegulator = GetComponent<Regulator>();
        //Change this, put in laser anymway?
        m_ShootPosition = GameObject.Find("ShootPosition").transform;
        if(!m_ShootPosition)
        {
            Debug.Log("ShootPosition not found");
        }

        //Dont show laser until fired
        m_LaserLine.enabled = false;

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

        //Check laser regulator, if true, disable laser vfx (time is up)
        if(m_LaserRegulator.CheckTimer())
        {
            DisableLaserVFX();
        }

    }

    void FixedUpdate()
    {
        //AddExplosionForce(.....ForceMode.Impulse)

        float horz = Input.GetAxis("Horizontal");
        bool jump = Input.GetButtonDown("Jump");

        if (m_CanMove)
        {
            MoveManager(horz, jump);
        }

        if(Input.GetKeyDown(primaryPowerKey))
        {
            if( m_Player.UseEnergy( ChargeData.energyCost ) )
            {
                if(primaryPower != null)
                    primaryPower.Execute();

                ChargeAttack();
            }
        }
        else if( Input.GetKeyDown(secondaryPowerKey) )
        {
            if (m_Player.UseEnergy(LaserData.energyCost))
            {
                if(secondaryPower != null)
                    secondaryPower.Execute();

                HandLaser();
            }
        }

        if (Input.GetButton("Fire1"))
        {
            if (activeParticles == null && particles != null)
            {
                Debug.Log(activeParticles);
                activeParticles = (Instantiate(particles, m_Player.transform.position, Quaternion.identity) as GameObject).GetComponent<ParticleSystem>();
                activeParticles.transform.SetParent(m_Player.transform);
            }
            else if (activeParticles.isStopped)
            {
                activeParticles.Play();
            }

            m_PoweringUp = true;
            m_CanMove = false;
            charging += powerUpRate;
            //StopCoroutine("ChargeAttack");
            charging = Mathf.Clamp(charging, 0.0f, powerUpTime);
            Debug.Log("ChargedTime: " + charging);
        }
        else if(charging >= powerUpTime && !m_UsingPower && Input.GetButtonUp("Fire1"))
        {

            if (activeParticles != null && activeParticles.isPlaying)
            {
                activeParticles.Stop();
            }
            m_PoweringUp = false;
            
            ChargeAttack();
            m_UsingPower = false;
            //StartCoroutine("ChargeAttack", horz);
        }
        else if(!m_UsingPower)
        {
            if (activeParticles != null && activeParticles.isPlaying)
            { 
                activeParticles.Stop();
            }
            charging = Mathf.Clamp(charging-powerLossRate, 0.0f, 5.0f);
            m_PoweringUp = false;
            m_CanMove = true;
        }
    }

    //Flips player facing if neccessary
    void Flip(float horz)
    {
        if(Mathf.Sign(horz) != Mathf.Sign(m_Player.facing) && Mathf.Abs(horz) > float.Epsilon)
        {
            m_Player.facing *= -1;
        }
    }

    void MoveManager(float horz, bool jump)
    {
        Flip(horz);
        Vector2 playerVelocity = Vector2.right * m_Player.maxSpeed * horz;
        Vector2 currentVel = new Vector2(playerVelocity.x, m_rigidbody.velocity.y);
        m_rigidbody.velocity = currentVel;

        if (jump && m_Player.grounded)
        {
            m_rigidbody.AddForce(m_Player.jumpForce * Vector2.up);
            m_Player.grounded = false;
        }
    }

    void ChargeAttack()
    {
        m_CanMove = false;
        m_UsingPower = true;

        bool hasHit = false;

        RaycastHit2D[] hits = Physics2D.CircleCastAll(m_Player.transform.position, 3f, Vector2.right * m_Player.facing, ChargeData.chargeAttackDistance);

        Debug.Log(hits.GetLength(0));

        foreach(RaycastHit2D hit in hits)
        {
            Debug.Log("Entered for each");
            if(hit.collider.tag == "Enemy" && !hasHit)
            {
                Debug.Log("Charge Attack Enemy at " + hit.point);
                Debug.Log(hit.normal);

                StartCoroutine(SmoothMove(hit));

                hasHit = true;
                EventManager.PostMessage(EventManager.MessageKey.ChargeHit);
            }
        }

       m_UsingPower = false;
       charging = 0f;
    }

    public void HandLaser()
    {
        LayerMask e = 1 << 9;
        Debug.Log(e.value);
        m_UsingPower = true;
        LayerMask enemyLayerMask = LayerMask.GetMask("Enemy");
        Ray laserRay = new Ray();
        laserRay.origin = m_ShootPosition.position;
        laserRay.direction = Vector3.right * m_Player.facing;

        m_LaserLine.SetPosition(0, m_ShootPosition.position);

        //Store raycast hit
        RaycastHit2D laserHit = Physics2D.Raycast(laserRay.origin, laserRay.direction, LaserData.laserRange, enemyLayerMask);

        if (!laserHit)
        {
            Debug.Log("Raycast returns null");
            //Draw a big line (FIX THIS I ADDED A RANDOM 5 FOR GRINS)
            m_LaserLine.SetPosition(1, m_ShootPosition.position + LaserData.laserRange * laserRay.direction * 5f);
        }
        else if (laserHit.collider.tag == "Player")
        {
            Debug.Log("You are hitting the player with this linecast");
        }
        else if (laserHit.collider.tag == "Enemy")
        {
            Debug.Log("You are hitting the enemy with this linecast");
            EventManager.PostMessage(EventManager.MessageKey.LaserHit);
            laserHit.transform.gameObject.GetComponent<Enemy>().ApplyDamage(LaserData.laserDamage); ;

            //End Laser shot at enemy
            m_LaserLine.SetPosition(1, laserHit.transform.position);
        }

        m_LaserLine.enabled = true;

        //Start regulator for vfx turn off. USE INVOKE?
        m_LaserRegulator.StartTimer();
    }

    void DisableLaserVFX()
    {
        m_LaserLine.enabled = false;
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

        //Reached target, add force. Move this into function?
        Debug.Log("Reached Target");
        enemy.GetComponent<Rigidbody2D>().AddForce(-Vector3.Normalize(target.normal) * ChargeData.chargeForce, ForceMode2D.Impulse);
        m_rigidbody.velocity = Vector2.zero;
        target.collider.gameObject.GetComponent<Enemy>().ApplyDamage(ChargeData.damage); //Apply damage
    }
}
