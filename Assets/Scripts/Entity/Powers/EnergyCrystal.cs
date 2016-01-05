using UnityEngine;
using System.Collections;

public class EnergyCrystal : Power
{
    public GameObject energyCrystalObject;

    void Awake()
    {
        base.Init();
    }

    public override void Execute()
    {
        if(targetSelector.targets.Count > 0) target = targetSelector.targets[0].GetComponent<Transform>();

        GameObject freeCrystal = GameManager.instance.GetObjectPool(energyCrystalObject).NextPooledObject(false);
         
        freeCrystal.transform.position = m_Hit.point;

        freeCrystal.SetActive(true);
        base.Execute();
    }
}