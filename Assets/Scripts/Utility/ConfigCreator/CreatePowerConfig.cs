using UnityEngine;
using System.Collections;
using UnityEditor;

public class CreatePowerConfig {

    [MenuItem("Assets/Create/PowerConfig")]
	public static void DoCreatePowerConfig()
    {
        CreateConfig.DoCreateConfig<PowerConfig>();
    }
}
