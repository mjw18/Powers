using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TargetSelector : MonoBehaviour {

    protected Power m_Power;

    protected CircleCollider2D m_Selector; 

    protected Transform m_Transform;

    protected SpriteRenderer m_SelectorSprite;
    protected CameraController m_CameraController;
    protected LineRenderer m_LineRenderer;

    public List<GameObject> targets;
    public RaycastHit2D[] targetRayHit;
    public RaycastHit2D[] targetRayHitAlloc;

    public Vector2 origin;
    protected Vector3 endPosition;

    public float maxRange;

    public float singleTargetRadius;
    public float radius = 0.1f;

    //Use start so that loader initializes first, load with loader? I think this is best. Maybe put in Game manager then
	void Start ()
    {
        m_Power = GetComponentInParent<Power>();

        targetRayHitAlloc = new RaycastHit2D[10];

        m_Transform = transform;
        m_LineRenderer = GetComponent<LineRenderer>();

        if ( !(m_CameraController = GameObject.Find("Main Camera").GetComponent<CameraController>()))
        {
            Debug.Log("Camera Controller Failed to Init");
        }

        if (!(m_Selector = GetComponent<CircleCollider2D>()))
        {
            Debug.Log("Selector collider not assigned");
        }

        //Selector radius sets size of circle cast and targeted trigger
        radius = m_Power.powerConfig.effectRadius;
        m_Selector.radius = radius;

        //Set target selector range to power range
        maxRange = m_Power.powerConfig.range;

        m_SelectorSprite = GetComponent<SpriteRenderer>();
        if(!m_SelectorSprite) m_SelectorSprite = GameObject.Find("SelectorSprite").GetComponent<SpriteRenderer>();

        //Deactivate the target selector
        gameObject.SetActive(false);
	}

    void Update()
    {
        UpdateSelector();

        //Set the linerenderer's bounds
        SetLineBounds();
    }

	virtual protected void UpdateSelector ()
    {
        //Have to intialize htiPosition. Filled later with rayCast result
        Vector3 hitPosition = Vector3.forward;

        //Where the mouse is on given frame
        endPosition = m_CameraController.GetMousePosition();
        
        //Raycast towards mouse, get intersect
        int j = Physics2D.RaycastNonAlloc(origin, endPosition - (Vector3)origin, targetRayHitAlloc, maxRange);
        for(int i = 0; i < j; i++ )
        {
            RaycastHit2D hit = targetRayHitAlloc[i];
            if(hit.collider.gameObject != gameObject && (!hit.collider.CompareTag(Tags.player) || !hit.collider.CompareTag(Tags.ignoreTargetSelect) ) )
            {
                hitPosition = hit.point;
                break;
            }
        }

        //raycast didn't hit. Draw selector at mouse position
        if (hitPosition == Vector3.forward)
        {
            m_Transform.position = Vector3.ClampMagnitude(endPosition - (Vector3)origin, maxRange) + (Vector3)origin;
        }
        else
        {
            //Raycast hit, draw selector at closest point (hit or mouse)
            m_Transform.position = Vector3.SqrMagnitude((Vector3)origin - hitPosition) < Vector3.SqrMagnitude((Vector3)origin - endPosition) ? 
                                   hitPosition : Vector3.ClampMagnitude(endPosition - (Vector3)origin, maxRange) + (Vector3)origin;
        }
	}

    //Move to separate gameobject
    protected void SetLineBounds()
    {
        m_LineRenderer.SetPosition(0, origin);
        m_LineRenderer.SetPosition(1, transform.position);
    }

    //Resize selector, if input is empty then radius is set to initial radius
    public void ResizeSelector(float tRadius = -1.0f)
    {
        m_Selector.radius = (tRadius > 0f) ? tRadius : singleTargetRadius;
    }

    virtual public void SelectTargets(out RaycastHit2D hit)
    {
        hit = new RaycastHit2D();

        //Clear target list
        if (targets.Count > 0) targets.Clear();

        //Circle cast at target position
        Collider2D[] cols = Physics2D.OverlapCircleAll(m_Transform.position, radius);
        foreach (Collider2D col in cols)
        {
            //Cast against targetable objects
            if((col.CompareTag(Tags.enemy) || col.CompareTag(Tags.interactable) ) && col.gameObject != gameObject)
            {
                targets.Add(col.gameObject);
            }
        }

        foreach(var rayHit in Physics2D.RaycastAll(origin, transform.position - (Vector3)origin, maxRange))
        {
            Collider2D col = rayHit.collider;
            if((col.CompareTag(Tags.enemy) || col.CompareTag(Tags.interactable)) && col.gameObject != gameObject)
            {
                hit = rayHit;
                break;
            }
        }

        //Raycast didn't intersect anything, set point on null to clicked position
        if(hit.point == Vector2.zero) hit.point = m_CameraController.GetMousePosition();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag(Tags.enemy))
        {
            other.GetComponent<Enemy>().targeted = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(Tags.enemy))
        {
            //Send message?
            other.GetComponent<Enemy>().targeted = false;
        }
    }
}


