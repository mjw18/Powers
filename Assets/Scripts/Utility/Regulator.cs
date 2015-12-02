using UnityEngine;
using System.Collections;

public class Regulator : MonoBehaviour {

    public float waitTime;
    //public bool canUse = true;
    private float m_startTime;
    private float m_timer = 0f;
    private float m_lastTime = 0f;
    private bool m_timing = false;

	void Awake ()
    {
        m_startTime = Time.realtimeSinceStartup;
        this.enabled = false;
	}
	
	void Update ()
    {
        if (m_timing) UpdateTimer();
	}

    //Udate Timer Independent of Time.timescale
    void UpdateTimer()
    {
        float currentTime = Time.realtimeSinceStartup;
        m_timer += currentTime - m_lastTime;
        m_lastTime = currentTime;
    }
    
    public void StartTimer()
    {
        this.enabled = true;
        m_startTime = Time.realtimeSinceStartup;
        m_timer = 0f;
        m_lastTime = m_startTime;
        m_timing = true;
    }

    public bool CheckTimer()
    {
        if(m_timer >= waitTime)
        {
            //try with returns only instead of boolean flags
            //canUse = true;

            //stop timer
            StopTimer();
            //return true
            return true;
        }

        return false;
    }

    public void StopTimer()
    {
        m_timer = 0;
        m_timing = false;
        this.enabled = false;
    }

}
