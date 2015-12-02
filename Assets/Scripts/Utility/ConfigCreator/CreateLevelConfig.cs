using UnityEngine;
using System.Collections;
using UnityEditor;

public class CreateLevelConfig {

    [MenuItem("Assets/Create/LevelConfig")]
    public static void DoCreateLevelConfig()
    {
        CreateConfig.DoCreateConfig<LevelConfig>();
    }
}
