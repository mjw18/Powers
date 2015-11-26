using UnityEngine;
using System.Collections;

public class ChargeEffect : VisualEffect
{
    public ParticleSystem collisionParticles;

    void Awake()
    {

    }

    public override void PlayEffect()
    {
        collisionParticles.Play();
    }
}
