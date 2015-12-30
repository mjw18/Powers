using UnityEngine;
using System.Collections;

public class PlatformMover : MonoBehaviour
{
    public float waitTime = 1f;
    public float distance;
    public bool StartAtTop = false;
    private float m_Direction = 1f;
    private SliderJoint2D m_SliderJoint;

    private Transform m_Transform;
    private GameObject m_Player;
    private Rigidbody2D m_Rigidbody;
    private bool m_PlayerOnPlatform;
    private Regulator m_ReturnRegulator;
    private Vector2 m_StartPos;

	// Use this for initialization
	void Start ()
    {
        m_SliderJoint = GetComponent<SliderJoint2D>();
        m_Transform = GetComponent<Transform>();
        m_Player = GameObject.FindGameObjectWithTag(Tags.player);
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_ReturnRegulator = GetComponent<Regulator>();
        m_StartPos = m_Transform.position;

        if (StartAtTop) m_Direction = -1f;

        float halfDistance = distance / 2f;
        //Set anchor in localSpace to half the total distance above platform
        //Ternary for start at bottom conditions

        m_SliderJoint.anchor = new Vector2(0.0f, m_Direction * -1 * halfDistance);
        m_SliderJoint.angle = m_Direction * 90f;
        //m_SliderJoint.anchor = !StartAtTop ? new Vector2(0.0f, - halfDistance) : new Vector2(0.0f, halfDistance);
        //m_SliderJoint.angle = !StartAtTop ? 90f : -90f;
        //Platform starts at bottom
        m_SliderJoint.connectedAnchor = m_Transform.position;
        JointTranslationLimits2D jointLimits = new JointTranslationLimits2D();
        jointLimits.min = -halfDistance;
        jointLimits.max = halfDistance;
        m_SliderJoint.limits = jointLimits;
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == m_Player)
        {
            m_PlayerOnPlatform = true;
            MovePlatform();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == m_Player)
        {
            m_PlayerOnPlatform = false;
        }
    }

    void Update()
    {
        if((m_Direction * (m_Transform.position.y - (m_StartPos.y + m_Direction * distance)) >= 0) && !m_PlayerOnPlatform)
        {
            m_ReturnRegulator.StartTimer();
        }
        if (m_ReturnRegulator.CheckTimer() && !m_PlayerOnPlatform && (m_Direction * ( m_Transform.position.y - (m_StartPos.y + m_Direction * distance) ) >= 0 ) )
        {
            ChangeDirection();
        }
    }

    void ChangeDirection()
    {
        m_SliderJoint.angle *= -1;
    }

    void MovePlatform()
    {
        m_ReturnRegulator.StopTimer();

        if (m_Transform.position.y - (m_StartPos.y + distance) >= 0)
        {
            m_SliderJoint.angle = m_Direction * 90f;

        }
        else
        {
            m_SliderJoint.angle = -1 * m_Direction * 90;
        }
    }
}
