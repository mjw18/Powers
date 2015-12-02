using UnityEngine;
using System.Collections;

public class ChargeCollisionEffect : VisualEffect
{
    public ParticleSystem collisionParticles;
    public Transform target;

    void Awake()
    {
        collisionParticles.Pause();
    }

    public override void PlayEffect()
    {
        collisionParticles.transform.position = target.position;
        collisionParticles.Play();
    }
}
