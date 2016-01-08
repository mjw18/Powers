using UnityEngine;
using System.Collections;

public class PlatformMover : MonoBehaviour
{
    public float waitTime = 1f;
    public float distance;
    public bool StartAtTop = false;
    public bool moveBackFromMax = true;
    public float angle;

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

        float halfDistance = distance * 0.5f;

        //Set anchor in localSpace to half the total distance above platform
        //Move starting position (joint) to half way between two endpoints inclined at angle degrees
        m_SliderJoint.anchor = (new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle))) * (m_Direction * -1 * halfDistance);
        m_SliderJoint.angle = m_Direction * angle;

        //Set up joint limits to allow proper movement
        m_SliderJoint.connectedAnchor = m_Transform.position;
        JointTranslationLimits2D jointLimits = new JointTranslationLimits2D();
        jointLimits.min = -halfDistance;
        jointLimits.max = halfDistance;
        m_SliderJoint.limits = jointLimits;
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Tags.player) && !m_PlayerOnPlatform)
        {
            m_PlayerOnPlatform = true;
            if(!m_ReturnRegulator.m_timing) MovePlatform();
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
        float maxPlatformDisplacement = Vector2.SqrMagnitude((Vector2)m_Transform.position - m_StartPos) - distance * distance;

        //Is Platform at max displacement?
        if (maxPlatformDisplacement >= 0f && !m_PlayerOnPlatform && moveBackFromMax)
        {
            m_ReturnRegulator.StartTimer();
        }
        //If waiting time is over, player is no longer on platform and at max displacement, move back
        if (m_ReturnRegulator.CheckTimer() && !m_PlayerOnPlatform && maxPlatformDisplacement >= 0f)
        {
            ChangeDirection();
        }
    }

    //Switch the current direction of travel along line of movement
    void ChangeDirection()
    {
        //Is the angle greater than 180? If yes subtract 180 to keep angle between 0 and 360
        m_SliderJoint.angle += (m_SliderJoint.angle >= 180f ? -180f : 180f);
    }

    void MovePlatform()
    {
        //Platform no longer waiting for move back
        m_ReturnRegulator.StopTimer();

        //Is the platform at its max displacement?
        if(Vector2.SqrMagnitude((Vector2)m_Transform.position - m_StartPos) >= distance * distance)
        {
            //m_SliderJoint.angle = m_Direction * angle;
            ChangeDirection();

        }
        else
        {
            //m_SliderJoint.angle = -1 * m_Direction * angle;
            ChangeDirection();
        }
    }
}
