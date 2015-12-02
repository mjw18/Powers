using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Power function and helper functions 
// Play Visual FX and Audio for specific power
// Regulator? Cooldown uses regulator class or handle logic 
//      with cooldown timer Mono object

//public class AbilityVisualEffect;

public class Power : MonoBehaviour {

    public int cooldown;
    protected KeyCode keyCode;

    public PowerConfig powerConfig;

    public Transform player;
    public Transform target;
    public TargetSelector targetSelector;

    void Init()
    {
        foreach (var effect in powerConfig.visualEffects)
        {
            SetPosition(effect.visualEffect, effect.placement, effect.offset);
        }
    }

    void SetPosition(VisualEffect effect, VisualEffectPlacement effectPlacement, Vector3 offset)
    {
        switch (effectPlacement)
        {
            case VisualEffectPlacement.CenteredOnPlayer:
                effect.SetPosition(player.position);
                break;
            case VisualEffectPlacement.CenteredAtTarget:
                effect.SetPosition(target.position);
                break;
            case VisualEffectPlacement.OffsetPlayer:
                effect.SetPosition(player.position + offset);
                break;
            case VisualEffectPlacement.OffsetTarget:
                effect.SetPosition(target.position + offset);
                break;
            default:
                Debug.Log("Visual Effect Placement not set");
                break;
        }
    }

    public void AcquireTarget()
    {
        //Clear current targets list
        if (targetSelector.targets.Count != 0)
            targetSelector.targets.Clear();

        targetSelector.gameObject.SetActive(true);

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

    void SingleTargetAcquire()
    {
    }

    virtual public void Execute()
    {
        foreach( var effect in powerConfig.visualEffects )
        {
            effect.visualEffect.PlayEffect();
        }
    }

}
