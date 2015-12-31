using UnityEngine;
using System.Collections;

// This script makes sure the world space UI is properly oriented 
// on moving/ rotating child objects

public class UIDirectionMover : MonoBehaviour {

    public bool useRelativeRotation = true;

    private Quaternion m_RelativeRotation;

	void Start ()
    {
        m_RelativeRotation = transform.parent.localRotation;
	}
	
	void Update ()
    {
	    if(useRelativeRotation)
        {
            transform.rotation = m_RelativeRotation;
        }
	}
}
