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
    public TargetSelector targetSelector;

    protected Player player;
    protected Rigidbody2D playerRigidbody;
    protected Transform playerTransform;

    protected void Init()
    {
        player = GetComponent<Player>();
        playerTransform = player.transform;
        playerRigidbody = player.GetComponent<Rigidbody2D>();

        targetSelector = GameObject.Find("TargetSelector").GetComponent<TargetSelector>();

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

    void AquireTargets()
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

    virtual public void Execute()
    {
        foreach( var effect in powerConfig.visualEffects )
        {
            effect.visualEffect.PlayEffect();
        }
    }

}
