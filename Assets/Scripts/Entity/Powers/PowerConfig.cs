using UnityEngine;
using System.Collections;

public class PowerConfig : ScriptableObject
{
    public float damage;
    public DamageType damageType;
    public float duration;
    public float energyCost;
    public float range;
    //If not set, will select single target acquisition
    public float effectRadius = -2.0f;

    [System.Serializable]
    public class AbilityVisualEffect
    {
        public VisualEffectPlacement placement;
        public VisualEffect visualEffect;
        public Vector3 offset = Vector3.zero;
    }

    public AbilityVisualEffect[] visualEffects;
}

public enum DamageType
{
    SingleTarget,
    SingleTargetDuration,
    AreaEffect,
    AreaEffectDuration    
}

