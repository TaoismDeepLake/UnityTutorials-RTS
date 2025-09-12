using UnityEngine;
using UnityEditor;

public class RemoveMissingScripts : Editor
{
    [MenuItem("Tools/Cleanup/Remove Missing Scripts In Selection")]
    private static void RemoveMissingScriptsInSelection()
    {
        int count = 0;

        foreach (GameObject go in Selection.gameObjects)
        {
            // 递归清理选择对象及其子物体
            count += GameObjectUtility.RemoveMonoBehavioursWithMissingScript(go);
        }

        Debug.Log($"✅ 清理完成，移除了 {count} 个 Missing Script 组件。");
    }

    [MenuItem("Tools/Cleanup/Remove Missing Scripts In Prefabs Folder")]
    private static void RemoveMissingScriptsInFolder()
    {
        string folderPath = EditorUtility.OpenFolderPanel("选择Prefab所在文件夹", "Assets", "");
        if (string.IsNullOrEmpty(folderPath))
            return;

        // 将绝对路径转换为相对路径
        string relativePath = "Assets" + folderPath.Replace(Application.dataPath, "");

        string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab", new[] { relativePath });
        int count = 0;

        foreach (string guid in prefabGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            count += GameObjectUtility.RemoveMonoBehavioursWithMissingScript(prefab);
        }

        Debug.Log($"✅ 清理完成，共处理 {prefabGuids.Length} 个Prefab，移除了 {count} 个 Missing Script 组件。");
    }
}
