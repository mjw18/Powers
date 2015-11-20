using UnityEngine;
using UnityEditor;

public class CreateEnemyConfig
{

    [MenuItem("Assets/Create/EnemyConfig")]
    public static void DoCreateEnemyConfig()
    {
        //Open save dialog window
        string path = EditorUtility.SaveFilePanel("Create EnemyConfig", "Assets/", "New Enemy Config", "asset");
        //If cancelled, path is empty string, return
        if (path == "")
        {
            return;
        }

        path = FileUtil.GetProjectRelativePath(path);

        //New scriptable object by create instnace
        EnemyConfig config = ScriptableObject.CreateInstance<EnemyConfig>();

        AssetDatabase.CreateAsset(config, path);

        //Commits all saves to disk
        AssetDatabase.SaveAssets();
    }

}
