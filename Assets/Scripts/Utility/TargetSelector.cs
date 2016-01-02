using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TargetSelector : MonoBehaviour {

    private CircleCollider2D m_Selector;
    private Transform m_Transform;

    private GameObject m_SelectorSprite;
    private CameraController m_CameraController;
    private LineRenderer m_LineRenderer;
    public List<GameObject> targets;
    public RaycastHit2D[] targetRayHit;
    public RaycastHit2D[] targetRayHitAlloc;
    public Vector2 origin;
    private Vector3 endPosition;

    public float maxRange;

    public float singleTargetRadius;
    public float radius = 0.1f;

    //Use start so that loader initializes first, load with loader? I think this is best. Maybe put in Game manager then
	void Start ()
    {
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
        m_Selector.radius = radius;
        m_SelectorSprite = GameObject.Find("SelectorSprite");
        gameObject.SetActive(false);
	}

	void Update ()
    {
        Vector3 hitPosition = Vector3.forward;

        endPosition = m_CameraController.GetMousePosition();
        //origin = Vector3.right;
        int j = Physics2D.RaycastNonAlloc(origin, endPosition - (Vector3)origin, targetRayHitAlloc, maxRange);
        for(int i = 0; i < j; i++ )
        {
            RaycastHit2D hit = targetRayHitAlloc[i];
            if(hit.collider.gameObject != gameObject && !hit.collider.CompareTag(Tags.player))
            {
                hitPosition = hit.point;
                break;
            }
        }

        if (hitPosition == Vector3.forward)
        {
            m_Transform.position = Vector3.ClampMagnitude(endPosition - (Vector3)origin, maxRange) + (Vector3)origin;
        }
        else
        {
            m_Transform.position = Vector3.SqrMagnitude((Vector3)origin - hitPosition) < Vector3.SqrMagnitude((Vector3)origin - endPosition) ? 
                                   hitPosition : Vector3.ClampMagnitude(endPosition - (Vector3)origin, maxRange) + (Vector3)origin;
        }

        SetLineBounds();
	}

    //Move to separate gameobject
    void SetLineBounds()
    {
        m_LineRenderer.SetPosition(0, origin);
        m_LineRenderer.SetPosition(1, transform.position);
    }

    //Resize selector, if input is empty then radius is set to initial radius
    public void ResizeSelector(float tRadius = -1.0f)
    {
        m_Selector.radius = (tRadius > 0f) ? tRadius : singleTargetRadius;
    }

    public void SelectTargets(out RaycastHit2D hit)
    {
        hit = new RaycastHit2D();

        if (targets.Count > 0) targets.Clear();

        Collider2D[] cols = Physics2D.OverlapCircleAll(m_Transform.position, radius);
        foreach (Collider2D col in cols)
        {
            //Fix this, I hate strings. Use a layermask in overlap
            if((col.CompareTag(Tags.enemy) || col.CompareTag(Tags.interactable) ) && col.gameObject != gameObject)
            {
                targets.Add(col.gameObject);
            }
        }

        foreach(var rayHit in Physics2D.RaycastAll(origin, transform.position - (Vector3)origin))
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
        Debug.Log(hit.point);
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


