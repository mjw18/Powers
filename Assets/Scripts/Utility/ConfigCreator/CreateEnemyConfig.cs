using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

public class CreateEnemyConfig
{

    [MenuItem("Assets/Create/EnemyConfig")]
    public static void DoCreateEnemyConfig()
    {
        CreateConfig.DoCreateConfig<EnemyConfig>();
    }
}
#endif