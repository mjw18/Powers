using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelConfig : ScriptableObject
{
    public LevelData data;
}

[System.Serializable]
public class LevelData
{
    List<Vector2> spawnPoints;
}