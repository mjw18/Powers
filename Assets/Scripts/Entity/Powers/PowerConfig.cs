using UnityEngine;
using System.Collections;

public class PowerConfig : ScriptableObject
{
    public float damage;
    public DamageType damageType;
    public TargetType targetType;
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

//The different damage types. Used for immunities and powers
public enum DamageType
{
    Physical,
    Energy
}

//Targeting types
public enum TargetType
{
    SingleTarget,
    SingleTargetDuration,
    AreaEffect,
    AreaEffectDuration    
}

