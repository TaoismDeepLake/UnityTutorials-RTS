using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraRenderSanitizer : MonoBehaviour
{
    [Tooltip("是否在新场景加载后自动执行一次清理")]
    public bool runOnSceneLoaded = true;

    [Tooltip("先只打印信息不改动；取消勾选则会真正禁用可疑组件/重置替换着色器")]
    public bool dryRun = false;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (runOnSceneLoaded)
            SceneManager.sceneLoaded += (_, __) => SanitizeAllCameras();
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= (_, __) => SanitizeAllCameras();
    }

    [ContextMenu("Sanitize Now")]
    public void SanitizeAllCameras()
    {
        var cams = FindObjectsOfType<Camera>(true);
        if (cams.Length == 0)
        {
            Debug.Log("[CamSanitizer] No cameras found.");
            return;
        }

        Debug.Log($"[CamSanitizer] Found {cams.Length} cameras. DryRun={dryRun}");

        foreach (var cam in cams)
        {
            Debug.Log($"[CamSanitizer] >>> Camera '{cam.name}'"
                    + $" | enabled={cam.enabled}"
                    + $" | depth={cam.depth}"
                    + $" | cullingMask=0x{cam.cullingMask:X}"
                    + $" | clearFlags={cam.clearFlags}"
                    + $" | targetTexture={(cam.targetTexture ? cam.targetTexture.name : "<null>")}");

            // 1) Replacement Shader
            // Unity 没有公开 getter 判断是否设置过 replacementShader，只能强制 Reset
            if (!dryRun)
            {
                cam.ResetReplacementShader();
            }
            Debug.Log($"[CamSanitizer] ResetReplacementShader() called on '{cam.name}'");

            // 2) 禁用所有带 OnRenderImage 的组件（最容易整屏覆盖）
            var mbs = cam.GetComponents<MonoBehaviour>();
            foreach (var mb in mbs)
            {
                if (mb == null) continue;
                var t = mb.GetType();

                // 是否实现了 OnRenderImage
                var hasOnRenderImage =
                    t.GetMethod("OnRenderImage", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly) != null;

                // 粗暴匹配常见后处理脚本名
                bool looksPostFx = t.Name.IndexOf("PostProcess", StringComparison.OrdinalIgnoreCase) >= 0
                                || t.Name.IndexOf("Bloom", StringComparison.OrdinalIgnoreCase) >= 0
                                || t.Name.IndexOf("Color", StringComparison.OrdinalIgnoreCase) >= 0
                                || t.Name.IndexOf("Vignette", StringComparison.OrdinalIgnoreCase) >= 0
                                || t.Name.IndexOf("FX", StringComparison.OrdinalIgnoreCase) >= 0
                                || t.FullName.IndexOf("PostProcessing", StringComparison.OrdinalIgnoreCase) >= 0;

                if (hasOnRenderImage || looksPostFx)
                {
                    Debug.Log($"[CamSanitizer]   Candidate image effect: {t.FullName} (enabled={mb.enabled})");
                    if (!dryRun && mb.enabled)
                    {
                        mb.enabled = false;
                        Debug.Log($"[CamSanitizer]   -> Disabled {t.FullName} on '{cam.name}'");
                    }
                }
            }
        }

        Debug.Log("[CamSanitizer] Done.");
    }
}
