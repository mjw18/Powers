using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float chargeAttackDistance;

    public float chargeDamage = 75f;
    public float chargeForce = 10f;
    public float powerUpTime = 0.5f;
    public float powerUpRate = 0.05f;
    public float powerLossRate = 0.01f;
    public float chargeTime = 3f;

    public float charging = 0f;
    private bool m_PoweringUp = false;
    private bool m_CanMove = true;
    private bool m_Charging = false;

    public GameObject particles;

    public ParticleSystem activeParticles;
    private Player m_Player;

    public Text chargeTimeText;
    private Color defaultTextColor;

    private Rigidbody2D m_rigidbody;
    private Collider2D m_Collider;

	void Awake ()
    {
        m_Player = GetComponent<Player>();
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_Collider = GetComponent<Collider2D>();

        //Fuck using strings though
        chargeTimeText = GameObject.Find("EnergyText").GetComponent<Text>();
        if(chargeTimeText == null)
        {
            Debug.Log("Charge text not set. CHANGE THIS THIS SUCKS");
        }
        chargeTimeText.text = "Energy: " + chargeTime;
	}
	
	// Update is called once per frame
	void Update ()
    {

        //Update text
        chargeTimeText.text = string.Format("Energy: {0}", (int)charging ); 

        if (particles == null)
        {
            Debug.Log("No particles!");
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

        Flip(horz);

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
        else if(charging >= powerUpTime && !m_Charging && Input.GetButtonUp("Fire1"))
        {

            if (activeParticles != null && activeParticles.isPlaying)
            {
                activeParticles.Stop();
            }
            m_PoweringUp = false;
            
            ChargeAttack();
            m_Charging = false;
            //StartCoroutine("ChargeAttack", horz);
        }
        else if(!m_Charging)
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
        if(Mathf.Sign(horz) != Mathf.Sign(m_Player.facing))
        {
            m_Player.facing *= -1;
        }
    }

    void MoveManager(float horz, bool jump)
    {
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
        m_Charging = true;

        bool hasHit = false;

        RaycastHit2D[] hits = Physics2D.CircleCastAll(m_Player.transform.position, 3f, Vector2.right * m_Player.facing, chargeAttackDistance);

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

       m_Charging = false;
       charging = 0f;
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
        enemy.GetComponent<Rigidbody2D>().AddForce(-Vector3.Normalize(target.normal) * chargeForce, ForceMode2D.Impulse);
        m_rigidbody.velocity = Vector2.zero;
        target.collider.gameObject.GetComponent<Enemy>().ApplyDamage(chargeDamage); //Apply damage
    }
}
