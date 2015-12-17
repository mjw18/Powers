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

    public GameObject m_Player;
    public Transform target;
    protected TargetSelector targetSelector;

    protected Player player;
    protected Rigidbody2D playerRigidbody;
    protected Transform playerTransform;

    //Time control
    private Regulator refRegulator;
    public float slowTime = 0.2f;
    public EasingType timeEaseType = EasingType.Quadratic;
    public float slowTimeDuration = 0.6f;
    public float speedTimeDuration = 0.2f;

    protected void Init()
    {
        player = GetComponent<Player>();
        playerTransform = player.transform;
        playerRigidbody = player.GetComponent<Rigidbody2D>();

        GameObject tempSelector = GameObject.Find("TargetSelector");
        if (tempSelector) targetSelector = tempSelector.GetComponent<TargetSelector>();

        refRegulator = GameManager.instance.globalRegulator;

        m_Player = this.gameObject;

        foreach (var effect in powerConfig.visualEffects)
        {
            SetEffectPosition(effect.visualEffect, effect.placement, effect.offset);
        }
    }

    //Use AbilityVisualEffect Offset
    void SetEffectPosition(VisualEffect effect, VisualEffectPlacement effectPlacement, Vector3 offset)
    {
        switch (effectPlacement)
        {
            case VisualEffectPlacement.CenteredOnPlayer:
                effect.SetPosition(playerTransform.position);
                break;
            case VisualEffectPlacement.CenteredAtTarget:
                effect.SetPosition(target.position);
                break;
            case VisualEffectPlacement.OffsetPlayer:
                effect.SetPosition(playerTransform.position + offset);
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

        if(Input.GetMouseButtonDown(0))
        {
            targetSelector.SelectTargets();
        }

        targetSelector.gameObject.SetActive(false);
    }

    public IEnumerator UsePower()
    {
        yield return StartCoroutine( refRegulator.SlowTimeScale(slowTime, slowTimeDuration, Easing.EaseOut, timeEaseType) );
        yield return StartCoroutine( AcquireTarget() );
        yield return StartCoroutine( refRegulator.ResetTimeScale(speedTimeDuration, Easing.EaseOut, timeEaseType));
        Execute();
    }

    IEnumerator AcquireTarget()
    {
        targetSelector.gameObject.SetActive(true);
        Debug.Log("Target acquisition phase");

        while (!Input.GetMouseButtonDown(0))
        {
            yield return null;
        }

        Debug.Log("The Mouse Button has been pressed and targets have been added"); 
        targetSelector.SelectTargets();
        Debug.Log(targetSelector.targets.Count);
        targetSelector.gameObject.SetActive(false);
        
        //Write new yield command
        yield return new WaitForSeconds(0.2f);
    }

    virtual public void Execute()
    {
        foreach( var effect in powerConfig.visualEffects )
        {
            effect.visualEffect.PlayEffect();
        }

    }

}
