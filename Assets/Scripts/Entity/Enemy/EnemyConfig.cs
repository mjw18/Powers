using UnityEngine;
using System.Collections;

public class EnemyConfig : ScriptableObject
{
    public EnemyData data;
}

[System.Serializable]
public class EnemyData
{
    public float health;
    public float movementSpeed;
    public int level;

    //Copy Condtructor
    public EnemyData(EnemyData copy)
    {
        health = copy.health;
        movementSpeed = copy.movementSpeed;
        level = copy.level;
    }
}