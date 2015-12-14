using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TargetSelector : MonoBehaviour {

    private CircleCollider2D m_Selector;
    private Transform m_Transform;

    public CameraController cameraController;
    public List<GameObject> targets;

    public float singleTargetRadius;
    public float radius = 0.1f;

	void Awake ()
    {
        m_Transform = transform;

        if (!(m_Selector = GetComponent<CircleCollider2D>()))
        {
            Debug.Log("Selector collider not assigned");
        }

        m_Selector.radius = radius;
	}

	void Update ()
    {
        if(Input.GetKeyDown("t"))
        {
            m_Transform.localScale *= 3;
            radius *= 3;
        }
        if (Input.GetKeyDown("u"))
        {
            m_Transform.localScale /= 3;
            radius /= 3;
        }

        if(Input.GetMouseButtonDown(1))
        {
            SelectTargets();
        }

        m_Transform.position = cameraController.GetMousePosition();  
	}

    //Resize selector, if input is empty then radius is set to initial radius
    public void ResizeSelector(float tRadius = -1.0f)
    {
        m_Selector.radius = (tRadius > 0f) ? tRadius : singleTargetRadius;
    }

    public void SelectTargets()
    {
        if (targets.Count > 0) targets.Clear();

        Collider2D[] cols = Physics2D.OverlapCircleAll(m_Transform.position, radius);
        foreach (Collider2D col in cols)
        {
            //Fix this, I hate strings. Use a layermask in overlap
            if(col.gameObject.tag != "Ground" && col.gameObject.tag != "TargetSelector")
            {
                targets.Add(col.gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            other.GetComponent<Enemy>().targeted = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            //Send message?
            other.GetComponent<Enemy>().targeted = false;
        }
    }
}


