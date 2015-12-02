using UnityEngine;
using UnityEditor;

public class CreateEnemyConfig
{

    [MenuItem("Assets/Create/EnemyConfig")]
    public static void DoCreateEnemyConfig()
    {
        CreateConfig.DoCreateConfig<EnemyConfig>();
    }
}
