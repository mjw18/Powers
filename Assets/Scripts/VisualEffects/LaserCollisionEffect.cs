using UnityEngine;
using System.Collections;
using ExtendedEvents;

public class LaserCollisionEffect : VisualEffect {

    public ParticleSystem hitParticles;

    void Start()
    {
        //Particle reference is never established, this should come from a pool

        //hitParticles = GameObject.Find("LaserHit").GetComponent<ParticleSystem>();
        if (hitParticles == null) Debug.Log("No Particles here!");
        if(hitParticles.isPlaying) hitParticles.Stop();

        RegisterListeners();
    }

    //Register Listeners for the hit event
    void RegisterListeners()
    {
        UnityEngine.Events.UnityAction<LaserHitMessage> onLaserHit = PlayParticlesAt;
        EventManager.RegisterListener<LaserHitMessage>(onLaserHit);
    }

    public void PlayParticlesAt(LaserHitMessage hitMessage)
    {
        Collision2D hit = hitMessage.collision;

        Transform temp = hitParticles.transform;
        temp.position = hit.contacts[0].point;
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, hit.contacts[0].normal);

        temp.rotation = rot;
        hitParticles.Play();
    }
}
