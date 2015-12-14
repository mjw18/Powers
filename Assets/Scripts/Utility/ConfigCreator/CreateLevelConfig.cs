using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
public class CreateLevelConfig {

    [MenuItem("Assets/Create/LevelConfig")]
    public static void DoCreateLevelConfig()
    {
        CreateConfig.DoCreateConfig<LevelConfig>();
    }
}
#endif