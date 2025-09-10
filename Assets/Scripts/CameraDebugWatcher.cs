using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class CameraDebugWatcher : MonoBehaviour
{
    private Camera mainCam;
    private string lastReplacementShader;
    private string lastTargetTexture;
    private string lastShaderName;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += (_, __) => FindMainCamera();
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= (_, __) => FindMainCamera();
    }

    void FindMainCamera()
    {
        mainCam = Camera.main;
        if (mainCam == null)
            Debug.LogWarning("[CamWatcher] No Main Camera found in current scene.");
        else
            Debug.Log($"[CamWatcher] Monitoring Camera: {mainCam.name}");
    }

    void LateUpdate()
    {
        if (mainCam == null) return;

        // 检查 targetTexture 是否被改
        string texName = mainCam.targetTexture ? mainCam.targetTexture.name : "<null>";
        if (texName != lastTargetTexture)
        {
            Debug.Log($"[CamWatcher] Camera '{mainCam.name}' targetTexture changed: {lastTargetTexture} -> {texName}");
            lastTargetTexture = texName;
        }

        // 检查是否有 replacementShader 被应用
        var camType = mainCam.GetType();
        var prop = camType.GetProperty("replacementShader", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Shader currentReplacement = prop?.GetValue(mainCam) as Shader;
        string currentReplacementName = currentReplacement ? currentReplacement.name : "<null>";

        if (currentReplacementName != lastReplacementShader)
        {
            Debug.Log($"[CamWatcher] Camera '{mainCam.name}' replacementShader changed: {lastReplacementShader} -> {currentReplacementName}");
            lastReplacementShader = currentReplacementName;
        }

        // 检查渲染的材质是否丢失
        var renderer = FindObjectsOfType<Renderer>().FirstOrDefault();
        if (renderer && renderer.sharedMaterial && renderer.sharedMaterial.shader)
        {
            string shaderName = renderer.sharedMaterial.shader.name;
            if (shaderName != lastShaderName)
            {
                Debug.Log($"[CamWatcher] Using shader: {shaderName}");
                lastShaderName = shaderName;
            }
        }
    }
}
