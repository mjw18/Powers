using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Power function and helper functions 
// Play Visual FX and Audio for specific power
// Regulator? Cooldown uses regulator class or handle logic 
//      with cooldown timer Mono object

//Consider virtual "CanHitTarget" Function that does not allow selection of unreachable targets
public class Power : MonoBehaviour
{
    //Which Execute function of the power should be used
    public enum PowerUsageMode
    {
        Primary,
        Secondary,
        Defensive,
        Movement,
        Passive,
        Unassigned
    }

    public int cooldown;
    public KeyCode keyCode;

    public PowerConfig powerConfig;
    public PowerUsageMode usageMode = PowerUsageMode.Unassigned;

    protected bool doExecute = true; //>This is no good!!!
    protected bool m_UsingPower = false;
    public bool canUsePower
    {
        get { return !m_UsingPower; }
    }

    public GameObject m_Player;
    public Transform target;
    public GameObject targetSelector;
    protected TargetSelector m_TargetSelector;
    protected RaycastHit2D m_Hit;

    protected Player player;
    protected Rigidbody2D m_PlayerRigidbody;
    protected Transform m_PlayerTransform;
    protected Transform m_ShootPosition;

    //Time control
    protected Regulator refRegulator;
    public float slowTime = 0.2f;
    public EasingType timeEaseType = EasingType.Quadratic;
    public float slowTimeDuration = 0.6f;
    public float speedTimeDuration = 0.2f;

    //This must be called before use! Put in awake of inherited power
    protected void Init()
    {
        m_Player = GetComponentInParent<PowerManager>().playerGameObject;
        m_PlayerRigidbody = m_Player.GetComponent<Rigidbody2D>();
        m_PlayerTransform = m_Player.transform;

        //Reference to Player component
        player = m_Player.GetComponent<Player>();
        m_ShootPosition = player.shootPosition;

        //Instantiate target selector from reference
        //Some powers don't nedd a target selector. This allows that
        if(targetSelector != null)
        {
            GameObject tempSelector = Instantiate(targetSelector) as GameObject;
            tempSelector.transform.SetParent(transform);
            m_TargetSelector = tempSelector.GetComponent<TargetSelector>();
        }

        refRegulator = GameManager.instance.globalRegulator;
    }
    
    //public function to get power execution
    //Override if coroutine not used 
    virtual public void ExecutePower()
    {
        IEnumerator powerAction = UsePower();
        StartCoroutine(powerAction);
    }

    //main execution coroutine. Overriden by most powers
    virtual protected IEnumerator UsePower()
    {
        m_UsingPower = true;

        //Use try catch or the special coroutine?
        doExecute = true;

        yield return StartCoroutine(GetTargets(1f));

        if(doExecute) Execute();

        m_UsingPower = false;
    }

    //Just the slowdown select and speed up bits
    protected IEnumerator GetTargets(float duration = -1f)
    {
        yield return StartCoroutine(refRegulator.SlowTimeScale(slowTime, slowTimeDuration, Easing.EaseOut, timeEaseType));
        yield return StartCoroutine(AcquireTarget(duration));
        yield return StartCoroutine(refRegulator.ResetTimeScale(speedTimeDuration, Easing.EaseOut, timeEaseType));
    }

    //Calls the TargetSelectors select target function
    protected IEnumerator AcquireTarget(float duration = -1f)
    {
        m_TargetSelector.origin = m_Player.transform.position;
        m_TargetSelector.gameObject.SetActive(true);

        //Set up target time timer
        float t = 0.0f;
        float rate = -1f;
        if (duration > 0f) rate = 1f / duration;

        while (!Input.GetMouseButtonDown(0))
        {
            m_TargetSelector.origin = player.shootPosition.position;

            //If user presses key again, exit. Switch later to hold and release
            if (Input.GetKeyDown(keyCode) || t > 1f)
            {
                m_TargetSelector.gameObject.SetActive(false);
                doExecute = false;
                yield break;
            }

            //Increase timer in GAME TIME
            if(rate > 0f) t += rate * Time.deltaTime;

            yield return null;
        }

        //Mouse has been pressed and targets have been added
        m_TargetSelector.SelectTargets(out m_Hit);
        m_TargetSelector.gameObject.SetActive(false);
        
        //Write new yield command
        yield return new WaitForSeconds(0.2f);
        //Can Execute
        yield return true;
    }

    //Have a switch here for usage mode exeute?
    virtual protected void Execute()
    {
        foreach( var effect in powerConfig.visualEffects )
        {
            effect.visualEffect.PlayEffect();
        }
    }

    //Use AbilityVisualEffect Offset
    void SetEffectPosition(VisualEffect effect, VisualEffectPlacement effectPlacement, Vector3 offset)
    {
        switch (effectPlacement)
        {
            case VisualEffectPlacement.CenteredOnPlayer:
                effect.SetPosition(m_PlayerTransform.position);
                break;
            case VisualEffectPlacement.CenteredAtTarget:
                effect.SetPosition(target.position);
                break;
            case VisualEffectPlacement.OffsetPlayer:
                effect.SetPosition(m_PlayerTransform.position + offset);
                break;
            case VisualEffectPlacement.OffsetTarget:
                effect.SetPosition(target.position + offset);
                break;
            default:
                Debug.Log("Visual Effect Placement not set");
                break;
        }
    }
}




