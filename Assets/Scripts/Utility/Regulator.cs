using UnityEngine;
using System.Collections;

public class Regulator : MonoBehaviour {

    public float waitTime;
    //public bool canUse = true;
    private float m_startTime;
    private float m_timer = 0f;
    private float m_lastTime = 0f;
    public bool m_timing = false;

    //DeltaTime calculated in real time
    private float deltaTime = 0.0f;
    public float realDeltaTime
    {
        get { return deltaTime; }
    }

    private float m_deltaLastTime = 0.0f;

	void Awake ()
    {
        m_startTime = Time.realtimeSinceStartup;
	}
	
	void Update ()
    {
        float cur = Time.realtimeSinceStartup;
        
        if (m_timing) UpdateTimer();

        deltaTime = cur - m_deltaLastTime;
        m_deltaLastTime = cur;
    }

    //Udate Timer Independent of Time.timescale
    void UpdateTimer()
    {
        float currentTime = Time.realtimeSinceStartup;
        m_timer += currentTime - m_lastTime;
        m_lastTime = currentTime;
    }

    //Use Start Timer to begin timing
    public void StartTimer()
    {
        //Cant start a running timer
        if (m_timing) return;

        m_startTime = Time.realtimeSinceStartup;
        m_timer = 0f;
        m_lastTime = m_startTime;
        m_timing = true;
    }

    //Use Check Timer, returns boolean if waitTime has passed since calling startTimer
    public bool CheckTimer()
    {
        if(m_timer >= waitTime)
        {
            //stop timer
            StopTimer();
            //return true
            return true;
        }

        return false;
    }

    //Stops timer and resets time value
    public void StopTimer()
    {
        m_timer = 0f;
        m_timing = false;
    }

    public IEnumerator SlowTimeScale(float slowFactor, float duration, Easing.EasingFunction easingFunc, EasingType easeType = EasingType.Linear )
    {
        float rate = 1f / duration;
        float t = 0.0f;
        float tempScale = Time.timeScale;

        while (t <= 1f)
        {
            Time.timeScale = Easing.InterpolateFloat(tempScale, slowFactor, t, easeType, easingFunc);
            //Multiply by real time delta Time
            t += rate * deltaTime;
            yield return null;
        }
        yield return null;
    }

    public IEnumerator ResetTimeScale(float duration, Easing.EasingFunction easingFunc, EasingType easeType = EasingType.Linear)
    {
        float rate = 1f / duration;
        float t = 0.0f;
        float tempScale = Time.timeScale;

        while (t <= 1f)
        {
            Time.timeScale = Easing.InterpolateFloat(tempScale, 1f, t, easeType, easingFunc);
            //Mult by real Time delta time
            t += rate * deltaTime;
            yield return null;
        }

        if (Time.timeScale != 1f) Time.timeScale = 1f;
        yield return null;
    }

    void OnDisable()
    {
        StopTimer();
    }
}
