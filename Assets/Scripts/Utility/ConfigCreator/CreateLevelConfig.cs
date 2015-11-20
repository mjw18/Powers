using UnityEngine;
using System.Collections;
using UnityEditor;

public class CreateLevelConfig {

    [MenuItem("Assets/Create/LevelConfig")]
    public static void DoCreateLevelConfig()
    {
        //Open save dialog window
        string path = EditorUtility.SaveFilePanel("Create LevelConfig", "Assets/", "New Level Config", "asset");
        //If cancelled, path is empty string, return
        if (path == "")
        {
            return;
        }

        path = FileUtil.GetProjectRelativePath(path);

        //New scriptable object by create instnace
        LevelConfig config = ScriptableObject.CreateInstance<LevelConfig>();

        AssetDatabase.CreateAsset(config, path);

        //Commits all saves to disk
        AssetDatabase.SaveAssets();
    }
}
