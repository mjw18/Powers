using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;

public class CreatePowerConfig {

    [MenuItem("Assets/Create/PowerConfig")]
	public static void DoCreatePowerConfig()
    {
        CreateConfig.DoCreateConfig<PowerConfig>();
    }
}
#endif