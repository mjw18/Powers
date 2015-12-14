using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;

public class CreateConfig {

    public static void DoCreateConfig<T>()
        where T : UnityEngine.ScriptableObject
    {
        //Open save dialog window
        string path = EditorUtility.SaveFilePanel("Create Config", "Assets/", "New Config", "asset");
        //If cancelled, path is empty string, return
        if (path == "")
        {
            return;
        }

        path = FileUtil.GetProjectRelativePath(path);

        //New scriptable object by create instnace
        T config = ScriptableObject.CreateInstance<T>();

        AssetDatabase.CreateAsset(config, path);

        //Commits all saves to disk
        AssetDatabase.SaveAssets();
    }
}
#endif