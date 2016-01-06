using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Power function and helper functions 
// Play Visual FX and Audio for specific power
// Regulator? Cooldown uses regulator class or handle logic 
//      with cooldown timer Mono object

//Consider virtual "CanHitTarget" Function that does not allow selection of unreachable targets
public class Power : MonoBehaviour {

    public int cooldown;
    protected KeyCode keyCode;

    public PowerConfig powerConfig;
    public PowerUsageMode usageMode = PowerUsageMode.Unassigned;

    protected bool m_UsingPower = false;
    public bool canUsePower
    {
        get { return !m_UsingPower; }
    }

    public GameObject m_Player;
    public Transform target;
    protected TargetSelector targetSelector;
    protected RaycastHit2D m_Hit;

    protected Player player;
    protected Rigidbody2D playerRigidbody;
    protected Transform m_PlayerTransform;
    protected Transform m_ShootPosition;

    //Time control
    private Regulator refRegulator;
    public float slowTime = 0.2f;
    public EasingType timeEaseType = EasingType.Quadratic;
    public float slowTimeDuration = 0.6f;
    public float speedTimeDuration = 0.2f;

    protected void Init()
    {
        player = GetComponent<Player>();
        playerRigidbody = player.GetComponent<Rigidbody2D>();
        m_PlayerTransform = player.transform;
        m_ShootPosition = player.shootPosition;

        //Change this, init in GameController? specific to each power?
        GameObject tempSelector = GameObject.Find(Tags.targetSelector);
        if (tempSelector) targetSelector = tempSelector.GetComponent<TargetSelector>();
        targetSelector.maxRange = powerConfig.range;

        refRegulator = GameManager.instance.globalRegulator;

        m_Player = this.gameObject;

        if (usageMode == PowerUsageMode.Unassigned) Debug.Log("not yet");
        else SetKey();
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

    public void DamageTarget()
    {
        switch (powerConfig.damageType)
        {
            case DamageType.SingleTarget:
            case DamageType.SingleTargetDuration:
                Debug.Log("SingleTargetAcquisition");
                break;
            case DamageType.AreaEffect:
            case DamageType.AreaEffectDuration:
                Debug.Log("AreaEffectTargetAcuisition");
                break;
        }
    }

    void AcquireTargets()
    {
        //Negative input value sets to singlee target acquisition
        targetSelector.ResizeSelector(powerConfig.effectRadius);
        targetSelector.gameObject.SetActive(true);

        //On mouse click, select targets
        if(Input.GetMouseButtonDown(0))
        {
            targetSelector.SelectTargets(out m_Hit);
        }

        targetSelector.gameObject.SetActive(false);
    }

    public IEnumerator UsePrimaryPower()
    {
        m_UsingPower = true;

        yield return StartCoroutine( refRegulator.SlowTimeScale(slowTime, slowTimeDuration, Easing.EaseOut, timeEaseType) );
        yield return StartCoroutine( AcquireTarget() );
        yield return StartCoroutine( refRegulator.ResetTimeScale(speedTimeDuration, Easing.EaseOut, timeEaseType));
        Execute();

        m_UsingPower = false;
    }

    IEnumerator AcquireTarget()
    {
        targetSelector.origin = m_Player.transform.position;
        targetSelector.gameObject.SetActive(true);

        while (!Input.GetMouseButtonDown(0))
        {
            targetSelector.origin = player.shootPosition.position;
            //If user presses key again, exit. Switch later to hold and release
            if (Input.GetKeyDown(keyCode))
            {
                targetSelector.gameObject.SetActive(false);
                yield break;
            }

            yield return null;
        }

        //Mouse has been pressed and targets have been added
        targetSelector.SelectTargets(out m_Hit);
        targetSelector.gameObject.SetActive(false);
        
        //Write new yield command
        yield return new WaitForSeconds(0.2f);
    }

    //TODO : Establish keybindings in powerManager
    public void SetKey()
    {
        switch(usageMode)
        {
            case PowerUsageMode.Primary:
                //keyCode = m_Player.GetComponent<PlayerController>().primaryPowerKey;
                break;
            case PowerUsageMode.Secondary:
                //keyCode = m_Player.GetComponent<PlayerController>().secondaryPowerKey;
                break;
        }
    }

    //Have a switch here for usage mode exeute?
    virtual public void Execute()
    {
        foreach( var effect in powerConfig.visualEffects )
        {
            effect.visualEffect.PlayEffect();
        }
    }

    virtual public void ExecuteSecondary()
    {

    }

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
}




