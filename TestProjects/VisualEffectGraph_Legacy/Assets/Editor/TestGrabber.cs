using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class TestGrabber
{
    static TestGrabber()
    {
        FileUtil.DeleteFileOrDirectory(Application.dataPath + "/AllTests");
        FileUtil.CopyFileOrDirectory(Application.dataPath + "/../../VisualEffectGraph/Assets/AllTests", Application.dataPath + "/AllTests");

        FileUtil.DeleteFileOrDirectory(Application.dataPath + "/ReferenceImages");
        FileUtil.CopyFileOrDirectory(Application.dataPath + "/../../VisualEffectGraph/Assets/ReferenceImages", Application.dataPath + "/ReferenceImages");

        FileUtil.DeleteFileOrDirectory(Application.dataPath + "/AllTests/Editor");
        FileUtil.ReplaceFile(Application.dataPath + "/../../VisualEffectGraph/ProjectSettings/EditorBuildSettings.asset", Application.dataPath + "/../ProjectSettings/EditorBuildSettings.asset");
    }
}
