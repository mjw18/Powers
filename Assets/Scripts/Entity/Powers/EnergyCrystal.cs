using UnityEngine;
using System.Collections;

public class EnergyCrystal : Power
{
    public GameObject energyCrystalObject;

    void Awake()
    {
        base.Init();
    }

    //Primary power execute
    public override void Execute()
    {
        GameObject freeCrystal = GameManager.instance.GetObjectPool(energyCrystalObject).NextPooledObject(false);

        //If there is a target, 
        if (targetSelector.targets.Count > 0)
        {
            target = targetSelector.targets[0].GetComponent<Transform>();
            //Move to an "Enemy Frozen" method?
            target.GetComponent<Rigidbody2D>().gravityScale = 0.0f;
            target.GetComponent<Collider2D>().enabled = false;
            //Place crystal at targe location
            freeCrystal.transform.position = target.position;
            //Parent the target to the crystal
            target.SetParent(freeCrystal.transform);
        }
        else
        {
            freeCrystal.transform.position = m_Hit.point;
        }

        freeCrystal.SetActive(true);

        base.Execute();
    }

    public override void ExecuteSecondary()
    {
        base.ExecuteSecondary();
    }
}